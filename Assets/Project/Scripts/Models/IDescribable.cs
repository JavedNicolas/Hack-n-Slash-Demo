using UnityEngine;
using System.Collections;

public interface IDescribable 
{
    string getName();
    string getDescription(Being owner);
    string getSmallDescription(Being owner);
}
