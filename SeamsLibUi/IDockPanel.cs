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
    public interface IDockPanel
    {
        IDockablePaneProvider GetDockProvider();
        IDockAddinControl GetAddinControl();
        IDockViewModel GetDockViewModel();
        void RemoveAddinControl();
        void AddAddinControl();
        void SetupDockView(IDockViewModel viewModel);
        void ResetDockView();
    }
}
