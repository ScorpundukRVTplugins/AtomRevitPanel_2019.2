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
    public partial class DoorsWindowsKksControl : UserControl, IDockControl, IDockElementUpdater
    {
        public DoorsWindowsKksControl()
        {
            InitializeComponent();            
        }

        private ControlViewModel viewModel;
        public ControlViewModel ViewModel
        {
            get { return viewModel; }
            set { viewModel = value; }
        }

        #region IDockPanelUpdater

        public void ExecuteUpdate()
        {
            DefineExternalExecute(UpdateState);
            if (!ExternalExecuteCaller.IsPending)
                ExternalExecuteCaller.Raise();
        }

        public void UpdateState(UIApplication uiapplication)
        {
            UIDocument uidoc = uiapplication.ActiveUIDocument;
            var app = uiapplication.Application;
            Document doc = uidoc.Document;
        }        

        #endregion

        #region IAddinControl

        public IDockViewModel GetDockControlViewModel()
        {
            return ViewModel as IDockViewModel;
        }

        public void SetupDockControl(IDockViewModel viewModel)
        {
            ViewModel = viewModel as ControlViewModel;
            DataContext = ViewModel;
            UpdateAddinControl += ExecuteUpdate;
            UpdateAddinViewModel += ViewModel.ExecuteUpdate;
        }        

        public void ResetDockControl()
        {
            UpdateAddinControl -= ExecuteUpdate;
            UpdateAddinViewModel -= ViewModel.ExecuteUpdate;
            DataContext = null;
            ViewModel = null;
        }

        #endregion 
    }
}
