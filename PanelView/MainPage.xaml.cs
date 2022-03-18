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

using System.IO;
using System.Reflection;
using Microsoft.Win32;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;

using Autodesk.Revit.ApplicationServices;
using SeamsLibUi;
using static SeamsLibUi.ExecuteProvider;

namespace PanelView
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>    
    public partial class MainPage : Page, IMainDockPanel, IDockablePaneProvider, IDockPanelWpfView
    {
        private Assembly addinControlAssembly = null;
        private UserControl addinControl = null;
        
        public MainPage()
        {
            UpdateDockPage += ExecuteUpdate;
            ViewModel = new MainPageViewModel();
            DataContext = viewModel;
            InitializeComponent();
        }

        private MainPageViewModel viewModel;
        public MainPageViewModel ViewModel
        {
            get { return viewModel; }
            set
            {
                viewModel = value;
            }
        }

        public object GetViewElement()
        {
            return this;
        }

        public void ExecuteUpdate()
        {
            DefineExternalExecute(UpdateView);
            ExternalExecuteCaller.Raise();
        }

        public void UpdateView(UIApplication uiapplication)
        {
            UIDocument uidoc = uiapplication.ActiveUIDocument;
            var app = uiapplication.Application;
            Document doc = uidoc.Document;

            // get the current document name
            docName.Text = doc.PathName.ToString().Split('\\').Last();
            // get the active view name
            viewName.Text = doc.ActiveView.Name;
        }

        public void UnhookAllBinds()
        {
            UpdateDockPage -= ExecuteUpdate;
            UpdateDockViewModel -= viewModel.ExecuteUpdate;
            DataContext = null;
            ViewModel = null;
        }
    }
}
