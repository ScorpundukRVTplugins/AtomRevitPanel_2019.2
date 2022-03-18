﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Autodesk.Revit.UI;

namespace SeamsLibUi
{
    public interface IDockPanelWpfView
    {
        void ExecuteUpdate();
        void UpdateView(UIApplication uiapplication);        
        object GetViewElement();
        void UnhookAllBinds();
    }
}