﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI.Events;

namespace AtomRevitPanel
{
    public partial class AtomRevitPanel : IExternalApplication
    {
        private RibbonPanel ribbonPanel;
        public RibbonPanel RibbonPanel
        {
            get { return ribbonPanel; }
            private set { ribbonPanel = value; }
        }

        private string assemblyPath;
        public string AssemblyPath
        {
            get { return assemblyPath; }
            private set { assemblyPath = value; }
        }

        private Assembly addinEntryAssembly;
        public Assembly AddinEntryAssembly
        {
            get { return addinEntryAssembly; }
            private set { addinEntryAssembly = value; }
        }

        /* поле с "запасом" - возможно, в будущем
         * я буду применять более "чистую" архитектуру
         * тогда понадобится переменная для сборки
         * с интерфейсом приложения
         */
        private Assembly windowPanelAssembly;
        public Assembly WindowPanelAssembly
        {
            get { return windowPanelAssembly; }
            private set { windowPanelAssembly = value; }
        }

        private string nameSpace = "AtomRevitPanel";
        public string NameSpace
        {
            get { return nameSpace; }
            private set { nameSpace = value; }
        }

        public ImageSource GetResourceImage(Assembly assembly, string imageName)
        {
            try
            {
                // bitmap stream to costruct bitmap frame
                Stream resource = assembly.GetManifestResourceStream(imageName);

                // return image data
                return BitmapFrame.Create(resource);
            }
            catch
            {
                return null;
            }
        }

        public void AddNewRibbon(UIControlledApplication application)
        {
            // create a ribbon panel
            RibbonPanel = application.CreateRibbonPanel(Tab.AddIns, "Atomic tools");
            // assembly
            AddinEntryAssembly = Assembly.GetExecutingAssembly();
            // assembly path
            AssemblyPath = AddinEntryAssembly.Location;
        }

        public void AddButtonOnRibbon(
            string name, 
            string text,
            string toolTipText,
            Type commandClassType, 
            Type availabilityCheckClass,
            string largeImageName,
            string imageName)
        {
            PushButtonData pushButtonData = new PushButtonData(
                name,
                text,
                AssemblyPath,
                $"{NameSpace}.{commandClassType.Name}"
                );

            // Create Register Button
            PushButton registerButton = RibbonPanel.AddItem(pushButtonData) as PushButton;

            if(availabilityCheckClass != null)
            {
                // accessibility check for register Button
                registerButton.AvailabilityClassName =
                    $"{NameSpace}.{availabilityCheckClass.Name}";
            }

            // btn tooltip 
            registerButton.ToolTip = toolTipText;
            //"Register dockable window at the zero document state."

            // register button icon images
            registerButton.LargeImage = 
                GetResourceImage(
                    AddinEntryAssembly, 
                    $"{NameSpace}.Resources.{largeImageName}");

            registerButton.Image = 
                GetResourceImage(
                    AddinEntryAssembly,
                    $"{NameSpace}.Resources.{imageName}");
        }
    }
}
