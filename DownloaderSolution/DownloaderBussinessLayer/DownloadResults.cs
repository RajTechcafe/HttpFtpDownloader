using System;
using System.Collections.Generic;
using System.Text;

namespace DownloaderBussinessLayer
{
    public class DownloadResult
    {
        public long Size { get; set; }
        public String FilePath { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public int ParallelDownloads { get; set; }
        public Helper.FileType fileType { get; set; }
        public long PercentDownloaded { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
