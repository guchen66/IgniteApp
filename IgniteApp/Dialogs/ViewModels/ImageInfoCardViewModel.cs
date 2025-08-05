using IgniteApp.Events;
using IT.Tangdao.Framework.DaoCommands;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IgniteApp.Dialogs.ViewModels
{
    public class ImageInfoCardViewModel : Screen, IHandle<ImageInfoTranEvent>
    {
        private string _filePath;

        public string FilePath
        {
            get => _filePath;
            set => SetAndNotify(ref _filePath, value);
        }

        private string _fileSize;

        public string FileSize
        {
            get => _fileSize;
            set => SetAndNotify(ref _fileSize, value);
        }

        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set => SetAndNotify(ref _fileName, value);
        }

        public IEventAggregator _eventAggregator { get; set; }

        public ImageInfoCardViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public void ExecuteBack()
        {
            RequestClose();
        }

        public void Handle(ImageInfoTranEvent message)
        {
            FilePath = message.FilePath;
            FileSize = message.FileSize;
            FileName = message.FileName;
        }
    }
}