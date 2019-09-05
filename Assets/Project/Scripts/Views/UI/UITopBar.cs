using UnityEngine;
using TMPro;
using System.Collections;

public class UITopBar : MonoBehaviour
{
    [Header("UI Content")]
    [SerializeField] UIExpBar _expBar;
    [SerializeField] TextMeshProUGUI _levelDisplayerText;

    Player _player;
    public void init(Player player)
    {
        _player = player;
        InvokeRepeating("updateBar", 0.0f, 0.1f);
    }

    private void updateBar()
    {
        float expPercentage = _player.currentLevelExp / LevelExperienceTable.levelExperienceNeeded[_player.currentLevel - 1];

        _expBar.updateExpDisplay(expPercentage);
        _levelDisplayerText.text = _player.currentLevel.ToString();
    }
}

