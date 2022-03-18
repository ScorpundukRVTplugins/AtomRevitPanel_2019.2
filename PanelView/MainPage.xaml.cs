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

using System.IO;
using System.Reflection;
using Microsoft.Win32;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;

using Autodesk.Revit.ApplicationServices;
using SeamsLibUi;

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
            MainPageViewModel viewModel = new MainPageViewModel();
            DataContext = viewModel;
            InitializeComponent();
        }             

        /* внутренний метод, одноимённый с методом интерфейса IRevitContextAccess
         * он может быть каким угодно в зависимости от устройства
         * конкретного класса панели.
         * Его задача - обновлять вид, и ничего больше
         */
        public void UpdateView(Document doc)
        {
            // get the current document name
            docName.Text = doc.PathName.ToString().Split('\\').Last();
            // get the active view name
            viewName.Text = doc.ActiveView.Name;
        }       
    }   
}
