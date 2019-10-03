using UnityEngine;
using System.Collections;

public class PassiveNode 
{
    [SerializeField] string _name;
    [SerializeField] Sprite _icon;
    [SerializeField] NodeEffect _nodeEffect;
    [SerializeField] int _numberOfLevel;
    [SerializeField] int _currentLevel;

    public new string name { get => _name; }
    public Sprite icon { get => _icon; }
    public NodeEffect nodeEffect { get => _nodeEffect; }
    public int numberOfLevel { get => _numberOfLevel; }
    public int currentLevel { get => _currentLevel; }
}
