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
using DockApplicationBase;
using static DockApplicationBase.ExecuteProvider;

namespace PanelView
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>    
    public partial class MainPage : Page, IDockablePaneProvider, IDockPanel, IViewElementUpdater
    {
        private Assembly addinControlAssembly = null;
        private UserControl addinControl = null;
        
        public MainPage()
        {
            InitializeComponent();
        }

        private MainPageViewModel viewModel;
        public MainPageViewModel ViewModel
        {
            get { return viewModel; }
            set { viewModel = value; }
        }

        #region IDockElementUpdater

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

            // get the current document name
            docName.Text = doc.PathName.ToString().Split('\\').Last();
            // get the active view name
            viewName.Text = doc.ActiveView.Name;
        }

        #endregion

        #region IDockablePaneProvider implementation

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this as FrameworkElement;
            data.InitialState = new DockablePaneState()
            {
                DockPosition = DockPosition.Tabbed,
                TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser,
            };
            data.VisibleByDefault = false;
        }

        #endregion

        #region IDockPanel implementation

        public IDockablePaneProvider GetDockProvider()
        {
            return this as IDockablePaneProvider;
        }

        public IDockAddinControl GetAddinControl()
        {
            return addinControl as IDockAddinControl;
        }

        public IDockViewModel GetDockViewModel()
        {
            return ViewModel as IDockViewModel;
        }

        public void RemoveAddinControl()
        {
            try
            {
                (addinControl as IDockAddinControl).ResetAddinView();
            }
            catch (Exception exc)
            {
                TaskDialog.Show("Type cast error", exc.Message);
            }
            panelGrid.Children.Remove(addinControl);
            addinControl = null;
            addinControlAssembly = null;
        }

        public void AddAddinControl()
        {
            //https://stackoverflow.com/questions/123391/how-to-unload-an-assembly-from-the%20-primary-appdomain

            string assemblyPath = "";
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() == true)
                {
                    assemblyPath = fileDialog.FileName;
                }
            }
            catch (Exception e)
            {
                TaskDialog.Show("Open file exception:", e.Message);
            }

            try
            {
                addinControlAssembly = Assembly.Load(File.ReadAllBytes(assemblyPath));
            }
            catch (Exception e)
            {
                TaskDialog.Show("Assembly info error", e.Message);
            }

            Type typeOfcontrol = addinControlAssembly.DefinedTypes
                .Where(typeinfo => typeinfo.GetInterfaces().Contains(typeof(IDockAddinControl))).First();

            Type typeOfViewModel = addinControlAssembly.DefinedTypes
                .Where(typeinfo => typeinfo.GetInterfaces().Contains(typeof(IDockViewModel))).First();

            if (typeOfcontrol != null)
            {
                IDockAddinControl controlView
                    = Activator.CreateInstance(typeOfcontrol) as IDockAddinControl;

                IDockViewModel controlViewModel
                    = Activator.CreateInstance(typeOfViewModel) as IDockViewModel;

                controlView.SetupAddinView(controlViewModel);

                addinControl = controlView as UserControl;
                panelGrid.Children.Add(addinControl);
                System.Windows.Controls.Grid.SetRow(addinControl, 1);

                InvokeAddinControlUpdate();
                InvokeAddinViewModelUpdate();
            }
        }

        public void SetupDockView(IDockViewModel viewModel)
        {
            ViewModel = viewModel as MainPageViewModel;
            DataContext = ViewModel;
            UpdateDockPage += ExecuteUpdate;
            UpdateDockViewModel += ViewModel.ExecuteUpdate;
        }

        public void ResetDockView()
        {
            UpdateDockPage -= ExecuteUpdate;
            UpdateDockViewModel -= ViewModel.ExecuteUpdate;
            DataContext = null;
            ViewModel = null;
        }

        #endregion
    }
}