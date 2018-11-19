using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DownloaderBussinessLayer
{
    public class HttpProtocolDownloader : IProtocolStrategy
    {
        public EventHandler<DownloadStatusEventArgs> DownloadingStatus;
        ConcurrentDictionary<long, string> tempFilesDictionary = new ConcurrentDictionary<long, string>();
       // int numberofParallelDownloads = 0;
        long responseSize = 0;
        DownloadResult downloadResult;
        List<Range> getReadRanges = new List<Range>();
        public HttpProtocolDownloader()
        {
            downloadResult = new DownloadResult();
        }

        protected virtual void OnDownloadStarted (DownloadStatusEventArgs e)
        {
            DownloadingStatus?.Invoke(this, e);
        }


        public long GetFileSize(string url)
        {
            try
            {
                WebRequest webRequest = HttpWebRequest.Create(url);
                webRequest.Method = "HEAD";
                long responseLength;
                using (WebResponse response = webRequest.GetResponse())
                {
                    responseLength = long.Parse(response.Headers.Get("Content-Length"));
                    downloadResult.Size = responseLength;
                    downloadResult.ContentType = response.ContentType;
                }
               
                return responseLength;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public List<Range> GetChunks(string url,int numberofParallelDownloads)
        {
            responseSize = GetFileSize(url);

            List<Range> readRanges = new List<Range>();

            for (int chunk = 0; chunk < numberofParallelDownloads - 1; chunk++)
            {
                var range = new Range()
                {
                    Start = chunk * (responseSize / numberofParallelDownloads),
                    End = ((chunk + 1) * (responseSize / numberofParallelDownloads)) - 1
                };
                readRanges.Add(range);
            }
            // Adding last chunk 
            readRanges.Add(new Range()
            {
                Start = readRanges.Any() ? readRanges.Last().End + 1 : 0,
                End = responseSize - 1
            });



            return readRanges;
        }

        public void ParallelDownload(string url, int numberofParallelDownloads)
        {

            int index = 0;
            getReadRanges = GetChunks(url, numberofParallelDownloads);
            Parallel.ForEach(getReadRanges, new ParallelOptions() { MaxDegreeOfParallelism = numberofParallelDownloads }, readRanges =>
            {
                HttpWebRequest httpWebRequest = HttpWebRequest.Create(url) as HttpWebRequest;
                httpWebRequest.Method = "GET";
                httpWebRequest.AddRange(readRanges.Start, readRanges.End);
             

                using (HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                       
                    string tempPath = Path.GetTempFileName();
                    using (FileStream fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        httpWebResponse.GetResponseStream().CopyTo(fs);
                        tempFilesDictionary.TryAdd(readRanges.Start, tempPath);
                    }
                }
                index++;
            });

            downloadResult.ParallelDownloads = index;
        }

        public void MergeParallelDownloadedChunk(string destinationPath)
        {
                
            using (FileStream destinationFs = new FileStream(destinationPath, FileMode.Append))
            {
                foreach (var tempFile in tempFilesDictionary.OrderBy(x => x.Key))
                {
                    long chunkDownloaded = getReadRanges.Where(x => x.Start == tempFile.Key).Select(y => y.End).FirstOrDefault();
                    double percentdownloaded = (chunkDownloaded/ Convert.ToDouble(downloadResult.Size))*100;
                    DownloadStatusEventArgs args = new DownloadStatusEventArgs();
                    args.percentDownloaded =(long) percentdownloaded;
                    OnDownloadStarted(args);
                    byte[] tempFileBytes = File.ReadAllBytes(tempFile.Value);
                    destinationFs.Write(tempFileBytes, 0, tempFileBytes.Length);
                    File.Delete(tempFile.Value);
                }
            }
        }


     
        public  DownloadResult DownloadFile(string url, int numberOfProceess,string destinationPath)
        {
            int numberofParallelDownloads = 0;
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
                ParallelDownload(url, numberofParallelDownloads);
                MergeParallelDownloadedChunk(destinationPath);
                downloadResult.TimeTaken = DateTime.Now.Subtract(startTime);
            }
            catch (Exception ex)
            {

                throw;
            }
            return downloadResult;
        }
    }

   
}
