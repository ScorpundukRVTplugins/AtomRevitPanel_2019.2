﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;

using SeamsLibUi;
using MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;

using static SeamsLibUi.ExecuteProvider;

namespace SampleDoorsWindowsKKS
{
    public partial class ControlViewModel : ViewModelBase
    {
        RelayCommand collectDoorsCmd;
        public ICommand CollectDoorsCmd
        {
            get
            {
                if (collectDoorsCmd == null)
                {
                    collectDoorsCmd =
                        new RelayCommand(CollectDoorsExecute, CollectDoorsCanExecute);
                }
                return collectDoorsCmd;
            }
        }

        public void CollectDoorsExecute(object parameter)
        {
            try
            {
                DefineExternalExecute(CollectDoors);
                ExternalExecuteCaller.Raise();
            }
            catch (Exception exc)
            {
                TaskDialog.Show("Static members call failed", exc.Message);
            }

        }

        public bool CollectDoorsCanExecute(object parameter)
        {
            return true;
        }

    }
}
