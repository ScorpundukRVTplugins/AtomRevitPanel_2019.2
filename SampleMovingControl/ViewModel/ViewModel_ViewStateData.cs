using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;

using DockApplicationBase;
using static DockApplicationBase.ExecuteProvider;
using MVVM;
using System.Collections.ObjectModel;

namespace SampleMovingControl
{
    public partial class ControlViewModel
    {
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
