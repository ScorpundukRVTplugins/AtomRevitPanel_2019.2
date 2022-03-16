using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace MVVM
{
    public class RevitModelBridge
    {
        UIApplication uiapp;
        UIDocument uidoc;
        Application app;
        Document doc;

        public RevitModelBridge(ExternalCommandData commandData)
        {
            Uiapp = commandData.Application;
            Uidoc = uiapp.ActiveUIDocument;
            App = uiapp.Application;
            Doc = uidoc.Document;
        }

        public UIApplication Uiapp
        {
            get{ return uiapp; }
            set{ uiapp = value; }
        }

        public UIDocument Uidoc
        {
            get{ return uidoc;}
            set{uidoc = value;}
        }

        public Application App
        {
            get{return app;}
            set{app = value;}
        }

        public Document Doc
        {
            get{return doc;}
            set{doc = value;}
        }

    }
}
