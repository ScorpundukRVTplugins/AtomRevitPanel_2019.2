using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.ComponentModel;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;

using DockApplicationBase;
using MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;

using static DockApplicationBase.ExecuteProvider;

namespace SampleDoorsWindowsKKS
{
    public partial class ControlViewModel : ViewModelBase, IDockViewModel, IDockElementUpdater
    {
        Line ln;
    }
}
