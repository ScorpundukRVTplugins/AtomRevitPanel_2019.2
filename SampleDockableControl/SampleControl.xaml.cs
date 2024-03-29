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
        public SampleControl()
        {
            UpdateAddinControl += ExecuteUpdate;
            ViewModel = new ControlViewModel();
            DataContext = ViewModel;
            InitializeComponent();
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
