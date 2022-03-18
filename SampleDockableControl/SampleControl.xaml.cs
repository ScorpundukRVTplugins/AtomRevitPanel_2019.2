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

using SeamsLibUi;
using static SeamsLibUi.ExecuteProvider;

namespace SampleDockableControl
{
    /// <summary>
    /// Логика взаимодействия для UserControl2.xaml
    /// </summary>
    public partial class SampleControl : UserControl, IDockPanelWpfView
    {
        public SampleControl()
        {
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

        public void UpdateView(UIApplication uiapplication)
        {
            UIDocument uidoc = uiapplication.ActiveUIDocument;
            var app = uiapplication.Application;
            Document doc = uidoc.Document;
        }

        public UserControl GetControl()
        {
            return this;
        }

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

        private void GetDocName(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
            MessageBox.Show(doc.PathName.ToString().Split('\\').Last());
        }

        public void ExecTransactionSample(UIApplication app)
        {
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                using (Transaction trans = new Transaction(doc, "change detail level"))
                {
                    trans.Start();
                    ViewDetailLevel dl = uidoc.ActiveView.DetailLevel;
                    if(dl == ViewDetailLevel.Fine)
                    {
                        uidoc.ActiveView.DetailLevel = ViewDetailLevel.Coarse;
                    }
                    else
                    {
                        uidoc.ActiveView.DetailLevel = ViewDetailLevel.Fine;
                    }
                    trans.Commit();
                }
            }
            catch (Exception exc)
            {
                TaskDialog.Show("Detail level change error", exc.Message);
            }
        }

        public void ExecSelectionSample(UIApplication app)
        {
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;
            Element elem = null;
            Reference elemRef = null;

            TaskDialog.Show("ACTION AD", "Please click in view, then select element");
            try
            {
                XYZ pointRef = uidoc.Selection.PickPoint();
            }
            catch { }

            try
            {
                elemRef = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                ElementId elemId = elemRef.ElementId;
                elem = doc.GetElement(elemId);
            }
            catch (Exception exc)
            {
                TaskDialog.Show("Element selection error", exc.Message);
            }

            if (elem != null)
                TaskDialog.Show("Element info", elem.Name);
        }        
    }
}
