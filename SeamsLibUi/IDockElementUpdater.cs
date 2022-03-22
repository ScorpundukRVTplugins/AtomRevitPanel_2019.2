using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;

namespace DockApplicationBase
{
    public interface IDockElementUpdater
    {
        void ExecuteUpdate();
        void UpdateState(UIApplication uiapplication);
    }
}
