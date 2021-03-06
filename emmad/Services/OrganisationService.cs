using AutoMapper;
using emmad.Context;
using emmad.Entity;
using emmad.Interface;
using emmad.Models;
using emmad.Parameter;
using emmad.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace emmad.Services
{
    public class OrganisationService : IOrganisation
    {
        private readonly MasterContext MasterContext;
        private readonly IMapper Mapper;
        private readonly AppSettings appSettings;

        public OrganisationService(MasterContext masterContext, IMapper mapper, IOptions<AppSettings> settings)
        {
            MasterContext = masterContext;
            Mapper = mapper;
            appSettings = settings.Value;
        }

        public CreateOrganisationResponse CreateOrganisation(Administrateur administrateur, CreateOrganisationRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.nom))
            {
                throw new Exception("Nom : Vide");
            }
            if (string.IsNullOrWhiteSpace(model.adresse))
            {
                throw new Exception("Adresse : Vide");
            }
            if (model.nb_salarie <= 0)
            {
                throw new Exception("Salarié : Doit être supérieur à 0");
            }

            var existingOrganisation = MasterContext.organisation
                                        .FirstOrDefault(o => o.nom == model.nom);

            if(existingOrganisation != null)
            {
                throw new Exception("L'organisation " + model.nom + " existe déjà.");
            }

            var organisation = Mapper.Map<Organisation>(model);
            organisation.id_administrateur = administrateur.id;

            if (!string.IsNullOrEmpty(model.logo))
            {
                var logoExtension = HelperService.GetFileExtension(model.logo);
                byte[] photoByte = Convert.FromBase64String(model.logo);

                string fileName = DateTime.Now.ToString() + "-" + organisation.nom + "." + logoExtension;
                Console.WriteLine(logoExtension);
                var fileContent = new FileContentResult(photoByte, logoExtension == "jpg" ? "image/jpeg" : "image/" + logoExtension);
                fileContent.FileDownloadName = fileName;
                fileName = fileName.Replace("/", "-").Replace(" ", "-").Replace(":", "-");
                Console.WriteLine(fileName);

                //FTP Server URL.
                string ftp = appSettings.FTPserver;

                //FTP Folder name. Leave blank if you want to upload to root folder.
                string ftpFolder = appSettings.FTPfolder;

                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    //Enter FTP Server credentials.
                    request.Credentials = new NetworkCredential(appSettings.FTPusername, appSettings.FTPpassword);
                    request.ContentLength = photoByte.Length;
                    request.UsePassive = true;
                    request.UseBinary = true;
                    request.ServicePoint.ConnectionLimit = photoByte.Length;
                    request.EnableSsl = false;

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(photoByte, 0, photoByte.Length);
                        requestStream.Close();
                    }

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Console.WriteLine(fileName + " uploaded.<br />");
                    response.Close();

                    organisation.logo = fileName;
                }
                catch (WebException ex)
                {
                    throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                }
            }

            MasterContext.organisation.Add(organisation);
            MasterContext.SaveChanges();

            return Mapper.Map<CreateOrganisationResponse>(organisation);
        }

        public IEnumerable GetOrganisation(Administrateur administrateur, PageParameters pageParameters)
        {
           var ListOrganisation = MasterContext.organisation
                        .Where(a => a.id_administrateur == administrateur.id)
                        .Skip((pageParameters.page - 1) * pageParameters.size)
                        .Take(pageParameters.size)
                        .ToList();

            List<Organisation> organisationsResponse = new List<Organisation>();

            foreach(var organisation in ListOrganisation)
            {
                var organisationResponse = new Organisation()
                {
                    id = organisation.id,
                    nom = organisation.nom,
                    adresse = organisation.adresse,
                    id_administrateur = organisation.id_administrateur,
                    nb_salarie = organisation.nb_salarie
                };

                if (!string.IsNullOrEmpty(organisation.logo))
                {
                    organisationResponse.logo = "http://www.emmad.somee.com/media/" + organisation.logo;
                }

                organisationsResponse.Add(organisationResponse);
            }

            return organisationsResponse;
        }

        public void DeleteOrganisation(Administrateur connectedUser, int idOrganisation)
        {
            var organisation = MasterContext.organisation.Find(idOrganisation);

            if (organisation == null)
            {
                throw new Exception("Cette organisation n'existe pas.");
            }

            if (organisation.id_administrateur != connectedUser.id)
            {
                throw new Exception("Vous n'avez pas les droits de supprimer cete organisation.");
            }

            var clients = MasterContext.client
                            .Where(c => c.id_organisation == organisation.id)
                            .ToList();

            foreach (var client in clients)
            {
                var rdvs = MasterContext.rdv
                            .Where(r => r.id_client == client.id)
                            .ToList();

                foreach (var rdv in rdvs)
                {
                    MasterContext.rdv.Remove(rdv);
                    MasterContext.SaveChanges();
                }

                MasterContext.client.Remove(client);
                MasterContext.SaveChanges();
            }

            MasterContext.organisation.Remove(organisation);
            MasterContext.SaveChanges();
        }

        public OrganisationResponse Update(Administrateur administrateur, int idOrganisation, UpdateOrganisationRequest model)
        {
            var organisation = MasterContext.organisation.Find(idOrganisation);

            if (organisation == null)
            {
                throw new Exception("Cette organisation n'existe pas.");
            }
            if (organisation.id_administrateur == administrateur.id)
            {
                // Traitement de l'image
                if (!string.IsNullOrEmpty(model.logo))
                {
                    var logoExtension = HelperService.GetFileExtension(model.logo);
                    byte[] photoByte = Convert.FromBase64String(model.logo);

                    string fileName = DateTime.Now.ToString() + "-" + organisation.nom + "." + logoExtension;
                    Console.WriteLine(logoExtension);
                    var fileContent = new FileContentResult(photoByte, logoExtension == "jpg" ? "image/jpeg" : "image/" + logoExtension);
                    fileContent.FileDownloadName = fileName;
                    fileName = fileName.Replace("/", "-").Replace(" ", "-").Replace(":", "-");
                    Console.WriteLine(fileName);

                    //FTP Server URL.
                    string ftp = appSettings.FTPserver;

                    //FTP Folder name. Leave blank if you want to upload to root folder.
                    string ftpFolder = appSettings.FTPfolder;

                    try
                    {
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + fileName);
                        FtpWebRequest requestDelete = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + organisation.logo);

                        request.Method = WebRequestMethods.Ftp.UploadFile;
                        requestDelete.Method = WebRequestMethods.Ftp.DeleteFile;

                        //Enter FTP Server credentials create request.
                        request.Credentials = new NetworkCredential(appSettings.FTPusername, appSettings.FTPpassword);
                        request.ContentLength = photoByte.Length;
                        request.UsePassive = true;
                        request.UseBinary = true;
                        request.ServicePoint.ConnectionLimit = photoByte.Length;
                        request.EnableSsl = false;

                        //Enter FTP Server credentials delete request.
                        requestDelete.Credentials = new NetworkCredential(appSettings.FTPusername, appSettings.FTPpassword);
                        requestDelete.ContentLength = photoByte.Length;
                        requestDelete.UsePassive = true;
                        requestDelete.UseBinary = true;
                        requestDelete.ServicePoint.ConnectionLimit = photoByte.Length;
                        requestDelete.EnableSsl = false;

                        FtpWebResponse responseDelete = (FtpWebResponse)requestDelete.GetResponse();
                        Console.WriteLine(organisation.logo + " deleted.<br />");
                        responseDelete.Close();

                        using (Stream requestStream = request.GetRequestStream())
                        {
                            requestStream.Write(photoByte, 0, photoByte.Length);
                            requestStream.Close();
                        }

                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        Console.WriteLine(fileName + " uploaded.<br />");
                        response.Close();

                        organisation.logo = fileName;
                        model.logo = fileName;
                    }
                    catch (WebException ex)
                    {
                        throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                    }
                }

                // Copie du model au l'entite organisation
                Mapper.Map(model, organisation);
                MasterContext.organisation.Update(organisation);
                MasterContext.SaveChanges();

                return Mapper.Map<OrganisationResponse>(organisation);
            }
            else
            {
                throw new KeyNotFoundException("Vous n'avez pas les droits de supprimer cete organisation.");
            }
        }

    }
    
}
