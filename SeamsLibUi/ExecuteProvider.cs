using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.UI;

namespace SeamsLibUi
{
    public static class ExecuteProvider
    {
        /* делегат для передачи извне контекста Revit
         * методa для исполнения в экземпляре ExternalEventProvider
         * упакованного в экземпляр ExternalEvent (ExEvent)
         */
        private static Action<Action<UIApplication>> defineExternalExecute;
        public static Action<Action<UIApplication>> DefineExternalExecute
        {
            get { return defineExternalExecute; }
            set
            {
                if (isDefinerDefined == false)
                {
                    defineExternalExecute = value;
                    isDefinerDefined = true;
                }
                else
                {
                    MessageBox.Show("DefineExternalExecute already defined");
                }
            }
        }
        private static bool isDefinerDefined = false;


        /* передаётся во внешний контекст
         * во внешнем контексте вызывается метод Raise
         * запускающий метод Execute экземпляра типа ExternalEventProvider
         *
         * передача в главную страницу DockablePane
         * а из неё в подключённый UserControl
         */
        private static ExternalEvent externalExecuteCaller;
        public static ExternalEvent ExternalExecuteCaller
        {
            get { return externalExecuteCaller; }
            set
            {
                if(isCallerDefined == false)
                {
                    externalExecuteCaller = value;
                    isCallerDefined = true;
                }
                else
                {
                    MessageBox.Show($"{nameof(ExternalExecuteCaller)} already defined");
                }
            }
        }
        private static bool isCallerDefined = false;
    }
}
