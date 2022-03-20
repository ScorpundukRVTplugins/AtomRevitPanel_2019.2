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
        public static IDockPanel dockView;
        public static IDockViewModel dockViewModel;
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
        
        // используется в определении Ribbon Tab
        private string applicationEntryAssemblyPath;
        public string ApplicationEntryAssemblyPath
        {
            get { return applicationEntryAssemblyPath; }
            private set { applicationEntryAssemblyPath = value; }
        }

        // используется в определении Ribbon Tab
        private Assembly applicationEntryAssembly;
        public Assembly ApplicationEntryAssembly
        {
            get { return applicationEntryAssembly; }
            private set { applicationEntryAssembly = value; }
        }

        // используется в поиске картинок и
        // регистрации кнопок на панели
        public string NameSpace
        {
            get { return GetType().Namespace; }
        }

        //
        private Assembly dockPanelAssembly;
        public Assembly DockPanelAssembly
        {
            get { return dockPanelAssembly; }
            private set { dockPanelAssembly = value; }
        }

        private Type dockPanelViewType;
        public Type DockPanelViewType
        {
            get { return dockPanelViewType; }
            set { dockPanelViewType = value; }
        }

        private Type dockPanelViewModelType;
        public Type DockPanelViewModelType
        {
            get { return dockPanelViewModelType; }
            set { dockPanelViewModelType = value; }
        }


    }
}
