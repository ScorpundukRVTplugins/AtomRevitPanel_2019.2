using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockApplicationBase
{
    // Создаем атрибут
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DockablePaneViewAttribute : System.Attribute
    {
        public string addinName;
        public AddinDocTypeUsage addinUsage = AddinDocTypeUsage.Both;
        public DockablePaneViewAttribute() { }
        public DockablePaneViewAttribute(string addinName, AddinDocTypeUsage addinUsage)
        {
            this.addinName = addinName;
            this.addinUsage = addinUsage;
        }
    }

    public enum AddinDocTypeUsage
    {
        RvtDoc = 2,
        RvtFamily = 4,
        Both = 8,
    }
}
