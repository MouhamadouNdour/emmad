using AutoMapper;
using emmad.Context;
using emmad.Entity;
using emmad.Interface;
using emmad.Models;
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
                    var fileContent = new FileContentResult(photoByte, photoExtension == "jpg" ? "image/jpeg" : photoExtension);
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
                        request.Credentials = new NetworkCredential(appSettings.FTPusername, appSettings.FTPpasword);
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

        public IEnumerable GetClient(Administrateur administrateur, int idOrganisation )
        {
            var clients  = MasterContext.client
                         .Where(c => c.id_organisation == idOrganisation)
                         .Select(c => new
                         {
                             c.id,
                             c.prenom,
                             c.nom,
                             c.email,
                             c.telephone,
                             c.age,
                             c.id_organisation,
                             c.Organisation,
                             Photos = c.Photos
                                        .Where(p => p.id_client == c.id)
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

        public void DeleteClient(Administrateur connectedUser,int idOrganisation, int idClient)
        {

            var organisation = MasterContext.organisation.Find(idOrganisation);

            var client = MasterContext.client
                            .FirstOrDefault(c => c.id_organisation == organisation.id && c.id == idClient);

                   MasterContext.client.Remove(client);
                   MasterContext.SaveChanges();
        }

    }
}
