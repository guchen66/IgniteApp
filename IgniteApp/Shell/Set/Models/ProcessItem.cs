using IT.Tangdao.Framework.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Shell.Set.Models
{
    public class ProcessItem:DaoViewModelBase
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// 上料
        /// </summary>
        private bool _isFeeding;

        public bool IsFeeding
        {
            get => _isFeeding;
            set => SetProperty(ref _isFeeding, value);
        }

        /// <summary>
        /// 电路板制造
        /// </summary>
        private bool _isBoardMade;

        public bool IsBoardMade
        {
            get => _isBoardMade;
            set => SetProperty(ref _isBoardMade, value);
        }

        /// <summary>
        /// 电路板检测
        /// </summary>
        private bool _isBoardCheck;

        public bool IsBoardCheck
        {
            get => _isBoardCheck;
            set => SetProperty(ref _isBoardCheck, value);
        }

        /// <summary>
        /// 密封测试
        /// </summary>
        private bool _isSeal;

        public bool IsSeal
        {
            get => _isSeal;
            set => SetProperty(ref _isSeal, value);
        }

        /// <summary>
        /// 安全测试
        /// </summary>
        private bool _isSafe;

        public bool IsSafe
        {
            get => _isSafe;
            set => SetProperty(ref _isSafe, value);
        }
        /// <summary>
        /// 装药
        /// </summary>
        private bool _isCharge;

        public bool IsCharge
        {
            get => _isCharge;
            set => SetProperty(ref _isCharge, value);
        }

        /// <summary>
        /// 下料
        /// </summary>
        private bool _isBlanking;

        public bool IsBlanking
        {
            get => _isBlanking;
            set => SetProperty(ref _isBlanking, value);
        }

    }
}
