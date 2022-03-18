using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;

using SeamsLibUi;
using static SeamsLibUi.ExecuteProvider;
using MVVM;
using System.Collections.ObjectModel;

namespace SampleMovingControl
{
    public class ControlViewModel : ViewModelBase
    {
        public ControlViewModel() : base()
        {
            UpdateAddinViewModel += ExecuteUpdate;
        }

        public void ExecuteUpdate()
        {
            DefineExternalExecute(UpdateViewModel);
            ExternalExecuteCaller.Raise();
        }

        public void UpdateViewModel(UIApplication uiapp)
        {

        }


        private ObservableCollection<ElementPresenter> elementsToMove;
        public ObservableCollection<ElementPresenter> ElementsToMove
        {
            get { return elementsToMove; }
            set
            {
                elementsToMove = value;
                OnPropertyChanged(nameof(ElementsToMove));
            }
        }

        public class ElementPresenter
        {
            private int elemId;
            public int ElemId
            {
                get;
                set;
            }

            private string elemName;
            public string ElemName
            {
                get;
                set;
            }

            private double xPos;
            public double XPos
            {
                get;
                set;
            }

            private double yPos;
            public double YPos
            {
                get;
                set;
            }

            private double zPos;
            public double ZPos
            {
                get;
                set;
            }
        }
    }
}
