using UnityEngine;
using System.Collections;
[System.Serializable]
[CreateAssetMenu(menuName = ScriptableObjectConstant.abilityAttributsMenuName + "Goliath", fileName = "GoliathAttributs")]
public class GoliathAbilityAttributs : AbilityAttributs, ICoolDownAttributs, IDurationAttributs
{
    [SerializeField] float _coolDown;
    [SerializeField] float _duration;

    public float duration => _duration;
    public float coolDown => _coolDown;

}
