using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "ArcanaAttributs", menuName = ScriptableObjectConstant.abilityAttributsMenuName + "Arcana")]
public class ArcaneAreaAttributs : AbilityAttributs, IAreaAttributs
{
    [SerializeField] private GameObject _areaPrefab;
    [SerializeField] private float _areaSize;
    [SerializeField] private AreaType _areaType;
    [SerializeField] private float _degenDelay;

    public GameObject areaPrefab => _areaPrefab;
    public float areaSize => _areaSize;
    public AreaType areaDamageType => _areaType;
    public float degenDelay => _degenDelay;
}
