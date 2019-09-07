﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAttributs : ScriptableObject
{
    [Header("General")]
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;

    [Header("Ability")]
    [SerializeField] private float _manaCost;
    [SerializeField] private AbilityType _type;
    [SerializeField] private List<EffectAndValue> _effectAndValues = new List<EffectAndValue>();
    [SerializeField] private AbilityCoolDownType _coolDownType;
    [SerializeField] private bool _needTarget;
    [SerializeField] private bool _canBeCastedOnSelf;

    public new string name { get => _name;}
    public string description { get => _description; }
    public Sprite icon { get => _icon; }
    public float manaCost { get => _manaCost; }
    public AbilityCoolDownType coolDownType { get => _coolDownType; }
    public bool needTarget { get => _needTarget;}
    public bool canBeCastedOnSelf { get => _canBeCastedOnSelf; }
    public AbilityType type { get => _type; }
    public List<EffectAndValue> effectAndValues { get => _effectAndValues; set => _effectAndValues = value; }

}

[System.Serializable]
public class EffectAndValue
{
    [SerializeField] public float value;
    [SerializeField] public Effect effect;

    public EffectAndValue() { }

    public void addEffectAndValue(Effect effect, float value)
    {
        this.effect = effect;
        this.value = value;
    } 

    public string getDescription()
    {
        return effect.description.Replace("{0}", value.ToString());
    }

}