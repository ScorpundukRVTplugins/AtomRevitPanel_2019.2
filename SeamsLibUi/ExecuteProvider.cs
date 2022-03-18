using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;

namespace SeamsLibUi
{
    public static class ExecuteProvider
    {
        public static Action<Action<UIApplication>> StaticDefineExecute;
        public static ExternalEvent StaticExecuteCaller;
    }
}
