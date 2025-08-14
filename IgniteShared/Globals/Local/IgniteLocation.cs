using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Globals.Local
{
    public static class IgniteLocation
    {
        /// <summary>
        /// It represents the path where the "Accelerider.Windows.exe" is located.
        /// </summary>
        public static readonly string Base = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// It represents the path where the current assembly (*.dll file) is located.
        /// </summary>
        public static string CurrentAssembly => Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

        /// <summary>
        /// %AppData%\Accelerider
        /// </summary>
        public static readonly string AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ignite");

        /// <summary>
        /// %AppData%\Accelerider\Apps
        /// </summary>
        //public static readonly string Apps = Path.Combine(IgniteInfoLocation.LoggerPath, nameof(Apps));

        /// <summary>
        /// %AppData%\Accelerider\Logs
        /// </summary>
        public static readonly string Logs = Path.Combine(AppData, nameof(Logs));

        /// <summary>
        /// %AppData%\Accelerider\Database
        /// </summary>
        public static readonly string Database = Path.Combine(AppData, nameof(Database));

        /// <summary>
        /// %AppData%\Accelerider\Temp
        /// </summary>
        public static readonly string Temp = Path.Combine(AppData, nameof(Temp));
    }
}