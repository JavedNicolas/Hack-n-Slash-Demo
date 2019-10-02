using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Buff
{
    [SerializeField] public float _startingTime;
    public float startingTime  => _startingTime;

    [SerializeField] float _duration;
    public float duration => _duration;

    [SerializeField] string _name;
    public string name => _name;

    [SerializeField] List<Stat> _stats = new List<Stat>();
    public List<Stat> stats => _stats;

    [SerializeField] Sprite _icon;
    public Sprite icon => _icon; 

    [SerializeField] bool _isDebuff;
    public bool isDebuff => _isDebuff; 

    public Buff(string name, Sprite icon, float duration, List<Stat> stats, bool isDebuff = false)
    {
        this._duration = duration;
        this._name = name;
        this._icon = icon;
        this._stats = stats;
        this._isDebuff = isDebuff;
        resetStartTime();
    }

    public void resetStartTime()
    {
        this._startingTime = Time.time;
    }
}
