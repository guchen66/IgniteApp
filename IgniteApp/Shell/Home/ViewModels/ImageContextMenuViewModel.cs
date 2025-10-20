using IgniteApp.Bases;
using IT.Tangdao.Framework.Commands;
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
using Stylet;
using IgniteApp.Events;
using IgniteApp.Dialogs.ViewModels;
using StyletIoC;
using IT.Tangdao.Framework.Extensions;

namespace IgniteApp.Shell.Home.ViewModels
{
    public class ImageContextMenuViewModel : ViewModelBase
    {
        public ImageContextMenuViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            SaveImageCommand = new TangdaoCommand<ImageSource>(ExecuteSaveImage);
            GetImageInfoCommand = new TangdaoCommand<ImageSource>(ExecuteGetImageInfo);
        }

        private void ExecuteGetImageInfo(ImageSource imageSource)
        {
            if (imageSource == null) return;

            try
            {
                string filePath = string.Empty;
                long fileSize = 0;
                DateTime? creationTime = null;
                DateTime? lastWriteTime = null;
                Size imageSize = Size.Empty;
                if (imageSource is BitmapImage bitmapImage && bitmapImage.UriSource != null)
                {
                    // bitmapImage.BaseUri
                }
                // 处理基于文件的图像（如从文件加载的BitmapImage）
                //if (imageSource is BitmapImage bitmapImage && bitmapImage.UriSource != null)
                //{
                //    filePath = bitmapImage.UriSource.LocalPath;
                //    var fileInfo = new FileInfo(filePath);

                //    fileSize = fileInfo.Length;
                //    creationTime = fileInfo.CreationTime;
                //    lastWriteTime = fileInfo.LastWriteTime;

                //    // 获取实际图像尺寸
                //    using (var img = System.Drawing.Image.FromFile(filePath))
                //    {
                //        imageSize = img.Size;
                //    }
                //}
                //// 处理其他可能的ImageSource类型（如MemoryStream）
                //else if (imageSource is BitmapFrame bitmapFrame)
                //{
                //    // 尝试获取文件路径（如果是从文件加载的）
                //    if (bitmapFrame.Decoder is BitmapDecoder decoder &&
                //        decoder.Frames.Count > 0 &&
                //        decoder.Frames[0] is BitmapFrame frame)
                //    {
                //        filePath = frame.ToString(); // 可能是URI

                //        if (Uri.TryCreate(filePath, UriKind.Absolute, out var uri) && uri.IsFile)
                //        {
                //            filePath = uri.LocalPath;
                //            var fileInfo = new FileInfo(filePath);

                //            fileSize = fileInfo.Length;
                //            creationTime = fileInfo.CreationTime;
                //            lastWriteTime = fileInfo.LastWriteTime;
                //        }
                //    }

                //    // 获取图像尺寸（即使没有文件路径）
                //    imageSize = new Size(bitmapFrame.PixelWidth, bitmapFrame.PixelHeight);
                //}
                ImageInfoTranEvent imageInfoTranEvent = new ImageInfoTranEvent()
                {
                    FilePath = filePath,
                    FileName = "Memory Image",
                    FileSize = fileSize.ToString(),
                    CreateTime = creationTime.Value,
                    UpdateTime = lastWriteTime.Value,
                };
                // 组装信息对象
                //var imageInfo = new
                //{
                //    FileName = !string.IsNullOrEmpty(filePath) ? Path.GetFileName(filePath) : "Memory Image",
                //    FilePath = filePath,
                //    FileSize = fileSize,
                //    FileSizeKB = fileSize > 0 ? fileSize / 1024 : 0,
                //    Dimensions = $"{imageSize.Width} x {imageSize.Height}",
                //    CreationTime = creationTime,
                //    LastWriteTime = lastWriteTime
                //};
                // ImageInfoCardViewModel = ServiceLocator.GetService<ImageInfoCardViewModel>();
                //WindowManager = ServiceLocator.GetService<IWindowManager>();
                _windowManager.ShowWindow(ImageInfoCardViewModel);
                _eventAggregator.Publish(imageInfoTranEvent);
            }
            catch (Exception ex)
            {
                // 处理异常（记录日志或显示错误）
                Console.WriteLine($"获取图片信息失败: {ex.Message}");
            }
        }

        private void ExecuteSaveImage(ImageSource imageSource)
        {
            // 确保ImageSource是BitmapSource类型
            if (imageSource is BitmapSource bitmapSource)
            {
                var openFilePath = IgniteInfoLocation.Images;
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

                    StringComparer comparer = StringComparer.OrdinalIgnoreCase;
                    // 根据文件扩展名选择编码器
                    BitmapEncoder encoder;
                    if (Path.GetExtension(filename).EqualsIgnoreCase(".jpg"))
                    {
                        encoder = new JpegBitmapEncoder();
                    }
                    else if (Path.GetExtension(filename).EqualsIgnoreCase(".png"))
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
                else
                {
                    MessageBox.Success("保存图片已取消");
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

        public ICommand SaveImageCommand { get; set; }
        public ICommand GetImageInfoCommand { get; set; }
        public IEventAggregator _eventAggregator { get; set; }
        public IWindowManager _windowManager { get; set; }

        [Inject]
        public ImageInfoCardViewModel ImageInfoCardViewModel { get; set; }
    }
}