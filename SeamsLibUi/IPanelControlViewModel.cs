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
        void SetDefineExternalExecute(Action<Action<UIApplication>> defineExecute);
        void SefExternalExecuteCaller(ExternalEvent exEvent);
    }
}
