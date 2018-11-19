using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloaderBussinessLayer
{
    public class FtpProtocolDownloader : IProtocolStrategy
    {
        
        int numberofParallelDownloads = 0;
        long responseSize = 0;
        DownloadResult downloadResult;
        int bufferSzie = 0;
        byte[] buffer = null;
        private  string networkHostName;
        private  string password;

        public FtpProtocolDownloader(string networkHostName, string password)
        {
            this.networkHostName = networkHostName;
            this.password = password;
            downloadResult = new DownloadResult();
        }
        public  DownloadResult DownloadFile(string url, int numberOfProceess,string dst)
        {
           
            if (numberOfProceess <= 0)
            {
                numberofParallelDownloads = Environment.ProcessorCount;
            }
            else
                numberofParallelDownloads = numberOfProceess;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

           
            try
            {
                var startTime = DateTime.Now;
               
               ParallelDownload(url, dst);
               
                downloadResult.TimeTaken = DateTime.Now.Subtract(startTime);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return downloadResult;
        }


       


        public long GetFileSize(string url)
        {
            try
            {
                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(url);
                ftpWebRequest.Credentials = new NetworkCredential(networkHostName, password);
                ftpWebRequest.Method = WebRequestMethods.Ftp.GetFileSize;

                long responseLength;
                using (WebResponse response = ftpWebRequest.GetResponse())
                {
                    responseLength = response.ContentLength;
                    downloadResult.ContentType = response.ContentType;
                    downloadResult.Size = responseLength;
                }
               
                return responseLength;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public void GetChunkSize(string url)
        {
            responseSize = GetFileSize(url);

            List<Range> readRanges = new List<Range>();

            bufferSzie = (int)responseSize / numberofParallelDownloads;

            if (bufferSzie > Int32.MaxValue)
            {
                bufferSzie = Int32.MaxValue;
                numberofParallelDownloads = (int)responseSize / bufferSzie;
            }

            buffer = new byte[bufferSzie];

        }

      

        public void ParallelDownload(string url,string destPath)
        {
            GetChunkSize(url);
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(url);
            ftpWebRequest.Credentials = new NetworkCredential(networkHostName,password);
            ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
           
            using (FtpWebResponse ftpWebResponse = ftpWebRequest.GetResponse() as FtpWebResponse)
            {
                
                using (FileStream fs = new FileStream(destPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    Stream ftpResponseStream = ftpWebResponse.GetResponseStream();
                    int read = 0;
                   
                        while ((read = ftpResponseStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fs.Write(buffer, 0, read);
                        }
                }
            }
          

        }

      
    }
}
