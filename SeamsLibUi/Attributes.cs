using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsLibUi
{
    // Создаем атрибут
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DockablePaneViewAttribute : System.Attribute
    {
        public bool isMainPanelView = false;
        public AddinDocTypeUsage addinUsage = AddinDocTypeUsage.Both;
        public DockablePaneViewAttribute() { }
        public DockablePaneViewAttribute(bool isMainPanelView, AddinDocTypeUsage addinUsage)
        {
            this.isMainPanelView = isMainPanelView;
            this.addinUsage = addinUsage;
        }

        public enum AddinDocTypeUsage
        {
            RvtDoc = 2,
            RvtFamily = 4,
            Both = 8,
        }
    }
}
