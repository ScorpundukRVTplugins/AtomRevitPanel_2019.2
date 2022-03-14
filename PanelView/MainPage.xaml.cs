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

namespace PanelView
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page, IDockablePaneProvider
    {
        private UIApplication uiapp = null;
        private UIDocument uidoc = null;
        private Autodesk.Revit.ApplicationServices.Application app = null;
        private Document doc = null;
        private ElementSet elementSet = null;
        private ExternalCommandData commandData = null;

        private Assembly addinControlAssembly = null;
        private UserControl addinControl = null;        

        public Action<Action<UIApplication>> DefineExecute
        {
            get;
            set;
        }
        public ExternalEvent ExEvent
        {
            get;
            set;
        }

        public MainPage()
        {            
            InitializeComponent();
        }

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

        // method name in sample CustomInitiator
        public void InitiateRevitAccess(ExternalCommandData commandData, ElementSet elements)
        {
            this.commandData = commandData;
            uiapp = commandData.Application;
            uidoc = uiapp.ActiveUIDocument;
            app = uiapp.Application;
            doc = uidoc.Document;
            elementSet = elements;

            // get the current document name
            docName.Text = doc.PathName.ToString().Split('\\').Last();
            // get the active view name
            viewName.Text = doc.ActiveView.Name;
            
            if(addinControl != null)
            (addinControl as IRevitAccessProvider<Action<UIApplication>, ExternalEvent>).SetRevitAccess(commandData, elements);
        }

        public void AddAddinControl()
        {
            //https://translated.turbopages.org/proxy_u/en-ru.ru.4faa0809-622b2b60-cf3d9858-74722d776562/https/stackoverflow.com/questions/123391/how-to-unload-an-assembly-from-the%20-primary-appdomain
            
            string assemblyPath = "";
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();            
                if (fileDialog.ShowDialog() == true)
                {
                    //assemblyPath = fileDialog.FileName;
                    assemblyPath = fileDialog.FileName;
                }
            }
            catch(Exception e)
            {
                TaskDialog.Show("Open file exception:", e.Message);
            }

            try
            {
                //addinControlAssembly = Assembly.LoadFrom(assemblyPath);
                addinControlAssembly = Assembly.Load(File.ReadAllBytes(assemblyPath));
                StringBuilder sb = new StringBuilder();
                sb.Append($"{addinControlAssembly.GetName()}\n");
                foreach(Type type in addinControlAssembly.DefinedTypes)
                {
                    sb.Append($"{type.Namespace}...{type.Name}\n");
                }
                MessageBox.Show(sb.ToString());
                //TaskDialog.Show("Assembly", sb.ToString());
            }
            catch(Exception e)
            {
                TaskDialog.Show("Assembly info error", e.Message);
            }

            Type typeOfcontrol = addinControlAssembly.DefinedTypes
                .Where(typeinfo => typeinfo.GetInterfaces().Contains(typeof(IRevitAccessProvider<Action<UIApplication>, ExternalEvent>))).First();

            if(typeOfcontrol != null)
            {
                TaskDialog.Show("Type if found", $"{typeOfcontrol.Name}");
                IRevitAccessProvider<Action<UIApplication>, ExternalEvent> proxy
                    = Activator.CreateInstance(typeOfcontrol) as IRevitAccessProvider<Action<UIApplication>, ExternalEvent>;
                proxy.SetRevitAccess(commandData, elementSet);
                proxy.DefineExecute = DefineExecute;
                proxy.ExEvent = ExEvent;
                addinControl = proxy.GetControl();                
                panelGrid.Children.Add(addinControl);
                System.Windows.Controls.Grid.SetRow(addinControl, 2);
            }
        }

        public void RemoveAddinControl()
        {
            panelGrid.Children.Remove(addinControl);
            addinControl = null;
            addinControlAssembly = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddAddinControl();
        }

        private void removeAddin_Click(object sender, RoutedEventArgs e)
        {
            RemoveAddinControl();
        }


        private void externalEventRise_Click(object sender, RoutedEventArgs e)
        {
            DefineExecute.Invoke(CheckExternalEvent);
            // didnt work - outside of revit context
            try
            {
                ExEvent.Raise();
            }
            catch
            {

            }
            //CommandProvider -= CheckExternalEvent;
        }

        private void CheckExternalEvent(UIApplication app)
        {
            try
            {
                using (Transaction trans = new Transaction(app.ActiveUIDocument.Document, "sample"))
                {
                    trans.Start();
                    trans.Commit();
                }                
                TaskDialog.Show("VICTORY!", "VICTORY!");
            }
            catch(Exception exc)
            {
                TaskDialog.Show("Info error", exc.Message);
            }            
        }
    }

    public class SampleExternalEvent : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            using(Transaction trans = new Transaction(app.ActiveUIDocument.Document, "sample"))
            {
                trans.Start();

                trans.Commit();
            }
        }

        public string GetName()
        {
            return nameof(SampleExternalEvent);
        }
    }    
}
