using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Events
{
    public class ImageInfoTranEvent
    {
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
    }
}