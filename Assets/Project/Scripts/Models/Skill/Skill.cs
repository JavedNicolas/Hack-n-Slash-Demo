using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    abstract public void animation();
    abstract public void effect(Being target, float aspd);
}
