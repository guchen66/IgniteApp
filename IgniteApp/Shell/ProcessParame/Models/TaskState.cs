using IT.Tangdao.Framework.Extensions;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IgniteApp.Shell.ProcessParame.Models
{
    public enum TaskState
    {
        Init,
        Running,
        Pause,
        Stop,
    }

    /// <summary>
    /// 进度报告类
    /// </summary>
    public class CalibrationProgress
    {
        public MotionCalibrationModel NewItem { get; set; }
    }
}