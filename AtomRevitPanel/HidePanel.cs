using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI.Events;

using PanelView;

namespace AtomRevitPanel
{
    [Transaction(TransactionMode.Manual)]
    public partial class HidePanel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            DockablePaneId id = new DockablePaneId(AtomRevitPanel.dockPanelGuid);
            DockablePane dockablePane = commandData.Application.GetDockablePane(id);
            dockablePane.Hide();

            return Result.Succeeded;
        }
    }
}
