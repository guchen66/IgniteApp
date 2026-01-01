using System;

namespace IgniteApp.Events
{
    public class ImageInfoTranEvent
    {
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}