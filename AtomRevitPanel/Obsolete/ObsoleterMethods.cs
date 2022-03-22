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

using System.Windows;
using System.Windows.Controls;

using DockApplicationBase;

namespace AtomRevitPanel
{
    public partial class AtomRevitPanel : IExternalApplication
    {
        /* поиск сборки, содержащей страницу WPF (WPF Page)
         * с интерфейсом IMainDockPanel
         * Тип страницы с интерфейсом IDockablePaneProvider 
         * может содержаться и в других сборках
         * но не будет подходить к данному приложению
         */
        public IDockPage FindPanelViewAssembly()
        {
            string addinAssembly = Assembly.GetExecutingAssembly().Location;
            string addinFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            DirectoryInfo addinFolderInfo = new DirectoryInfo(addinFolder);
            FileInfo[] files = addinFolderInfo.GetFiles("*.dll");
            foreach (FileInfo file in files)
            {
                Type mainPanelViewType = null;
                Assembly dockPanelAssembly = Assembly.Load(file.FullName);
                string name = dockPanelAssembly.GetName().Name;

                try
                {
                    mainPanelViewType = (from t in dockPanelAssembly.DefinedTypes
                                         where t.GetInterfaces().Contains(typeof(IDockPage))
                                         select t).First();
                }
                catch { }

                if (mainPanelViewType != null && mainPanelViewType.BaseType == typeof(Page))
                {
                    return Activator.CreateInstance(mainPanelViewType) as IDockPage;
                }
            }
            return null;
        }
    }
}
