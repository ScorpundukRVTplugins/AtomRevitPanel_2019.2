using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Interop;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI.Events;

using PanelView;

namespace AtomRevitPanel
{
    public partial class AtomRevitPanel : IExternalApplication
    {
        public static MainPage dockPanelView = null;
        //public static MainWindow dockPanelView = null;
        public static Guid dockPanelGuid = new Guid("ABF5C50F-A592-43DB-9DC4-8017CCBE3E0D");
        UIControlledApplication app = null;

        public void DockPanelRegister(UIControlledApplication application)
        {
            MainPage dock = new MainPage();
            dock.ExEvent = exEvent;
            dock.DefineExecute = defineOuterExecute;
            //MainWindow dock = new MainWindow(); - black square in dockablepane
            //WindowInteropHelper interopHelper = new WindowInteropHelper(dock);
            dockPanelView = dock;
            DockablePaneId dockId = new DockablePaneId(dockPanelGuid);

            //DockablePane dockPane = application.GetDockablePane(dockId);

            try
            {
                //register dockable panel                
                application.RegisterDockablePane
                    (
                        dockId,
                        "Atom Revit Panel",
                        dockPanelView
                    );

            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("Info Message", ex.Message);
            }
        }
    }   
}
