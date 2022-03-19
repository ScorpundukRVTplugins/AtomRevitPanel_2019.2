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
using static DockApplicationBase.ExecuteProvider;
using MVVM;
using System.Collections.ObjectModel;

namespace SampleDoorsWindowsKKS
{
    public partial class ControlViewModel : ViewModelBase, IViewUpdater
    {
        public ControlViewModel() : base()
        {

        }

        public void ExecuteUpdate()
        {
            DefineExternalExecute(UpdateState);
            ExternalExecuteCaller.Raise();
        }
        
        public void UpdateState(UIApplication uiapplication)
        {
            CollectDoors(uiapplication);
        }

        public void UnhookAllBinds()
        {
            
        }
    }
}
