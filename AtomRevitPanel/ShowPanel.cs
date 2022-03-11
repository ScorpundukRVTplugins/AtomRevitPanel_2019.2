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
    public class ShowAtomPanel : IExternalCommand
    {
        private ExternalCommandData commandData = null;
        private ElementSet elements = null;
        public MainPage dockPanelView = AtomRevitPanel.dockPanelView;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            this.commandData = commandData;

            try
            {
                // dockable window id
                DockablePaneId id = new DockablePaneId(AtomRevitPanel.dockPanelGuid);
                DockablePane dockableWindow = commandData.Application.GetDockablePane(id);
                dockableWindow.Show();
                dockPanelView.InitiateRevitAccess(commandData, elements);
            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("Info Message", ex.Message);
            }

            commandData.Application.Application.DocumentOpened +=
                Application_DocumentOpened;

            // subscribe view activated event
            commandData.Application.ViewActivated +=
                Application_ViewActivated;

            commandData.Application.ApplicationClosing += 
                Application_ApplicationClosing;

            // return result
            return Result.Succeeded;
        }

        private void Application_ApplicationClosing(object sender, ApplicationClosingEventArgs e)
        {
            //commandData.Application.Application.DocumentOpened -= Application_DocumentOpened;
            //commandData.Application.ViewActivated -= Application_ViewActivated;
            //AtomRevitPanel.dockPanelView = null;
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
            //UIApplication uiapp = sender as UIApplication;
            //DockablePaneId id = new DockablePaneId(AtomRevitPanel.dockPanelGuid);
            //DockablePane dockablePane = uiapp.GetDockablePane(id);
            //dockablePane.Hide();

            // provide ExternalCommandData object to dockable page
            dockPanelView.InitiateRevitAccess(commandData, elements);
        }
    }
}
