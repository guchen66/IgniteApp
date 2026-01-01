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