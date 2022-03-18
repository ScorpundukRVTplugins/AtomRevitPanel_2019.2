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
using SeamsLibUi;

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

    [Transaction(TransactionMode.Manual)]
    public class ShowAtomPanel : IExternalCommand
    {
        private ExternalCommandData commandData = null;
        public Page dockPanelView = (AtomRevitPanel.dockAccess as IDockPanelWpfView).GetViewElement() as Page;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            this.commandData = commandData;

            try
            {
                // dockable window id
                DockablePaneId id = new DockablePaneId(AtomRevitPanel.dockPanelGuid);
                DockablePane dockableWindow = commandData.Application.GetDockablePane(id);
                (AtomRevitPanel.dockAccess as IDockPanelWpfView).UpdateView(commandData.Application);

                dockableWindow.Show();
            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("DockablePane showing error", ex.Message);
            }

            commandData.Application.Application.DocumentOpened +=
                Application_DocumentOpened;

            // subscribe view activated event
            commandData.Application.ViewActivated +=
                Application_ViewActivated;

            // return result
            return Result.Succeeded;
        }

        /* view activated event
         * срабатывает когда открывается или переключается новый вид
         */
        public void Application_ViewActivated(object sender, ViewActivatedEventArgs e)
        {
            // provide ExternalCommandData object to dockable page
            //dockPanelView.InitiateRevitAccess(commandData, elements);
            (AtomRevitPanel.dockAccess as IDockPanelWpfView).UpdateView(commandData.Application);

        }

        /* document opened event
         * срабатывает когда открывается новый документ - в этом случае также срабатывает 
         * и ViewActivated
         */
        private void Application_DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            // provide ExternalCommandData object to dockable page
            (AtomRevitPanel.dockAccess as IDockPanelWpfView).UpdateView(commandData.Application);
        }
    }
}