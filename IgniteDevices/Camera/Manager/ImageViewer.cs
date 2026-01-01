using HalconDotNet;

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