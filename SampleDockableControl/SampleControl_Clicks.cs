using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;
using System.Runtime.InteropServices;

using System.Threading;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

using DockApplicationBase;
using static DockApplicationBase.ExecuteProvider;

namespace SampleDockableControl
{
    /// <summary>
    /// Логика взаимодействия для UserControl2.xaml
    /// </summary>
    public partial class SampleControl : UserControl, IDockPanelWpfView, IUpdateSubscriber
    {
        private void getDocName_Click(object sender, RoutedEventArgs e)
        {
            DefineExternalExecute.Invoke(GetDocName);
            ExternalExecuteCaller.Raise();
        }


        private void selectElement_Click(object sender, RoutedEventArgs e)
        {
            DefineExternalExecute.Invoke(ExecSelectionSample);
            ExternalExecuteCaller.Raise();
        }

        private void changeDetailLevel_Click(object sender, RoutedEventArgs e)
        {
            DefineExternalExecute.Invoke(ExecTransactionSample);
            ExternalExecuteCaller.Raise();
        }
    }
}
