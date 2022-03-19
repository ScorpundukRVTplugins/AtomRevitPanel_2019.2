using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.UI;

namespace DockApplicationBase
{
    public static partial class ExecuteProvider
    {
        /* Вызовы событий обновления видов 
         * (при условии подписки на них)
         * 
         */
        public static void InvokeDockPageUpdate()
        {
            if (updateDockPage != null)
                updateDockPage.Invoke();
        }
        public static void InvokeDockViewModelUpdate()
        {
            if (updateDockViewModel != null)
                updateDockViewModel.Invoke();
        }
        public static void InvokeAddinControlUpdate()
        {
            if (updateAddinControl != null)
                updateAddinControl.Invoke();
        }
        public static void InvokeAddinViewModelUpdate()
        {
            if (updateAddinViewModel != null)
                updateAddinViewModel.Invoke();
        }

        /* статическое событие для обновления страницы 
         * DockablePane из CodeBehind
         */
        private static event Action updateDockPage;
        public static event Action UpdateDockPage
        {
            add
            {
                if (updateDockPage == null)
                    updateDockPage += value;
            }
            remove
            {
                if (updateDockPage != null)
                    updateDockPage -= value;
            }
        }

        /* статическое событие для обновления страницы
         * DockablePane через ViewModel
         */
        private static event Action updateDockViewModel;
        public static event Action UpdateDockViewModel
        {
            add
            {
                if (updateDockViewModel == null)
                    updateDockViewModel += value;
            }
            remove
            {
                if (updateDockViewModel != null)
                    updateDockViewModel -= value;
            }
        }

        /* статическое событие для обновления
         * контрола плагина из его CodeBehind
         */
        private static event Action updateAddinControl;
        public static event Action UpdateAddinControl
        {
            add
            {
                if (updateAddinControl == null)
                    updateAddinControl += value;
            }
            remove
            {
                if (updateAddinControl != null)
                    updateAddinControl -= value;
            }
        }

        /* статическое событие для обновления
         * контрола плагина из его ViewModel
         */
        private static event Action updateAddinViewModel;
        public static event Action UpdateAddinViewModel
        {
            add
            {
                if (updateAddinViewModel == null)
                    updateAddinViewModel += value;
            }
            remove
            {
                if (updateAddinViewModel != null)
                    updateAddinViewModel -= value;
            }
        }
    }
}
