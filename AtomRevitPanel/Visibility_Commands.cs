using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI.Events;

using PanelView;
using DockApplicationBase;
using static DockApplicationBase.ExecuteProvider;

namespace AtomRevitPanel
{
    [Transaction(TransactionMode.Manual)]
    public partial class HidePanel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            DockablePaneId id = new DockablePaneId(AtomRevitPanel.dockPanelGuid);
            DockablePane dockablePane = commandData.Application.GetDockablePane(id);

            UIApplication uiapp = commandData.Application;

            uiapp.Application.DocumentOpened -=
                AtomRevitPanel.Application_DocumentOpened;

            // subscribe view activated event
            uiapp.ViewActivated -=
                AtomRevitPanel.Application_ViewActivated;

            uiapp.Application.DocumentChanged -=
                AtomRevitPanel.DocumentChangedHandler;

            //AtomRevitPanel.firstOpenDone = false;

            dockablePane.Hide();

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class ShowAtomPanel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;

            AtomRevitPanel.ShowPanel(uiapp);
            
            InvokeDockPageUpdate();

            InvokeDockViewModelUpdate();

            return Result.Succeeded;
        }
    }
}