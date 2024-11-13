using IgniteApp.Bases;
using IT.Tangdao.Framework.DaoCommands;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using HandyControl.Controls;
using IgniteShared.Globals.Local;

namespace IgniteApp.Shell.Home.ViewModels
{
    public class ImageContextMenuViewModel:ControlViewModelBase
    {
        public ImageContextMenuViewModel()
        {
            SaveImageCommand = new TangdaoCommand<ImageSource>(ExecuteSaveImage);
        }

        private void ExecuteSaveImage(ImageSource imageSource)
        {
            // 确保ImageSource是BitmapSource类型
            if (imageSource is BitmapSource bitmapSource)
            {
                var openFilePath = DeviceInfoLocation.CameraPhotoPath;
                // 检查路径是否存在，如果不存在则使用默认路径
                if (!Directory.Exists(openFilePath))
                {
                    openFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                }
                // 创建SaveFileDialog
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp";
                saveFileDialog.Title = "保存图片";
              
                saveFileDialog.InitialDirectory = openFilePath;
                string currentTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                saveFileDialog.FileName = currentTime;
                // 显示保存文件对话框
                if (saveFileDialog.ShowDialog() == true)
                {
                    // 获取文件完整路径
                    string filename = saveFileDialog.FileName;
                 
                    // 根据文件扩展名选择编码器
                    BitmapEncoder encoder;
                    if (Path.GetExtension(filename).ToLower() == ".jpg")
                    {
                        encoder = new JpegBitmapEncoder();
                    }
                    else if (Path.GetExtension(filename).ToLower() == ".png")
                    {
                        encoder = new PngBitmapEncoder();
                    }
                    else
                    {
                        encoder = new BmpBitmapEncoder();
                    }

                    // 将BitmapSource添加到帧中并保存
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    using (var fileStream = new FileStream(filename, FileMode.Create))
                    {
                        encoder.Save(fileStream);
                        MessageBox.Success("保存成功");
                    }
                }
              
            }
            else
            {
                MessageBox.Show("不支持的图像源类型。");
            }
        }

        public void CreateGrayImage(string filePath)
        {
            //加载图像
            var curBitmap = (Bitmap)Image.FromFile(filePath);
            Rectangle rect = new Rectangle(0, 0, curBitmap.Width, curBitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = curBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, curBitmap.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = curBitmap.Width * curBitmap.Height * 3;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            double colorTemp = 0;
            for (int i = 0; i < rgbValues.Length; i += 3)
            {
                colorTemp = rgbValues[i + 2] * 0.299 + rgbValues[i + 1] * 0.587 + rgbValues[i] * 0.114;
                rgbValues[i] = rgbValues[i + 1] = rgbValues[i + 2] = (byte)colorTemp;
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            curBitmap.UnlockBits(bmpData);

        }

       
        public ICommand SaveImageCommand {  get; set; }


    }
}
