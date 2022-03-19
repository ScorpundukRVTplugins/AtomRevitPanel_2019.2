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
        RelayCommand collectDoorsCmd;
        public ICommand CollectDoorsCmd
        {
            get
            {
                if (collectDoorsCmd == null)
                {
                    collectDoorsCmd =
                        new RelayCommand(CollectDoorsExecute, CollectDoorsCanExecute);
                }
                return collectDoorsCmd;
            }
        }

        public void CollectDoorsExecute(object parameter)
        {
            try
            {
                DefineExternalExecute(CollectDoors);
                ExternalExecuteCaller.Raise();
            }
            catch (Exception exc)
            {
                TaskDialog.Show("Static members call failed", exc.Message);
            }
        }

        public bool CollectDoorsCanExecute(object parameter)
        {
            return true;
        }

        public void CollectDoors(UIApplication uiapp)
        {
            Application app = uiapp.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Element> doors = new List<Element>();
            try
            {
                ElementCategoryFilter doorCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
                FilteredElementCollector doorCollector = new FilteredElementCollector(doc);
                doors = (List<Element>)doorCollector.WherePasses(doorCategoryFilter).WhereElementIsNotElementType().ToElements();
            }
            catch (Exception exc)
            {
                TaskDialog.Show("ERROR", exc.Message);
            }

            DoorsList.Clear();

            foreach (var door in doors)
            {
                DoorPresenter doorPresenter = new DoorPresenter() { Id = door.Id.IntegerValue.ToString(), Name = door.Name, };
                DoorsList.Add(doorPresenter);
            }

            CountOfDoors = DoorsList.Count.ToString();
        }

        public string countOfDoors;
        public string CountOfDoors
        {
            get
            {
                if (countOfDoors == null) countOfDoors = string.Empty;
                return countOfDoors;
            }
            set
            {
                countOfDoors = value;
                OnPropertyChanged(nameof(CountOfDoors));
            }
        }

    }
}
