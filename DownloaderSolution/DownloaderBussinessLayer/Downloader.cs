using System;
using System.Collections.Generic;
using System.Text;

namespace DownloaderBussinessLayer
{
    public class Downloader
    {
        private IProtocolStrategy _protocolStrategy;
        public Downloader(IProtocolStrategy protocolStrategyl)
        {
            this._protocolStrategy = protocolStrategyl;
        }

        public DownloadResult Download (string url, int numberOfParallelDownloads, string destinationPath = "C:")
        {
           return _protocolStrategy.DownloadFile(url, numberOfParallelDownloads, destinationPath);
        }
    }
}
