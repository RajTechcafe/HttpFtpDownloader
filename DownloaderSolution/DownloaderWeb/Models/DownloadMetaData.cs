using DownloaderBussinessLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DownloaderWeb.Models
{
    public class DownloadMetaData
    {
        public string downloadUrl { get; set; }
        public string FileName { get; set; }
        public string downloadPercentage { get; set; }
        public long Size { get; set; }
        public string FilePath { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public int ParallelDownloads { get; set; }
        public FileType FileType { get; set; }
        public int DownloadId { get; set; }
        public string Status { get; set; }
        public string ContentType { get; set; }

    }
}
