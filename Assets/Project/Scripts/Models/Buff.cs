using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Buff
{
    [SerializeField] public float startingTime;
    [SerializeField] public float duration;
    [SerializeField] public string name;
    [SerializeField] public List<Stat> stats = new List<Stat>();

    public Buff(string name, float duration,List<Stat> stats)
    {
        this.duration = duration;
        this.name = name;
        this.stats = stats;
    }
}
