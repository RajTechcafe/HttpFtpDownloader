using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DownloaderBussinessLayer.Helper
{
    public enum FileType
    {
        [Description("Images")]
        Images = 0,

        [Description("Files")]
        Files = 1,

        [Description("Videos")]
        Videos = 2,
    }
}
