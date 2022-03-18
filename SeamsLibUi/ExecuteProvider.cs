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
                //else
                //{
                //    MessageBox.Show("DefineExternalExecute already defined");
                //}
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
                //else
                //{
                //    MessageBox.Show($"{nameof(ExternalExecuteCaller)} already defined");
                //}
            }
        }
        private static bool isCallerDefined = false;


        private static event Action updateDockPage;
        public static event Action UpdateDockPage
        {
            add
            {
                if(isDockPageUpdaterDefined == false)
                {
                    updateDockPage += value;
                    isDockPageUpdaterDefined = true;
                }
                //else
                //{
                //    MessageBox.Show($"{nameof(UpdateDockPage)} already defined");
                //}
            }
            remove
            {
                if(updateDockPage != null)
                {
                    updateDockPage -= value;
                    isDockPageUpdaterDefined = false;
                }
            }
        }
        private static bool isDockPageUpdaterDefined = false;

        public static void InvokeDockPageUpdate()
        {
            if(updateDockPage != null)
            updateDockPage.Invoke();
        }


        private static event Action updateDockViewModel;
        public static event Action UpdateDockViewModel
        {
            add
            {
                if (isDockViewModelUpdaterDefined == false)
                {
                    updateDockViewModel += value;
                    isDockViewModelUpdaterDefined = true;
                }
                //else
                //{
                //    MessageBox.Show($"{nameof(UpdateDockViewModel)} already defined");
                //}
            }
            remove
            {
                if (updateDockViewModel != null)
                {
                    updateDockViewModel -= value;
                    isDockViewModelUpdaterDefined = false;
                }
            }
        }
        private static bool isDockViewModelUpdaterDefined = false;

        public static void InvokeDockViewModelUpdate()
        {
            if (updateDockViewModel != null)
                updateDockViewModel.Invoke();
        }



        private static event Action updateAddinControl;
        public static event Action UpdateAddinControl
        {
            add
            {
                if (isAddinControlUpdaterDefined == false)
                {
                    updateAddinControl += value;
                    isAddinControlUpdaterDefined = true;
                }
                //else
                //{
                //    MessageBox.Show($"{nameof(UpdateAddinControl)} already defined");
                //}
            }
            remove
            {
                if (updateAddinControl != null)
                {
                    updateAddinControl -= value;
                    isAddinControlUpdaterDefined = false;
                }
            }
        }
        private static bool isAddinControlUpdaterDefined = false;

        public static void InvokeAddinControlUpdate()
        {
            if (updateAddinControl != null)
                updateAddinControl.Invoke();
        }



        private static event Action updateAddinViewModel;
        public static event Action UpdateAddinViewModel
        {
            add
            {
                if (addinViewModelUpdaterDefined == false)
                {
                    updateAddinViewModel += value;
                    addinViewModelUpdaterDefined = true;
                }
                //else
                //{
                //    MessageBox.Show($"{nameof(UpdateAddinViewModel)} already defined");
                //}
            }
            remove
            {
                if (updateAddinViewModel != null)
                {
                    updateAddinViewModel -= value;
                    addinViewModelUpdaterDefined = false;
                }
            }
        }
        private static bool addinViewModelUpdaterDefined = false;

        public static void InvokeAddinViewModelUpdate()
        {
            if(updateAddinViewModel != null)
            updateAddinViewModel.Invoke();
        }
    }
}
