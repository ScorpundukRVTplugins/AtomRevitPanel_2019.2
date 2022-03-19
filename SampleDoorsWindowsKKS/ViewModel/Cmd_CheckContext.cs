using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;

using DockApplicationBase;
using MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;

using static DockApplicationBase.ExecuteProvider;
using System.Windows;

namespace SampleDoorsWindowsKKS
{
    public partial class ControlViewModel
    {
        RelayCommand checkContextCmd;
        public ICommand CheckContextCmd
        {
            get
            {
                if (checkContextCmd == null)
                {
                    checkContextCmd =
                        new RelayCommand(CheckContextExecute, CheckContextCanExecute);
                }
                return checkContextCmd;
            }
        }

        public void CheckContextExecute(object parameter)
        {
            MessageBox.Show(CountOfDoors);
        }

        public bool CheckContextCanExecute(object parameter)
        {
            return true;
        }
    }
}
