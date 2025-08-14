using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Dialogs.Manage
{
    public interface IPhotoView
    {
        string ViewName { get; }
        int DisplayOrder { get; }
    }
}