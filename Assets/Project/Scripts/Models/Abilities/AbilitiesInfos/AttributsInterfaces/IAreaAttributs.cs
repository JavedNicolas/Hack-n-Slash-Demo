using UnityEngine;
using System.Collections;

public interface IAreaAttributs
{
    GameObject areaPrefab { get; }
    float areaSize { get; }
    AreaType areaDamageType { get; }
    float degenDelay { get; }
}
