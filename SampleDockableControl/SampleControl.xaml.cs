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

namespace SampleDockableControl
{
    /// <summary>
    /// Логика взаимодействия для UserControl2.xaml
    /// </summary>
    public partial class SampleControl : UserControl , IRevitAccessProvider<Action<UIApplication>, ExternalEvent>
    {
        private UIApplication uiapp = null;
        private UIDocument uidoc = null;
        private Autodesk.Revit.ApplicationServices.Application app = null;
        private Document doc = null;

        private ElementSet elementSet = null;

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

        public SampleControl()
        {
            InitializeComponent();
        }

        public void SetRevitAccess(object uiapp, object uidoc, object app, object doc, object elementSet)
        {
            this.uiapp = uiapp as UIApplication;
            this.uidoc = uidoc as UIDocument;
            this.app = app as Autodesk.Revit.ApplicationServices.Application;
            this.doc = doc as Document;
            this.elementSet = elementSet as ElementSet;
        }

        public void SetRevitAccess(object commandData, object elementSet)
        {
            ExternalCommandData cd = commandData as ExternalCommandData;
            this.uiapp = cd.Application;
            this.uidoc = uiapp.ActiveUIDocument;
            this.app = uiapp.Application;
            this.doc = uidoc.Document;
            this.elementSet = elementSet as ElementSet;
        }

        public UserControl GetControl()
        {
            return this;
        }

        private void getDocName_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(doc.PathName.ToString().Split('\\').Last());
        }

        private void selectElement_Click(object sender, RoutedEventArgs e)
        {
            DefineExecute.Invoke(ExecSelectionSample);
            ExEvent.Raise();
        }

        private void changeDetailLevel_Click(object sender, RoutedEventArgs e)
        {
            DefineExecute.Invoke(ExecTransactionSample);
            ExEvent.Raise();
        }

        public void ExecTransactionSample(UIApplication app)
        {
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
