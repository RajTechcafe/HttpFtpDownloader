using DownloaderBussinessLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace DownloaderWeb.DataLayer
{
    public class DownloadContext : DbContext
    {
        public DownloadContext(DbContextOptions<DownloadContext> options)
            : base(options)
        { }

        public DbSet<Download> Downloads { get; set; }
      
    }

    public class Download
    {
        public int DownloadId { get; set; }
        public string downloadUrl { get; set; }
        public string FileName { get; set; }
        public string downloadPercentage { get; set; }
        public long Size { get; set; }
        public string FilePath { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public int ParallelDownloads { get; set; }
        public string FileType { get; set; }
        public string Status { get; set; }
        public string ContentType { get; set; }
    }
}
