using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;

using SeamsLibUi;
using static SeamsLibUi.ExecuteProvider;
using MVVM;

namespace SampleParameterChangingControl
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
    }
}
