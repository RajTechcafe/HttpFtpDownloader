using System;
using System.Collections.Generic;
using System.Text;

namespace DownloaderBussinessLayer
{
   public class DownloadStatusEventArgs : EventArgs
    {
        public long percentDownloaded { get; set; }
    }
}
