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
using Application = Autodesk.Revit.ApplicationServices.Application;

using System.Windows;
using System.Windows.Controls;

using static DockApplicationBase.ExecuteProvider;

namespace AtomRevitPanel
{
    public partial class AtomRevitPanel : IExternalApplication
    {
        public void ControlledApplication_DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            Application app = (Application)sender;
            //TaskDialog.Show("Controlled application Document opened", count.ToString());
            if (app.Documents.Size == 1)
            {
                /* Через DefineExternalExecute передаётся метод,
                 * который назначается делегату RunExternalExecute
                 * Если в передаваемый метод вложен один или
                 * несколько вызовов DefineExternalExecute
                 * то эти вызовы будут произведены только после
                 * вызова на исполнение делегата RunExternalExecute
                 * и передаваемые методы не смогут на ходу назначится
                 * уже исполняемому делегату и не смогут быть
                 * выполнены
                 */
                DefineExternalExecute(ShowPanel);
                InvokeDockPageUpdate();
                InvokeDockViewModelUpdate();
            }
        }

        /* view activated event
         * срабатывает когда открывается или переключается новый вид
         * срабатывает раньше, чем происходит событие DocumentOpened
         */
        public static void Application_ViewActivated(object sender, ViewActivatedEventArgs e)
        {
            InvokeDockPageUpdate();
            InvokeDockViewModelUpdate();
            if (dockView.GetDockControl() != null)
            {
                InvokeAddinControlUpdate();
                InvokeAddinViewModelUpdate();
            }
        }

        /* document opened event
         * зачем это надо если есть ViewActivated, и он отрабатывает раньше? 
         * 
         */
        public static void Application_DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {

        }

        public static void DocumentChangedHandler(object sender, DocumentChangedEventArgs e)
        {
            InvokeAddinControlUpdate();
            InvokeAddinViewModelUpdate();
            if (dockView.GetDockControl() != null)
            {
                InvokeAddinControlUpdate();
                InvokeAddinViewModelUpdate();
            }
        }
    }
}
