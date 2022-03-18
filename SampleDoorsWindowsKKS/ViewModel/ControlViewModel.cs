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

namespace SampleDoorsWindowsKKS
{
    public partial class ControlViewModel : ViewModelBase
    {
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
                doors = (List<Element>)(doorCollector.WherePasses(doorCategoryFilter).WhereElementIsElementType().ToElements());                
            }
            catch (Exception exc)
            {
                TaskDialog.Show("ERROR", exc.Message);
            }

            foreach(var door in doors)
            {                
                DoorPresenter doorPresenter = new DoorPresenter() { Id = door.Id.IntegerValue.ToString(), Name = door.Name, };
                DoorsList.Add(doorPresenter);
            }
        }


        private ObservableCollection<DoorPresenter> doorsList;
        public ObservableCollection<DoorPresenter> DoorsList
        {
            get 
            {
                if (doorsList == null) doorsList = new ObservableCollection<DoorPresenter>();
                return doorsList; }
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
