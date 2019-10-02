using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIBuff : MonoBehaviour
{
    [SerializeField] Buff _buff;
    public Buff buff => _buff;

    [SerializeField] Image _buffDuration;
    [SerializeField] Image _buffIcon;

    public void setBuff(Buff buff)
    {
        this._buff = buff;
        _buffIcon.sprite = buff.icon;
        updateBuff();
    }

    void updateBuff()
    {
        if(_buff.duration != -1)
        {
            float buffTimeElapsed = Time.time - _buff.startingTime;
            _buffDuration.fillAmount = -(buffTimeElapsed / _buff.duration) + 1;
        }
    }

}
