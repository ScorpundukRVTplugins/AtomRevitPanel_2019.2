﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DockApplicationBase;
using static DockApplicationBase.ExecuteProvider;

using MVVM;
using Autodesk.Revit.UI;

namespace PanelView
{
    public class MainPageViewModel : ViewModelBase, IUpdateSubscriber
    {
        public MainPageViewModel() : base()
        {
            UpdateDockViewModel += ExecuteUpdate;
        }

        public void ExecuteUpdate()
        {
            DefineExternalExecute(UpdateState);
            ExternalExecuteCaller.Raise();
        }

        public void UpdateState(UIApplication uiapplication)
        {
            
        }

        public void UnhookAllBinds()
        {
            UpdateDockViewModel -= ExecuteUpdate;
        }
    }
}
