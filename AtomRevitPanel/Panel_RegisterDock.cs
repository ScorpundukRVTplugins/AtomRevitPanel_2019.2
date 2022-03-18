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
        public bool DockPanelRegister(UIControlledApplication application)
        {
            try
            {
                dockAccess = FindPanelViewAssembly() as IMainDockPanel;                
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
                        dockAccess.GetDockProvider()
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

        /* поиск сборки, содержащей страницу WPF (WPF Page)
         * с интерфейсом IMainDockPanel
         * Тип страницы с интерфейсом IDockablePaneProvider 
         * может содержаться и в других сборках
         * но не будет подходить к данному приложению
         */
        public IMainDockPanel FindPanelViewAssembly()
        {
            string addinAssembly = Assembly.GetExecutingAssembly().Location;
            string addinFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            DirectoryInfo addinFolderInfo = new DirectoryInfo(addinFolder);
            FileInfo[] files = addinFolderInfo.GetFiles("*.dll");
            int count = files.Length;
            foreach (FileInfo file in files)
            {
                Type mainPanelViewType = null;
                Assembly potentialMainPanelView = Assembly.Load(file.FullName);
                string name = potentialMainPanelView.GetName().Name;

                try
                {
                    mainPanelViewType = (from t in potentialMainPanelView.DefinedTypes
                                         where t.GetInterfaces().Contains(typeof(IMainDockPanel))
                                         select t).First();
                }
                catch { }

                if(mainPanelViewType != null && mainPanelViewType.BaseType == typeof(Page))
                {
                    return Activator.CreateInstance(mainPanelViewType) as IMainDockPanel;
                }
            }
            return null;
        }
    }   
}
