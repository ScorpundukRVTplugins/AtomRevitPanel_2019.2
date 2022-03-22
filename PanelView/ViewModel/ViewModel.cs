using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DockApplicationBase;
using static DockApplicationBase.ExecuteProvider;

using MVVM;
using Autodesk.Revit.UI;

namespace PanelView
{
    public class MainPageViewModel : ViewModelBase, IDockElementUpdater, IDockViewModel
    {
        public MainPageViewModel() : base()
        {

        }

        #region IDockPanelUpdater implementation

        public void ExecuteUpdate()
        {
            DefineExternalExecute(UpdateState);
            if (!ExternalExecuteCaller.IsPending)
                ExternalExecuteCaller.Raise();
        }

        public void UpdateState(UIApplication uiapplication)
        {
            
        }

        #endregion

        #region IDockViewModel implementation


        #endregion
    }
}
