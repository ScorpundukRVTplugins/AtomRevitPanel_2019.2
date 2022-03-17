using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeamsLibUi;
using MVVM;
using Autodesk.Revit.UI;

namespace PanelView
{
    public class MainPageViewModel : ViewModelBase, IPanelControlViewModel
    {
        public void SefExternalExecuteCaller(ExternalEvent exEvent)
        {
            throw new NotImplementedException();
        }

        public void SetDefineExternalExecute(Action<Action<UIApplication>> defineExecute)
        {
            throw new NotImplementedException();
        }
    }
}
