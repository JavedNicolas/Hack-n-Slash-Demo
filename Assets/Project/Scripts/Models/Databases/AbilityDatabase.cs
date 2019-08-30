using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(menuName = "Database/Ability", fileName = DatabaseConstant.abilityDatabaseFileName)]
public class AbilityDatabase : Database<Ability>
{
    [SerializeField] List<AbilityAttributs> attributs = new List<AbilityAttributs>();

    private void OnEnable()
    {
        _elements = new List<Ability>()
        {
            new LightningBall()
        };
        setAbilitiesAttributs();
    }

    public override int getFreeId()
    {
        int lastID = -1;
        for (int i = 0; i < _elements.Count; i++)
        {
            int currentID = _elements[i].databaseID;
            if (lastID != -1 && currentID - lastID > 1)
                return lastID + 1;
        }

        return _elements.Count;
    }

    void setAbilitiesAttributs()
    {
        while(attributs.Count != _elements.Count)
        {
            if (attributs.Count < _elements.Count)
                attributs.Add(null);
            if (attributs.Count > _elements.Count)
                attributs.RemoveAt(attributs.Count - 1);
        }

        for(int i = 0; i < _elements.Count; i++)
        {
            _elements[i].setAttributs(attributs[i]);
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
        if(index < _elements.Count)
        {
            attributs[index] = attribut;
            setAbilitiesAttributs();
        }
    }


}
