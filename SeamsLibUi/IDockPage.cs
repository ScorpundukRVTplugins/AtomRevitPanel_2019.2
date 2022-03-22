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
    public interface IDockPage
    {
        IDockablePaneProvider GetDockPageProvider();
        IDockControl GetDockControl();
        IDockViewModel GetDockPageViewModel();
        void RemoveDockControl();
        void AddDockControl();
        void SetupDockPage(IDockViewModel viewModel);
        void ResetDockPage();
    }
}
