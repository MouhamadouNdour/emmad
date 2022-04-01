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
    public class ClientService : IClient
    {

        private readonly MasterContext MasterContext;
        private readonly IMapper Mapper;
        private readonly AppSettings appSettings;

        public ClientService(MasterContext masterContext, IMapper mapper, IOptions<AppSettings> settings)
        {
            MasterContext = masterContext;
            Mapper = mapper;
            appSettings = settings.Value;
        }

        public CreateClientResponse CreateClient(Administrateur administrateur, CreateClientRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.nom))
            {
                throw new Exception("Nom : Vide");
            }
            if (string.IsNullOrWhiteSpace(model.prenom))
            {
                throw new Exception("Prenom : Vide");
            }
            if (string.IsNullOrWhiteSpace(model.email))
            {
                throw new Exception("Email : Vide");
            }
            if (string.IsNullOrWhiteSpace(model.telephone))
            {
                throw new Exception("Email : Vide");
            }

            var existingOrganisation = MasterContext.organisation
                            .FirstOrDefault(c => c.id == model.societe);


            if (existingOrganisation == null)
            {
                throw new Exception("L'organisation n'existe pas.");
            }

            var existingClient = MasterContext.client
                            .FirstOrDefault(c => c.email == model.email
                                                && c.id_organisation == model.societe);

            if (existingClient != null)
            {
                throw new Exception("L'adresse email du client " + model.email + " est déjà utilisé dans cette organisation.");
            }

            var client = Mapper.Map<Client>(model);
            client.id_organisation = model.societe;

            MasterContext.client.Add(client);
            MasterContext.SaveChanges();

            if (model.photos != null)
            {
                foreach (var photo in model.photos)
                {
                    var photoExtension = HelperService.GetFileExtension(photo);
                    byte[] photoByte = Convert.FromBase64String(photo);

                    string fileName = DateTime.Now.ToString() + "-" + client.nom + client.prenom + "." + photoExtension;
                    var fileContent = new FileContentResult(photoByte, photoExtension == "jpg" ? "image/jpeg" : "image/" + photoExtension);
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

                        var photoDB = new Photo() {
                            id_client = client.id,
                            lien = fileName
                        };

                        MasterContext.photo.Add(photoDB);
                        MasterContext.SaveChanges();
                    }
                    catch (WebException ex)
                    {
                        throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
                    }
                }
            }

            return Mapper.Map<CreateClientResponse>(client);
        }


        public IEnumerable GetClient(Administrateur administrateur,int idOrganisation, PageParameters pageParameters)
        {

            var clients  = MasterContext.client
                         .Join(MasterContext.organisation,
                         client => client.id_organisation,
                         organisation => organisation.id,
                         (client, organisation) => new {client, organisation})
                         .Where(c => c.client.id_organisation == idOrganisation
                                    && c.organisation.id_administrateur == administrateur.id)
                         .Skip((pageParameters.page - 1) * pageParameters.size)
                         .Take(pageParameters.size)
                         .Select(c => new
                         {
                             c.client.id,
                             c.client.prenom,
                             c.client.nom,
                             c.client.email,
                             c.client.telephone,
                             c.client.age,
                             c.client.id_organisation,
                             c.client.Organisation,
                             Photos = c.client.Photos
                                        .Where(p => p.id_client == c.client.id)
                                        .Select(p => new
                                        {
                                            p.id,
                                            p.lien,
                                            p.id_client
                                        })
                         })
                         .ToList();

            var photosResponse = new List<PhotoResponse>();
            var clientsResponse = new List<GetClientResponse>();

            foreach (var client in clients)
            {
                foreach(var photo in client.Photos)
                {
                    var photoResponse = new PhotoResponse { lien = "http://www.emmad.somee.com/media/"+photo.lien };
                    photosResponse.Add(photoResponse);
                }
                var clientResponse = new GetClientResponse
                {
                    id = client.id,
                    id_organisation = client.id_organisation,
                    nom = client.nom,
                    prenom = client.prenom,
                    email = client.email,
                    telephone = client.telephone,
                    age = client.age,
                    Organisation = client.Organisation,
                    Photos = photosResponse
                };
                clientsResponse.Add(clientResponse);
            }

            return clientsResponse;
        }

        public void DeleteClient(Administrateur connectedUser, int idOrganisation, int idClient)
        {

            var client = MasterContext.client.Find(idClient);

            if (client == null)
            {
                throw new Exception("Ce client n'existe pas.");
            }

            var organisation = MasterContext.organisation.Find(client.id_organisation);

            if (client == null)
            {
                throw new Exception("Ce client n'a pas d'organisation.");
            }

            if (organisation.id_administrateur != connectedUser.id)
            {
                throw new Exception("Vous n'avez pas les droits de supprimer ce rdv.");
            }

            var rdvs = MasterContext.rdv
                            .Where(r =>r.id_client == client.id)
                            .ToList();

            foreach (var rdv in rdvs)
            {
                MasterContext.rdv.Remove(rdv);
                MasterContext.SaveChanges();
            }

            var photos = MasterContext.photo
                            .Where(p => p.id_client == client.id)
                            .ToList();

            foreach (var photo in photos)
            {
                MasterContext.photo.Remove(photo);
                MasterContext.SaveChanges();
            }

            MasterContext.client.Remove(client);
            MasterContext.SaveChanges();

        }

        public ClientResponse Update(Administrateur administrateur, int idClient, UpdateClientRequest model)
        {
            var client = MasterContext.client.Find(idClient);

            if (client == null)
            {
                throw new Exception("Ce client n'existe pas.");
            }

            var organisation = MasterContext.organisation.Find(client.id_organisation);

            if (organisation == null)
            {
                throw new Exception("Vous n'avez pas le droit de modifier ce client.");
            }

            if (organisation.id_administrateur == administrateur.id)
            {
                // Copie du model au l'entite organisation
                Mapper.Map(model, client);
                MasterContext.client.Update(client);
                MasterContext.SaveChanges();

                return Mapper.Map<ClientResponse>(client);
            }
            else
            {
                throw new KeyNotFoundException("Vous n'avez pas les droits de modifier ce client.");
            }
        }
    }
}
