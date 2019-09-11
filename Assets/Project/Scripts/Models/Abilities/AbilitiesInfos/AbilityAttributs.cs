using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAttributs : ScriptableObject
{
    [Header("General")]
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _levelNeeded;
    [SerializeField] private int _maxLevel;
    [SerializeField] private List<AbilityTag> _abilityTags;

    [Header("Ability")]
    [SerializeField] private float _manaCost;
    [SerializeField] private AbilityCoolDownType _coolDownType;
    [SerializeField] private bool _needTarget;
    [SerializeField] private bool _canBeCastedOnSelf;

    [Header("Effects")]
    [SerializeField] private List<AbilityEffectAndValue> _effectAndValues = new List<AbilityEffectAndValue>();

    public new string name { get => _name;}
    public string description { get => _description; }
    public int levelNeeded{ get => _levelNeeded; }
    public int maxLevel { get => _maxLevel; }
    public Sprite icon { get => _icon; }
    public float manaCost { get => _manaCost; }
    public AbilityCoolDownType coolDownType { get => _coolDownType; }
    public bool needTarget { get => _needTarget;}
    public bool canBeCastedOnSelf { get => _canBeCastedOnSelf; }
    public List<AbilityTag> abilityTags { get => _abilityTags; }
    public List<AbilityEffectAndValue> effectAndValues { get => _effectAndValues; }
}