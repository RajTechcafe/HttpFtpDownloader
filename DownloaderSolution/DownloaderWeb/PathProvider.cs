using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DownloaderWeb
{
   
        public interface IPathProvider
        {
            string MapPath(string path);
        Dictionary<string,string> MimeTypes { get; }
    }

    public class PathProvider : IPathProvider
    {
        private IHostingEnvironment _hostingEnvironment;
        private Dictionary<string, string> mimeTypes = new Dictionary<string, string>();

        public Dictionary<string, string> MimeTypes { get { return mimeTypes; }  }

        public PathProvider(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            mimeTypes.Add("jpeg", "image/jpg");
            mimeTypes.Add("jpg", "image/jpeg");
            mimeTypes.Add("png", "image/png");
            mimeTypes.Add("gif", "image/gif");
            mimeTypes.Add("pdf", "application/pdf");
            mimeTypes.Add("html", "text/html");
            mimeTypes.Add("rtf", "text/rtf");
            mimeTypes.Add("doc", "application/msword");
        }

        public string MapPath(string path)
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, path);
            return filePath;
        }
    }
    
}
