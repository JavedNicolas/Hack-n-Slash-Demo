using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAttributs : ScriptableObject
{
    [Header("General")]
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;


    [Header("Ability")]
    [SerializeField] private AbilityType _type;
    [SerializeField] private EffectAndValues _effectAndValues = new EffectAndValues();
    [SerializeField] private AbilityCoolDownType _coolDownType;
    [SerializeField] private bool _needTarget;
    [SerializeField] private bool _canBeCastedOnSelf;

    public new string name { get => _name;}
    public string description { get => _description; }
    public Sprite icon { get => _icon; }
    public AbilityCoolDownType coolDownType { get => _coolDownType; }
    public bool needTarget { get => _needTarget;}
    public bool canBeCastedOnSelf { get => _canBeCastedOnSelf; }
    public AbilityType type { get => _type; }
    public EffectAndValues effectAndValues { get => _effectAndValues; set => _effectAndValues = value; }

}

[System.Serializable]
public class EffectAndValues
{
    [SerializeField] private List<float> _effectsValues = new List<float>();
    [SerializeField] private List<Effect> _effects = new List<Effect>();

    public EffectAndValues() { }

    public void addEffectAndValue(Effect effect, float value)
    {
        _effects.Add(effect);
        _effectsValues.Add(value);
    } 

    public List<float> effectValues { get => _effectsValues; set => _effectsValues = value; }
    public List<Effect> effects { get => _effects; set => _effects = value; }
}
