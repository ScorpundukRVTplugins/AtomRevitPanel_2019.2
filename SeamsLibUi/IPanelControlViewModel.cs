using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsLibUi
{
    public interface IPanelControlViewModel
    {
        Action<Action<UIApplication>> DefineExternalExecute { get; set; }
        ExternalEvent ExternalExecuteCaller { get; set; }
    }
}
