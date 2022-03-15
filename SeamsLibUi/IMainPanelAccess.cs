using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsLibUi
{
    public interface IMainPanelAccess<RvtUIApp, ExEv>
    {
        //RvtUIApp - Autodesk.Revit.UI.UIApplication
        Action<Action<RvtUIApp>> DefineExternalExecute { get; set; }

        //ExEv - Autodesk.Revit.UI.ExternalEvent
        ExEv ExternalExecuteCaller { get; set; }

        //PT - page type
        PT GetPageType<PT>();
    }
}
