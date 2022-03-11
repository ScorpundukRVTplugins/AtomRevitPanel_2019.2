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
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI.Events;

using PanelView;

namespace AtomRevitPanel
{
    /* команда регистрации DockablePane в приложении Revit
     * думаю, что следует перенести логику метода 
     * Execute в метод OnStartUp приложения
     */
    [Transaction(TransactionMode.Manual)]
    public class RegisterAtomPanel : IExternalCommand
    {
        private MainPage dockPanelView = null;
        private ExternalCommandData commandData = null;
        private ElementSet elements = null;


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MainPage dock = new MainPage();
            dockPanelView = dock;
            this.commandData = commandData;

            DockablePaneId dockId = new DockablePaneId(new Guid("ABF5C50F-A592-43DB-9DC4-8017CCBE3E0D"));

            try
            {
                //register dockable panel
                commandData.Application.RegisterDockablePane
                    (
                        dockId,
                        "Atom Revit Panel",
                        dockPanelView as IDockablePaneProvider
                    );
                TaskDialog.Show("Info message", "Dockable window was registered!");
                // subscribe document opened event
                commandData.Application.Application.DocumentOpened += 
                    new EventHandler<DocumentOpenedEventArgs>(Application_DocumentOpened);
                // subscribe view activated event
                commandData.Application.ViewActivated += 
                    new EventHandler<ViewActivatedEventArgs>(Application_ViewActivated);
            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("Info Message", ex.Message);
            }

            return Result.Succeeded;
        }

        // view activated event
        public void Application_ViewActivated(object sender, ViewActivatedEventArgs e)
        {
            // provide ExternalCommandData object to dockable page
            dockPanelView.InitiateRevitAccess(commandData, elements);

        }
        // document opened event
        private void Application_DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            // provide ExternalCommandData object to dockable page
            dockPanelView.InitiateRevitAccess(commandData, elements);
        }
    }
}
