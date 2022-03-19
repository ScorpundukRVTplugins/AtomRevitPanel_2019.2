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
        public static IMainDockPanel dockAccess;
        public static bool firstOpenDone = false;

        public static Guid dockPanelGuid = new Guid("ABF5C50F-A592-43DB-9DC4-8017CCBE3E0D");

       /* статическое свойство, содержащее метод
        * для исполнения, переданный извне контекста Revit
        * передаётся через делегат DefineOuterExecute
        * переданный во внешний контекст
        */
        public static Action<UIApplication> RunExternalExecute
        {
            get;
            set;
        }

        /* Раздел вкладки приложения
         * 
         */
        private RibbonPanel ribbonPanel;
        public RibbonPanel RibbonPanel
        {
            get { return ribbonPanel; }
            private set { ribbonPanel = value; }
        }

        private string assemblyPath;
        public string AssemblyPath
        {
            get { return assemblyPath; }
            private set { assemblyPath = value; }
        }

        private Assembly addinEntryAssembly;
        public Assembly AddinEntryAssembly
        {
            get { return addinEntryAssembly; }
            private set { addinEntryAssembly = value; }
        }

        /* поле с "запасом" - возможно, в будущем
         * я буду применять более "чистую" архитектуру
         * тогда понадобится переменная для сборки
         * с интерфейсом приложения
         */
        private Assembly windowPanelAssembly;
        public Assembly WindowPanelAssembly
        {
            get { return windowPanelAssembly; }
            private set { windowPanelAssembly = value; }
        }

        public string NameSpace
        {
            get { return GetType().Namespace; }            
        }

    }
}
