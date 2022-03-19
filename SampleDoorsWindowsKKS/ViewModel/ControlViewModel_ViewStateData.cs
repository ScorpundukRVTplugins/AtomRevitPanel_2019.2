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

namespace SampleDoorsWindowsKKS
{
    public partial class ControlViewModel
    {
        private ObservableCollection<DoorPresenter> doorsList;
        public ObservableCollection<DoorPresenter> DoorsList
        {
            get
            {
                if (doorsList == null) doorsList = new ObservableCollection<DoorPresenter>();
                return doorsList;
            }
            set
            {
                doorsList = value;
                OnPropertyChanged(nameof(DoorsList));
            }
        }

        public class DoorPresenter
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}
