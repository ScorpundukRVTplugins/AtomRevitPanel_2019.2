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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

using DockApplicationBase;
using static DockApplicationBase.ExecuteProvider;

namespace SampleMovingControl
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class MovingControl : UserControl, IDockPanelWpfView, IUpdateSubscriber
    {
        public MovingControl()
        {
            InitializeComponent();
            UpdateAddinControl += ExecuteUpdate;
            ViewModel = new ControlViewModel();
            DataContext = viewModel;
        }

        private ControlViewModel viewModel;
        public ControlViewModel ViewModel
        {
            get { return viewModel; }
            set { viewModel = value; }
        }

        public object GetViewElement()
        {
            return this as object;
        }

        public void ExecuteUpdate()
        {
            DefineExternalExecute(UpdateState);
            ExternalExecuteCaller.Raise();
        }

        public void UpdateState(UIApplication uiapplication)
        {
            UIDocument uidoc = uiapplication.ActiveUIDocument;
            var app = uiapplication.Application;
            Document doc = uidoc.Document;
        }

        public void UnhookAllBinds()
        {
            UpdateAddinControl -= ExecuteUpdate;
            ViewModel.UnhookAllBinds();
            DataContext = null;
            ViewModel = null;
        }
    }
}
