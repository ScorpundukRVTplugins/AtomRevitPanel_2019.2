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
    // IRevitContextAccess realization
    public partial class MainPage
    {
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
            DefineExternalExecute.Invoke(CheckExternalEvent);
            // didnt work - outside of revit context
            try
            {
                ExternalExecuteCaller.Raise();
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
            catch (Exception exc)
            {
                TaskDialog.Show("Info error", exc.Message);
            }
        }
    }
}
