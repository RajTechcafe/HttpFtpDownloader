using DownloaderBussinessLayer.Helper;
using DownloaderWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DownloaderWeb.DataLayer
{
    public class DownloadRepo: IDownloadRepo
    {
        private DownloadContext _context;

        public DownloadRepo(DownloadContext _context)
        {
            this._context = _context;
        }

        public bool SaveDownloadDetails(DownloadMetaData info)
        {
            try
            {
                var downloadDetail = new Download()
                {
                    
                    downloadUrl = info.downloadUrl,
                    FileName = info.FileName,
                    FilePath = info.FilePath,
                    FileType = info.FileType.ToString(),
                    Size = info.Size,
                    TimeTaken = info.TimeTaken,
                    Status=info.Status,
                    ParallelDownloads=info.ParallelDownloads,
                    ContentType=info.ContentType
                };
                _context.Downloads.Add(downloadDetail);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
            

        }

        public List<DownloadMetaData> GetAllDownloads()
        {
            List<DownloadMetaData> lstDownloadedData = new List<DownloadMetaData>();
            try
            {
                var details = _context.Downloads.ToList();
                foreach (var item in details)
                {
                    lstDownloadedData.Add(new DownloadMetaData()
                    {
                        FileName = item.FileName,
                        downloadUrl = item.downloadUrl,
                        FilePath = GetVirtualPath(item.FilePath),
                        FileType = (FileType)Enum.Parse(typeof(FileType), (item.FileType)),
                        ParallelDownloads = item.ParallelDownloads,
                        Size = item.Size,
                        TimeTaken = item.TimeTaken,
                        DownloadId=item.DownloadId,
                        Status=item.Status,
                        ContentType=item.ContentType
                       

                    });
                }
                return lstDownloadedData;
            }
            catch (Exception ex)
            {

                throw;
            }
            
          
        }

        public List<DownloadMetaData> UpdateStatusOfFile(int Id, string Status)
        {
            try
            {
                var originaldata = (from data in _context.Downloads
                                    where data.DownloadId == Id
                                    select data).FirstOrDefault();
                originaldata.Status = Status;
                //Download updatedfield = new Download()
                //{
                //    downloadUrl = info.downloadUrl,
                //    FileName = info.FileName,
                //    FilePath = info.FilePath,
                //    FileType = info.FileType.ToString(),
                //    Size = info.Size,
                //    TimeTaken = info.TimeTaken
                //};

                _context.Update(originaldata);
                 _context.SaveChanges();
               return GetAllDownloads();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        private string GetVirtualPath(string filePath)
        {
            string dir = Directory.GetCurrentDirectory();
            var virtualpath = filePath.Replace(dir + @"\wwwroot", "~").Replace(@"\", "/");
            return virtualpath;
        }
    }

    public interface IDownloadRepo
    {
        bool SaveDownloadDetails(DownloadMetaData info);
        List<DownloadMetaData> GetAllDownloads();
        List<DownloadMetaData> UpdateStatusOfFile(int Id, string Status);
    }
}
