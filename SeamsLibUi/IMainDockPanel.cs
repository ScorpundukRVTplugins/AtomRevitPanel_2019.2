using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

using System.Windows.Controls;

namespace SeamsLibUi
{
    public interface IMainDockPanel
    {
        IDockablePaneProvider GetDockProvider();
        IDockPanelWpfView GetRevitAccess();
        void ContextEventUpdate();
        void UpdateCurrentControl(UIApplication uiapplication);
        void RemoveAddinControl();
        void AddAddinControl();
    }
}
