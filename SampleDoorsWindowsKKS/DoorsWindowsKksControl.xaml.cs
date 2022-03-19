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
using MVVM;

namespace SampleDoorsWindowsKKS
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class DoorsWindowsKksControl : UserControl, IDockPanelWpfView, IViewUpdater
    {
        public DoorsWindowsKksControl()
        {
            InitializeComponent();
            ViewModel = new ControlViewModel();
            DataContext = ViewModel;
        }

        private ControlViewModel viewModel;
        public ControlViewModel ViewModel
        {
            get { return viewModel; }
            set { viewModel = value; }
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


        public object GetViewElement()
        {
            return this;
        }

        public void UnhookAllBinds()
        {
            ViewModel.UnhookAllBinds();
            DataContext = null;
            ViewModel = null;
        }

        public IViewUpdater GetViewModelUpdater()
        {
            return ViewModel as IViewUpdater;
        }

        public IViewUpdater GetViewUpdater()
        {
            return this as IViewUpdater;
        }
    }
}
