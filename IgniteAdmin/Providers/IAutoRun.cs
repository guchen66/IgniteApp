using System.Threading.Tasks;

namespace IgniteAdmin.Providers
{
    public interface IAutoRun
    {
        Task Run();
        Task Stop();
    }
}
