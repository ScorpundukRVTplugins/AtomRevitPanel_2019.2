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
    // IMainDockPanel realization
    public partial class MainPage
    {
        public IDockablePaneProvider GetDockProvider()
        {
            return this as IDockablePaneProvider;
        }

        public IRevitContextAccess GetRevitAccess()
        {
            return this as IRevitContextAccess;
        }

        public void ContextEventUpdate()
        {
            DefineExternalExecute(UpdateView);
            ExternalExecuteCaller.Raise();
        }

        public void UpdateCurrentControl(UIApplication uiapplication)
        {
            if (addinControl != null)
                (addinControl as IRevitContextAccess).UpdateView(uiapplication);
        }

        public void RemoveAddinControl()
        {
            panelGrid.Children.Remove(addinControl);
            addinControl = null;
            addinControlAssembly = null;
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
            catch (Exception e)
            {
                TaskDialog.Show("Open file exception:", e.Message);
            }

            try
            {                
                //addinControlAssembly = Assembly.LoadFrom(assemblyPath);
                addinControlAssembly = Assembly.Load(File.ReadAllBytes(assemblyPath));
                //StringBuilder sb = new StringBuilder();
                //sb.Append($"{addinControlAssembly.GetName()}\n");
                //foreach (Type type in addinControlAssembly.DefinedTypes)
                //{
                //    sb.Append($"{type.Namespace}...{type.Name}\n");
                //}
                //MessageBox.Show(sb.ToString());
                //TaskDialog.Show("Assembly", sb.ToString());
            }
            catch (Exception e)
            {
                TaskDialog.Show("Assembly info error", e.Message);
            }

            Type typeOfcontrol = addinControlAssembly.DefinedTypes
                .Where(typeinfo => typeinfo.GetInterfaces().Contains(typeof(IRevitContextAccess))).First();;

            if (typeOfcontrol != null)
            {
                //TaskDialog.Show("Type if found", $"{typeOfcontrol.Name}");
                IRevitContextAccess proxy
                    = Activator.CreateInstance(typeOfcontrol) as IRevitContextAccess;

                DefineExternalExecute( (UIApplication uiapp) => proxy.UpdateView(uiapp));
                ExternalExecuteCaller.Raise();

                proxy.DefineExternalExecute = DefineExternalExecute;
                proxy.ExternalExecuteCaller = ExternalExecuteCaller;
                addinControl = proxy.GetViewElement() as UserControl;
                panelGrid.Children.Add(addinControl);
                proxy.ViewModel.SetDefineExternalExecute(DefineExternalExecute);
                proxy.ViewModel.SefExternalExecuteCaller(ExternalExecuteCaller);
                System.Windows.Controls.Grid.SetRow(addinControl, 2);
            }
        }
    }
}
