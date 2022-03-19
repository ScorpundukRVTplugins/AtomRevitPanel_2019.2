using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;

using DockApplicationBase;
using static DockApplicationBase.ExecuteProvider;

using MVVM;

namespace SampleDockableControl
{
    public class ControlViewModel : ViewModelBase, IUpdateSubscriber
    {
        public ControlViewModel() : base()
        {
            UpdateAddinViewModel += ExecuteUpdate;
        }

        public void ExecuteUpdate()
        {
            DefineExternalExecute(UpdateState);
            ExternalExecuteCaller.Raise();
        }


        public void UpdateState(UIApplication uiapp)
        {

        }
    
        public void UnhookAllBinds()
        {
            UpdateAddinViewModel -= ExecuteUpdate;
        }
    }
}
