using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDevices.Camera.Manager
{
    public class ImageViewer
    {
        public void GetWindow()
        {
            HWindow hWindow = null;
            var handle = hWindow.Handle;
            HTuple windowHandle = new HTuple(handle.ToInt32());
        }
    }
}