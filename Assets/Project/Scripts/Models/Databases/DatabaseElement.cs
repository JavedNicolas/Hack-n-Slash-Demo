using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CustomAttributs : System.Attribute
{
    public string name;
}

[CustomAttributs(name = "")]
public class DatabaseElement 
{
    public string name;
}
