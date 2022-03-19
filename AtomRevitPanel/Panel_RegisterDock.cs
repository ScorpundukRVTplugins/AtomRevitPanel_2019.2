using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Interop;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI.Events;

using DockApplicationBase;
using static DockApplicationBase.ExecuteProvider;


namespace AtomRevitPanel
{
    public partial class AtomRevitPanel : IExternalApplication
    {
        public bool DockPanelRegister(UIControlledApplication application)
        {
            try
            {
                dockAccess = FindPanelViewAssembly() as IMainDockPanel;                
            }
            catch(Exception exc)
            {
                TaskDialog.Show("Main panel view assembly search error", exc.Message);
                return false;
            }           


            DockablePaneId dockId = new DockablePaneId(dockPanelGuid);
            try
            {
                //register dockable panel                
                application.RegisterDockablePane
                    (
                        dockId,
                        "Atom Revit Panel",
                        dockAccess.GetDockProvider()
                    );

            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("Info Message", ex.Message);
                return false;
            }
            return true;
        }

        /* поиск сборки, содержащей страницу WPF (WPF Page)
         * с интерфейсом IMainDockPanel
         * Тип страницы с интерфейсом IDockablePaneProvider 
         * может содержаться и в других сборках
         * но не будет подходить к данному приложению
         */
        public IMainDockPanel FindPanelViewAssembly()
        {
            string addinAssembly = Assembly.GetExecutingAssembly().Location;
            string addinFolder = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            DirectoryInfo addinFolderInfo = new DirectoryInfo(addinFolder);
            FileInfo[] files = addinFolderInfo.GetFiles("*.dll");
            int count = files.Length;
            foreach (FileInfo file in files)
            {
                Type mainPanelViewType = null;
                Assembly potentialMainPanelView = Assembly.Load(file.FullName);
                string name = potentialMainPanelView.GetName().Name;

                try
                {
                    mainPanelViewType = (from t in potentialMainPanelView.DefinedTypes
                                         where t.GetInterfaces().Contains(typeof(IMainDockPanel))
                                         select t).First();
                }
                catch { }

                if(mainPanelViewType != null && mainPanelViewType.BaseType == typeof(Page))
                {
                    return Activator.CreateInstance(mainPanelViewType) as IMainDockPanel;
                }
            }
            return null;
        }

        public static void ShowPanel(UIApplication uiapp)
        {
            try
            {
                // dockable window id
                DockablePaneId id = new DockablePaneId(AtomRevitPanel.dockPanelGuid);
                DockablePane dockableWindow = uiapp.GetDockablePane(id);
                (AtomRevitPanel.dockAccess as IViewUpdater).UpdateState(uiapp);

                dockableWindow.Show();
            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("DockablePane showing error", ex.Message);
            }

            uiapp.Application.DocumentOpened +=
                Application_DocumentOpened;

            // subscribe view activated event
            uiapp.ViewActivated +=
                Application_ViewActivated;

            uiapp.Application.DocumentChanged +=
                DocumentChangedHandler;
        }

        /* view activated event
         * срабатывает когда открывается или переключается новый вид
         */
        public static void Application_ViewActivated(object sender, ViewActivatedEventArgs e)
        {
            // provide ExternalCommandData object to dockable page
            //dockPanelView.InitiateRevitAccess(commandData, elements);
            //(AtomRevitPanel.dockAccess as IDockPanelWpfView).UpdateView(commandData.Application);
            dockAccess.GetDockPage().GetViewUpdater().ExecuteUpdate();
            dockAccess.GetDockPage().GetViewModelUpdater().ExecuteUpdate();
            if(dockAccess.GetAddinControl() != null)
            {
                dockAccess.GetAddinControl().GetViewUpdater().ExecuteUpdate();
                dockAccess.GetAddinControl().GetViewModelUpdater().ExecuteUpdate();
            }

                
            //InvokeDockPageUpdate();
            //InvokeDockViewModelUpdate();
        }

        /* document opened event
         * срабатывает когда открывается новый документ - в этом случае также срабатывает 
         * и ViewActivated
         */
        public static void Application_DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            // provide ExternalCommandData object to dockable page
            //(AtomRevitPanel.dockAccess as IDockPanelWpfView).UpdateView(commandData.Application);
            //if (AtomRevitPanel.firstOpenDone == false)
            //{
            //    InvokeDockPageUpdate();
            //    InvokeDockViewModelUpdate();
            //    AtomRevitPanel.firstOpenDone = true;
            //}
            dockAccess.GetDockPage().GetViewUpdater().ExecuteUpdate();
            dockAccess.GetDockPage().GetViewModelUpdater().ExecuteUpdate();
            if (dockAccess.GetAddinControl() != null)
            {
                dockAccess.GetAddinControl().GetViewUpdater().ExecuteUpdate();
                dockAccess.GetAddinControl().GetViewModelUpdater().ExecuteUpdate();
            }
            //InvokeDockPageUpdate();
            //InvokeDockViewModelUpdate();
        }

        public static void DocumentChangedHandler(object sender, DocumentChangedEventArgs e)
        {
            /* по неизвестной мне пока причине при вызове статических
             * событий, на которые подписаны методы обновления ViewModel
             * при том, что они отрабатывают - обновления интерфейса не происходит
             * похоже, что метод подписчик отрабатывает вне контекста
             * и ничего не изменяет в классе viewmodel
             * 
             * Если вызывается событие, на которое подписан метод, обновляющий 
             * сами поля класса контрола (например, текст в TextBlock)
             * - тогда всё работает
             */
            //InvokeAddinControlUpdate();
            //InvokeAddinViewModelUpdate();
            if (dockAccess.GetAddinControl() != null)
            {
                dockAccess.GetAddinControl().GetViewUpdater().ExecuteUpdate();
                dockAccess.GetAddinControl().GetViewModelUpdater().ExecuteUpdate();
            }
            //dockAccess.GetAddinControl().GetViewUpdater().ExecuteUpdate();
        }
    }   
}
