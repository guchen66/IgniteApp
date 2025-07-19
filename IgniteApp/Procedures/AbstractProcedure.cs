using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Procedures
{
    public class AbstractProcedure
    {
    }

    public class SimpleProcedure<T>
    {
        public readonly Func<T, IIgniteProcedure> ProcedureProvider;

        public SimpleProcedure(Func<T, IIgniteProcedure> procedureProvider)
        {
            ProcedureProvider = procedureProvider;
        }

        public IIgniteProcedure GetValue(T obj) => ProcedureProvider.Invoke(obj);
    }

    public class Test
    {
        public void Init()
        {
            //SimpleProcedure<Cameraable> simple = new SimpleProcedure<Cameraable>(item => item.Prodcure);
            // simple.GetValue(new Cameraable());

            CameraProvider cameraProvider = new CameraProvider(camera => camera.Init());
            cameraProvider.Execute("Camera1");
        }
    }

    public class Cameraable
    {
        public Action action;

        public void Init()
        {
        }
    }

    public class CameraProvider
    {
        private readonly Action<ICamera> _cameraAction;

        // 构造函数：接收一个操作相机的委托
        public CameraProvider(Action<ICamera> cameraAction)
        {
            _cameraAction = cameraAction;
        }

        // 执行相机操作（可扩展为支持不同相机ID）
        public void Execute(string cameraId)
        {
            //ICamera camera = GetCameraById(cameraId); // 根据ID获取具体相机实例
            //try
            //{
            //    _cameraAction?.Invoke(camera); // 执行委托操作
            //}
            //finally
            //{
            //    camera.Dispose();
            //}
        }
    }

    public interface ICamera
    {
        string CameraId { get; }

        void Init();                   // 初始化相机

        bool SnapImage();             // 拍照

        void SetParameters(string config); // 设置参数

        void Dispose();                // 释放资源
    }
}