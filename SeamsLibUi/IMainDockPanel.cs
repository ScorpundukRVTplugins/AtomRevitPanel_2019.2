using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

using System.Windows.Controls;

namespace DockApplicationBase
{
    public interface IMainDockPanel
    {
        IDockablePaneProvider GetDockProvider();
        IDockPanelWpfView GetDockPage();
        IDockPanelWpfView GetAddinControl();
        void RemoveAddinControl();
        void AddAddinControl();
    }
}
