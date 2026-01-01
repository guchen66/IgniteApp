using IgniteApp.Common;

namespace IgniteApp.Extensions
{
    /// <summary>
    /// ViewModel 需实现的接口
    /// </summary>
    public interface IDialogProvider
    {
        void OnDialogOpened(DialogParameters parameters);

        // 返回输出参数（返回 DialogResult）
        DialogResult OnDialogClosing();
    }
}