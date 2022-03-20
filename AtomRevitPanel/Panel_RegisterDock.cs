using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Interop;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI.Events;

using DockApplicationBase;
using static DockApplicationBase.ExecuteProvider;


namespace AtomRevitPanel
{
    public partial class AtomRevitPanel : IExternalApplication
    {
        public bool DockPanelRegister(UIControlledApplication application)
        {
            if (!FindDockPanelAssembly())
            {
                return false;
            }

            try
            {
                dockView = Activator.CreateInstance(DockPanelViewType) as IDockPanel;
                dockViewModel = Activator.CreateInstance(DockPanelViewModelType) as IDockViewModel;
                dockView.SetupDockView(dockViewModel);
            }
            catch(Exception exc)
            {
                TaskDialog.Show("Main panel view assembly search error", exc.Message);
                return false;
            }

            DockablePaneId dockId = new DockablePaneId(dockPanelGuid);
            try
            {
                //register dockable panel                
                application.RegisterDockablePane
                    (
                        dockId,
                        "Atom Revit Panel",
                        dockView.GetDockProvider()
                    );
            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("Info Message", ex.Message);
                return false;
            }
            return true;
        }

        public bool FindDockPanelAssembly()
        {
            string addinAssembly = Assembly.GetExecutingAssembly().Location;
            string addinFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            DirectoryInfo addinFolderInfo = new DirectoryInfo(addinFolder);
            FileInfo[] files = addinFolderInfo.GetFiles("*.dll");

            Type dockPanelViewType = null;
            Type dockPanelViewModelType = null;
            Assembly dockPanelAssembly = null;
            bool typesFound = false;

            foreach (FileInfo file in files)
            {
                dockPanelAssembly = Assembly.Load(file.FullName);

                try
                {
                    dockPanelViewType = (from t in dockPanelAssembly.DefinedTypes
                                         where t.GetInterfaces().Contains(typeof(IDockPanel))
                                         select t).First();
                    DockPanelViewType = dockPanelViewType;
                }
                catch { }

                try
                {
                    dockPanelViewModelType = (from t in dockPanelAssembly.DefinedTypes
                                         where t.GetInterfaces().Contains(typeof(IDockViewModel))
                                         select t).First();
                    DockPanelViewModelType = dockPanelViewModelType;
                    typesFound = true;
                }
                catch { }

                if (typesFound)
                {
                    DockPanelAssembly = dockPanelAssembly;
                    return true;
                }
            }
            return false;
        }

        public static void ShowPanel(UIApplication uiapp)
        {
            try
            {
                // dockable window id
                DockablePaneId id = new DockablePaneId(AtomRevitPanel.dockPanelGuid);
                DockablePane dockableWindow = uiapp.GetDockablePane(id);
                dockableWindow.Show();
            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("DockablePane showing error", ex.Message);
            }

            uiapp.Application.DocumentOpened +=
                Application_DocumentOpened;

            // subscribe view activated event
            uiapp.ViewActivated +=
                Application_ViewActivated;

            uiapp.Application.DocumentChanged +=
                DocumentChangedHandler;
        }
    }
}
