using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIBuffBar : MonoBehaviour
{
    [SerializeField] bool isDebuffBar;
    [SerializeField] GameObject _buffPrefab;
    Player _player;

    private void Start()
    {
        GetComponent<GridLayoutGroup>()?.setCellSize(Orientation.Vertical, 1, GetComponent<RectTransform>());
    }

    private void Update()
    {
        if(_player == null)
            _player = GameManager.instance.getPlayer();

        displayBuff();
    }

    void displayBuff()
    {
        transform.clearChild();
        foreach (Buff buff in _player?.buffs)
        {
            if (buff.isDebuff == isDebuffBar)
            {
                // set the ui buff
                GameObject buffGameObject = Instantiate(_buffPrefab);
                UIBuff uiBuff = buffGameObject.GetComponent<UIBuff>();
                uiBuff.setBuff(buff);
                buffGameObject.transform.SetParent(transform);
                buffGameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
