using UnityEngine;
using System.Collections;

public static class DescriptionText
{ 
    public static string getElementTypeText(Element Element)
    {
        switch (Element)
        {
            case Element.Physical: return "PhysicalDamage_Description".localize();
            case Element.Lightning: return "LightningDamage_Description".localize();
            case Element.Fire: return "FireDamage_Description".localize();
            case Element.Wind: return "WindDamage_Description".localize();
            case Element.Ice: return "IceDamage_Description".localize();
            case Element.Arcane: return "ArcaneDamage_Description".localize();
        }

        return "";
    }

}
