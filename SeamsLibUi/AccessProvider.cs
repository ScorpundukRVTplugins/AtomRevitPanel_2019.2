using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SeamsLibUi
{
    public interface IRevitAccessProvider<T,ExEv>
    {
        void SetRevitAccess(object uiapp, object uidoc, object app, object doc, object elementSet);
        void SetRevitAccess(object commandData, object elementSet);
        UserControl GetControl();

        Action<T> DefineExecute
        {
            get;
            set;
        }

        ExEv ExEvent
        {
            get;
            set;
        }
    }
}
