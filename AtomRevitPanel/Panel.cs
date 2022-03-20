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

using static DockApplicationBase.ExecuteProvider;
using Autodesk.Revit.ApplicationServices;
using System.Windows;
using Application = Autodesk.Revit.ApplicationServices.Application;

namespace AtomRevitPanel
{
    public partial class AtomRevitPanel : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {            
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            AddNewRibbon(application);
            
            /* Only if you want to register dockablepane manualy
             * while no opened document in application
            AddButtonOnRibbon(
                "Register Window",
                "Register",
                "Register dockable window at the zero document state.",
                typeof(RegisterAtomPanel),
                typeof(CommandAvailability),
                "register32.png",
                "register16.png");
            */

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

            DefineExternalExecute += DefineExecute;

            // определение статического ExternalEvent
            try
            {
                ExternalExecuteCaller = ExternalEvent.Create(new ExternalEventProvider());

            }
            catch(Exception exc)
            {
                TaskDialog.Show("External Event definition failed", exc.Message);
            }

            if (!DockPanelRegister(application)) return Result.Failed;

            application.ControlledApplication.DocumentOpened += ControlledApplication_DocumentOpened;

            return Result.Succeeded;
        }


        /* методя для передачи с делегатом DefineExternalExecute
         * будет вызваться вне контекста приложения
         * где-то на главной странице DockablePane
         * или в контроле дополнения
         * 
         * через него в контекст Revit передаётся метод извне контекста
         */
        public void DefineExecute(Action<UIApplication> exec)
        {
            RunExternalExecute += exec;
        }

        /* переданный метод обнуляется 
         */
        public static void DeleteDefineExecute()
        {
            RunExternalExecute = null;
        }
    }
     
    /* external command availability
     * класс, обеспечивающий проверку возможности
     * запуска команды регистрации DockablePane 
     * дело в том, что по какой-то причине 
     * регистрация DockablePane может быть 
     * произведена только если 
     * нет откртого активного документа
     * 
     * Tammik: You can only register a dockable dialog in "Zero doc state"
     * 
     * я перенёс регистрацию DockablePane
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

    /* специальный класс для вызова методов за пределами контекста
     * выполнения Revit, что особенно характерно для плагинов с 
     * с интерфейсом на основе DockablePane
     * его метод Execute вызывается через экземпляр ExternalEvent
     */
    public class ExternalEventProvider : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            if(AtomRevitPanel.RunExternalExecute != null)
            {
                if (false) // для проверки если будет надо
                {
                    StringBuilder sb = new StringBuilder();
                    foreach(var item in AtomRevitPanel.RunExternalExecute.GetInvocationList())
                    {
                        sb.Append($"{item.GetMethodInfo().DeclaringType}.{item.GetMethodInfo().Name}\n");
                    }
                    MessageBox.Show(sb.ToString());
                }

                // запуск переданного извне контекста Revit метода
                AtomRevitPanel.RunExternalExecute.Invoke(app);
            }
            // обнуление
            AtomRevitPanel.RunExternalExecute = null;
        }

        public string GetName()
        {
            return nameof(ExternalEventProvider);
        }
    }    
}
