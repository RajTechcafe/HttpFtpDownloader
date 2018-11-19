using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DownloaderWeb.Models;
using DownloaderBussinessLayer;
using System.IO;
using DownloaderBussinessLayer.Helper;
using DownloaderWeb.DataLayer;
using Microsoft.Extensions.Configuration;

namespace DownloaderWeb.Controllers
{
    public class DownloadController : Controller
    {
        public static int dlpercent = 0;
        private IPathProvider pathProvider;
        private IDownloadRepo downloadRepo;
        private IProtocolStrategy _protocolStrategy;
        private Downloader _downloader;
        private IConfiguration iConfig;
        
        public DownloadController(IPathProvider pathProvider, IDownloadRepo downloadRepo, IConfiguration iConfig)
        {
            this.pathProvider = pathProvider;
            this.downloadRepo = downloadRepo;
            this.iConfig = iConfig;

        }
       

        // GET: Download
        public ActionResult Index()
        {
            return View();
        }


        // POST: Download/Create
        [HttpPost]
        public ActionResult DownloadFile(DownloadMetaData info)
        {
            DownloadResult result = null;
            if (ModelState.IsValid)
            {
                try
                {

                    info.FileName = GetUniqueName(info.downloadUrl);
                    string destinationPath = pathProvider.MapPath(info.FileName);
                    info.FileType = GetFileTye(info.downloadUrl);
                    string requestType = GetDownloadType(info.downloadUrl);
                    requestType = requestType.Contains("https") == true ? "http" : requestType;
                    switch (requestType)
                    {
                       
                        case "http":
                            _protocolStrategy = new HttpProtocolDownloader();
                           _downloader = new Downloader(_protocolStrategy);
                           result= _downloader.Download(info.downloadUrl, info.ParallelDownloads, destinationPath);
                            break;
                        case "ftp":
                            _protocolStrategy = new FtpProtocolDownloader(iConfig.GetSection("MyFtpSetting").GetSection("NetworkHostName").Value, iConfig.GetSection("MyFtpSetting").GetSection("Password").Value);
                            _downloader = new Downloader(_protocolStrategy);
                           result=_downloader.Download(info.downloadUrl, info.ParallelDownloads, destinationPath);
                            break;
                        default:
                            _protocolStrategy = null;
                            break;
                    }
                    if (_protocolStrategy == null)
                    {
                        ModelState.AddModelError("ProtocolError", "This protocol is currently not supported");
                        return View();
                    }
                    else
                    {
                        if(result==null)
                        {
                            ModelState.AddModelError("DownloadError", "Something went wrong, Please try again");
                            return View();
                        }
                        info.FilePath = destinationPath.ToString();
                        info.Size = result.Size;
                        info.TimeTaken = result.TimeTaken;
                        info.Status = "Processing";
                        info.ContentType = result.ContentType;

                        downloadRepo.SaveDownloadDetails(info);
                        var details = downloadRepo.GetAllDownloads();



                        return View(details);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", "Please try again check your source url");
                    return View();
                }
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }
        // GET: Download/Create

        public ActionResult Approve(int id)
        {
            var updatedList = downloadRepo.UpdateStatusOfFile(id, "Approved");
            return View("DownloadFile", updatedList);
        }

        public ActionResult Reject(int id)
        {
            var updatedList = downloadRepo.UpdateStatusOfFile(id, "Rejected");
            return View("DownloadFile", updatedList);
        }

        
        
        public FileResult OpenFile(string virtualPath,string extension)
        {
            string fullextension = pathProvider.MimeTypes.Where(x => x.Key == extension).Select(y => y.Value).SingleOrDefault();
            return File(virtualPath, extension);

        }

        #region Private Method
        private string GetUniqueName(string url)
        {
            string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(url);
            return uniqueName;


        }

        private string GetDownloadType(string url)
        {

            int index = url.IndexOf("://");

            if (index > 0)
            {
                string prefix = url.Substring(0, index);
                return prefix;
            }
            else
            {
                return null;
            }

        }

        private FileType GetFileTye(string url)
        {
            string extension = Path.GetExtension(url).Substring(1);
            switch (extension)
            {
                case "jpeg":
                    return FileType.Images;

                case "jpg":
                    return FileType.Images;

                case "png":
                    return FileType.Images;

                default:
                    return FileType.Files;


            }
        }

        #endregion
        public void c_DownloadingStatus(object sender, DownloadStatusEventArgs args)
        {
            
            // info.downloadPercentage = args.percentDownloaded.ToString();

        }



    }
}