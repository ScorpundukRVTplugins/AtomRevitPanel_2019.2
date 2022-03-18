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
    // IMainDockPanel realization
    public partial class MainPage
    {
        public IDockablePaneProvider GetDockProvider()
        {
            return this as IDockablePaneProvider;
        }

        public IDockPanelWpfView GetRevitAccess()
        {
            return this as IDockPanelWpfView;
        }

        public void ContextEventUpdate()
        {
            DefineExternalExecute(UpdateView);
            ExternalExecuteCaller.Raise();
        }

        public void UpdateCurrentControl(UIApplication uiapplication)
        {
            if (addinControl != null)
                (addinControl as IDockPanelWpfView).UpdateView(uiapplication);
        }

        public void RemoveAddinControl()
        {
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
                .Where(typeinfo => typeinfo.GetInterfaces().Contains(typeof(IDockPanelWpfView))).First();

            if (typeOfcontrol != null)
            {
                IDockPanelWpfView controlView
                    = Activator.CreateInstance(typeOfcontrol) as IDockPanelWpfView;

                DefineExternalExecute( (UIApplication uiapp) => controlView.UpdateView(uiapp));
                ExternalExecuteCaller.Raise();

                addinControl = controlView.GetViewElement() as UserControl;
                panelGrid.Children.Add(addinControl);
                
                System.Windows.Controls.Grid.SetRow(addinControl, 2);
            }
        }
    }
}
