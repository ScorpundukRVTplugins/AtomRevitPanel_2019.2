using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Autodesk.Revit.UI;

namespace DockApplicationBase
{
    public interface IDockPanelWpfView
    {
        IViewUpdater GetViewUpdater();
        IViewUpdater GetViewModelUpdater();
        object GetViewElement();
    }
}