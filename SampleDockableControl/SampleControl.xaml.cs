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

using System.Threading;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using SeamsLibUi;
using System.Diagnostics;

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

        private void OuterExec(UIApplication app)
        {

        }

        private void selectElement_Click(object sender, RoutedEventArgs e)
        {
            DefineExecute.Invoke(OldExec);
            ExEvent.Raise();
        }

        public void OldExec(UIApplication app)
        {
            Process proc = Process.GetCurrentProcess();

            TaskDialog.Show("Process info", proc.ProcessName);

            try
            {
                using (Transaction trans = new Transaction(doc, "change detail level"))
                {
                    trans.Start();

                    ViewDetailLevel dl = uidoc.ActiveView.DetailLevel;
                    ////uidoc.RefreshActiveView();
                    uidoc.ActiveView.DetailLevel = ViewDetailLevel.Coarse;
                    uidoc.RefreshActiveView();
                    //uidoc.ActiveView.DetailLevel = dl;

                    trans.Commit();
                }
            }
            catch (Exception exc)
            {
                TaskDialog.Show("Detail level change error", exc.Message);
            }

            Element elem = null;

            //try
            //{
            //    uidoc.ActiveView = activeView;
            //}
            //catch(Exception exc)
            //{
            //    TaskDialog.Show("Info", exc.Message);
            //}

            uidoc.RequestViewChange(uidoc.ActiveView);

            try
            {
                Reference elemRef = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
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

    public class SampleInContextCommand : IExternalEventHandler
    {
        UIDocument uidoc = null;
        Autodesk.Revit.ApplicationServices.Application app = null;
        Document doc = null;

        public void Execute(UIApplication uiapp)
        {
            uidoc = uiapp.ActiveUIDocument;
            app = uiapp.Application;
            doc = uidoc.Document;

            Process proc = Process.GetCurrentProcess();

            TaskDialog.Show("External Event Info", proc.ProcessName);

            try
            {
                using (Transaction trans = new Transaction(doc, "change detail level"))
                {
                    trans.Start();

                    //ViewDetailLevel dl = uidoc.ActiveView.DetailLevel;
                    ////uidoc.RefreshActiveView();
                    //uidoc.ActiveView.DetailLevel = ViewDetailLevel.Medium;
                    ////uidoc.RefreshActiveView();
                    //uidoc.ActiveView.DetailLevel = dl;

                    trans.Commit();
                }
            }
            catch (Exception exc)
            {
                TaskDialog.Show("Detail level change error", exc.Message);
            }
        }

        public string GetName()
        {
            return "SampleInContextCommand";
        }
    }
}
