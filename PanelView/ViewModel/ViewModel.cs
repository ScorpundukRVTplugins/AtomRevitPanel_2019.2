using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeamsLibUi;
using static SeamsLibUi.ExecuteProvider;

using MVVM;
using Autodesk.Revit.UI;

namespace PanelView
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel() : base()
        {
            UpdateDockViewModel += ExecuteUpdate;
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
