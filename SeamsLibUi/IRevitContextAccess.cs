using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Autodesk.Revit.UI;

namespace SeamsLibUi
{
    public interface IRevitContextAccess
    {
        void UpdateView(UIApplication uiapplication);        
        object GetViewElement();
        Action<Action<UIApplication>> DefineExternalExecute { get; set; }        
        ExternalEvent ExternalExecuteCaller { get; set; }
        IPanelControlViewModel ViewModel { get; set; }
    }    
}