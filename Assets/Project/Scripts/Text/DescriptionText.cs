using UnityEngine;
using System.Collections;

public static class DescriptionText
{ 
    public static string getElementTypeText(Element Element)
    {
        switch (Element)
        {
            case Element.Physical: return "PhysicalElement_Description".localize();
            case Element.Lightning: return "LightningElement_Description".localize();
            case Element.Fire: return "FireElement_Description".localize();
            case Element.Wind: return "WindElement_Description".localize();
            case Element.Ice: return "IceElement_Description".localize();
            case Element.Arcane: return "ArcaneElement_Description".localize();
        }

        return "";
    }

}
