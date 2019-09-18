using UnityEngine;
using System.Collections;

public class EnemyAndAggro
{
    public BeingBehavior beingBehavior;
    public float aggro;

    public EnemyAndAggro(BeingBehavior enemy, float aggro)
    {
        this.beingBehavior = enemy;
        this.aggro = aggro;
    }

    public void addAggro(float value)
    {
        aggro += value;
        Debug.Log(aggro + " : " + value);
    }
}

