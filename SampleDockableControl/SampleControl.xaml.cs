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
using SeamsLibUi;

namespace SampleDockableControl
{
    /// <summary>
    /// Логика взаимодействия для UserControl2.xaml
    /// </summary>
    public partial class SampleControl : UserControl , IRevitAccessProvider
    {
        private UIApplication uiapp = null;
        private UIDocument uidoc = null;
        private Autodesk.Revit.ApplicationServices.Application app = null;
        private Document doc = null;
        private ElementSet elementSet = null;

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
    }
}
