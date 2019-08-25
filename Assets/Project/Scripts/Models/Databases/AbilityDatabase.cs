using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Database/Ability", fileName = "AbilityDatabase")]
public class AbilityDatabase : Database<Ability>
{
    [SerializeField] List<AbilityAttributs> attributs = new List<AbilityAttributs>();

    private void OnEnable()
    {
        elements = new List<Ability>()
        {
            new LightningBall()
        };
        setAbilitiesAttributs();
    }

    void setAbilitiesAttributs()
    {
        while(attributs.Count != elements.Count)
        {
            if (attributs.Count < elements.Count)
                attributs.Add(null);
            if (attributs.Count > elements.Count)
                attributs.RemoveAt(attributs.Count - 1);
        }

        for(int i = 0; i < elements.Count; i++)
        {
            elements[i].setAttributs(attributs[i]);
        }
    }

    public Ability getAbilityOfType(Type abilityType)
    {
        for(int i =0; i < getDatabaseSize(); i++)
        {
            Ability ability = getElementAt(i);
            if(abilityType == ability.GetType())
            {
                return getElementAt(i);
            }
        }
        return null;
    }

    public void updateAbilityAttribut(AbilityAttributs attribut, int index)
    {
        if(index < elements.Count)
        {
            attributs[index] = attribut;
            setAbilitiesAttributs();
        }
    }
}
