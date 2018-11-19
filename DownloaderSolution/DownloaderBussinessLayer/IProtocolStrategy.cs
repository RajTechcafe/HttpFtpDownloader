using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DownloaderBussinessLayer
{
   public interface  IProtocolStrategy
    {
        
       DownloadResult DownloadFile(string url,int numberOfParallelDownloads,string destinationPath="C:");
    
    }
}
