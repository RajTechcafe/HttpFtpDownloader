using DownloaderBussinessLayer;
using System;

namespace Downloaderclient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //HttpProtocolDownloader httpProtocolDownloader = new HttpProtocolDownloader();
           // httpProtocolDownloader.DownloadingStatus += c_DownloadingStatus;
            //FtpProtocolDownloader httpProtocolDownloader = new FtpProtocolDownloader();
            // var result = httpProtocolDownloader.Download(@"https://empstoragetemp.blob.core.windows.net/employeesimages/head first design patterns - ora 2004.pdf",4);
            // var result = httpProtocolDownloader.Download(@"ftp://ftp.dlptest.com/head%20first%20design%20patterns%20-%20ora%202004.pdf", 4);
           // var result = httpProtocolDownloader.Download(@"ftp://ftp.dlptest.com/document.pdf", 4);
            //Console.WriteLine($"Location: {result.FilePath}");
            //Console.WriteLine($"Size: {result.Size}bytes");
            //Console.WriteLine($"Time taken: {result.TimeTaken.Milliseconds}ms");
            //Console.WriteLine($"Parallel: {result.ParallelDownloads}");

            
            Console.ReadKey();
        }

        static void c_DownloadingStatus(object sender, DownloadStatusEventArgs args)
        {
            Console.WriteLine($"DownloadPercentage: {args.percentDownloaded}");
        }
    }
}
