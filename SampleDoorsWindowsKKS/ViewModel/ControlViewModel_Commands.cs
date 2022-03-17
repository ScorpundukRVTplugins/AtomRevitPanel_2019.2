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

using SeamsLibUi;
using MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SampleDoorsWindowsKKS
{
    public partial class ControlViewModel : ViewModelBase, IPanelControlViewModel
    {
        RelayCommand collectDoorsCmd;
        public ICommand CollectDoorsCmd
        {
            get
            {
                if (collectDoorsCmd == null)
                {
                    collectDoorsCmd =
                        new RelayCommand(DeleteVSSetsExecute, DeleteVSSetsCanExecute);
                }
                return collectDoorsCmd;
            }
        }

        public void DeleteVSSetsExecute(object parameter)
        {
            bool full = doorsList == null;
            DefineExternalExecute(CollectDoors);
            ExternalExecuteCaller.Raise();
            TaskDialog.Show("From Execute", doorsList.Count.ToString());
        }

        public bool DeleteVSSetsCanExecute(object parameter)
        {
            return true;
        }

    }
}
