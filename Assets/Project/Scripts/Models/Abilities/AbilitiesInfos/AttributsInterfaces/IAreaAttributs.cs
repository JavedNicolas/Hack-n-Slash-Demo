using UnityEngine;
using System.Collections;

public interface IAreaAttributs
{
    GameObject areaPrefab { get; }
    AreaType areaDamageType { get; }
    float degenDelay { get; }
}
