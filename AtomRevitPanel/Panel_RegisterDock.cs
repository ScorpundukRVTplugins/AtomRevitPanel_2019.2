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

using SeamsLibUi;


namespace AtomRevitPanel
{
    public partial class AtomRevitPanel : IExternalApplication
    {
        public void DockPanelRegister(UIControlledApplication application)
        {
            IMainPanelAccess<UIApplication, ExternalEvent> dock = null;

            try
            {
                dock = FindPanelViewAssembly() as IMainPanelAccess<UIApplication,ExternalEvent>;
            }
            catch(Exception exc)
            {
                TaskDialog.Show("Main panel view assembly search error", exc.Message);
            }

            dockPanelView = 

            dock.ExternalExecuteCaller = ExternalExecuteCaller;
            dock.DefineExternalExecute = DefineExternalExecute;

            DockablePaneId dockId = new DockablePaneId(dockPanelGuid);

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

        public Page FindPanelViewAssembly()
        {
            string addinFolder = Assembly.GetExecutingAssembly().Location;

            DirectoryInfo addinFolderInfo = new DirectoryInfo(addinFolder);
            FileInfo[] files = addinFolderInfo.GetFiles("*.dll");
            foreach (FileInfo file in files)
            {
                Assembly potentialMainPanelView = Assembly.Load(file.FullName);
                Type mainPanelViewType = (from t in potentialMainPanelView.DefinedTypes
                                          where t.ImplementedInterfaces.Contains(typeof(IMainPanelAccess<UIApplication, ExternalEvent>))
                                          select t).First();
                if(mainPanelViewType != null && mainPanelViewType.BaseType == typeof(Page))
                {
                    return Activator.CreateInstance(mainPanelViewType) as Page;
                }
            }
            return null;
        }
    }   
}
