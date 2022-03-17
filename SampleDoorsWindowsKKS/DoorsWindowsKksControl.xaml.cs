﻿using System;
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

using SeamsLibUi;
using MVVM;

namespace SampleDoorsWindowsKKS
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class DoorsWindowsKksControl : UserControl, IRevitContextAccess
    {
        public DoorsWindowsKksControl()
        {
            ViewModel = new ControlViewModel();
            DataContext = viewModel;
            InitializeComponent();
        }

        public Action<Action<UIApplication>> DefineExternalExecute { get; set; }
        public ExternalEvent ExternalExecuteCaller { get; set; }

        private ControlViewModel viewModel;
        public IPanelControlViewModel ViewModel
        {
            get { return viewModel as IPanelControlViewModel; }
            set { viewModel = (ControlViewModel)value; }
        }

        public object GetViewElement()
        {
            return this;
        }

        public void UpdateView(UIApplication uiapplication)
        {
            UIDocument uidoc = uiapplication.ActiveUIDocument;
            var app = uiapplication.Application;
            Document doc = uidoc.Document;
        }

        
    }
}