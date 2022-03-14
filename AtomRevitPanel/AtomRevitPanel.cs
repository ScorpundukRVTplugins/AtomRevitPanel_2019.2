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

namespace AtomRevitPanel
{
    public partial class AtomRevitPanel : IExternalApplication
    {
        public ExternalEvent exEvent = null;
        public Action<Action<UIApplication>> defineOuterExecute;
        public static Action<UIApplication> outerExecute;


        public Result OnShutdown(UIControlledApplication application)
        {            
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            AddNewRibbon(application);

            //AddButtonOnRibbon(
            //    "Register Window",
            //    "Register",
            //    "Register dockable window at the zero document state.",
            //    typeof(RegisterAtomPanel),
            //    typeof(CommandAvailability),
            //    "register32.png",
            //    "register16.png");

            
            AddButtonOnRibbon(
                "Show window",
                "Show",
                "Show the registered dockable window.",
                typeof(ShowAtomPanel),
                null,
                "show32.png",
                "show16.png");

            AddButtonOnRibbon(
                "Hide window",
                "Hide",
                "Hide dockable window.",
                typeof(HidePanel),
                null,
                "hide32.png",
                "hide16.png");

            defineOuterExecute += DefineExecute;

            try
            {
                exEvent = ExternalEvent.Create(new ExternalEventProvider());

            }
            catch(Exception exc)
            {
                TaskDialog.Show("External Event Creation error", exc.Message);
            }
            
            DockPanelRegister(application);
            //application.ApplicationClosing += Application_ApplicationClosing;

            //DockablePaneId id = new DockablePaneId(new Guid("ABF5C50F-A592-43DB-9DC4-8017CCBE3E0D"));
            //if(id != null)
            //{
            //    DockablePane dockablePane = application.GetDockablePane(id);
            //    dockablePane.Hide();
            //}

            //application.ViewActivated += Application_ViewActivated;

            //app.DockableFrameFocusChanged += App_DockableFrameFocusChanged;
            //app.ControlledApplication.ApplicationInitialized += ControlledApplication_ApplicationInitialized;


            return Result.Succeeded;
        }

        

        private void App_DockableFrameFocusChanged(object sender, DockableFrameFocusChangedEventArgs e)
        {

        }

        private void Application_ViewActivated(object sender, ViewActivatedEventArgs e)
        {
            UIControlledApplication uiconapp = sender as UIControlledApplication;
            DockPanelRegister(uiconapp);

            //DockablePaneId id = new DockablePaneId(new Guid("ABF5C50F-A592-43DB-9DC4-8017CCBE3E0D"));
            ////
            //if (!DockablePane.PaneIsRegistered(id))
            //{
            //}
            //else
            //{
            //    DockablePane dockablePane = uiconapp.GetDockablePane(id);
            //    dockablePane.Hide();
            //}
        }

        //private void Application_ApplicationClosing(object sender, ApplicationClosingEventArgs e)
        //{
        //    UIApplication uiapp = sender as UIApplication;

        //    DockablePaneId id = new DockablePaneId(new Guid("ABF5C50F-A592-43DB-9DC4-8017CCBE3E0D"));
        //    DockablePane dockablePane = uiapp.GetDockablePane(id);
        //    dockablePane.Hide();
        //}
        public void DefineExecute(Action<UIApplication> exec)
        {
            outerExecute += exec;
        }
    }

    // external command availability
    /* 
     * класс, обеспечивающий проверку возможности
     * запуска команды регистрации DockablePane 
     * дело в том, что по какой-то причине 
     * регистрация DockablePane может быть 
     * произведена только если 
     * нет откртого активного документа
     * 
     * Tammik: You can only register a dockable dialog in "Zero doc state"
     * 
     * думаю, что следует переносить регистрацию DockablePane
     * в метод OnStartUp приложения, чтобы пользователю
     * не надо было помнить, что окно должно создаваться 
    */
    public class CommandAvailability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            // zero doc state
            if (applicationData.ActiveUIDocument == null)
            {
                // disable register btn
                return true;
            }
            // enable register btn
            return false;
        }
    }

    public class ExternalEventProvider : IExternalEventHandler
    {
        //public Action<UIApplication> CommandProvider
        //{
        //    get;
        //    set;
        //}

        public void Execute(UIApplication app)
        {
            //TaskDialog.Show("ExternalEvent Info", "it works!");
            if(AtomRevitPanel.outerExecute != null)
            {
                AtomRevitPanel.outerExecute.Invoke(app);
            }
            AtomRevitPanel.outerExecute = null;
        }

        public string GetName()
        {
            return nameof(ExternalEventProvider);
        }
    }

    
}
