using UnityEngine;
using System.Collections;
using TMPro;

public class UILevelUpDisplayer : MonoBehaviour
{
    [SerializeField] Animator levelAnimator;
    [SerializeField] Animator backgroundAnimation;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject levelDisplayer;

    private float hideTime = 0;
    private float levelDisplayTime = 0;

    void Update()
    {
        if (hideTime <= Time.time && levelDisplayer.activeSelf)
            gameObject.SetActive(false);

        if (levelDisplayTime <= Time.time)
            levelDisplayer.SetActive(true);
    }

    public void displayLevelUp(int level)
    {
        if (gameObject.activeSelf)
        {
            levelDisplayer.SetActive(false);
            gameObject.SetActive(false);
        }

        gameObject.SetActive(true);
        levelDisplayer.SetActive(false);
        levelText.text = level.ToString();

        // set animations timers
        float backgroundAnimationDuration = levelAnimator.GetCurrentAnimatorStateInfo(0).length + 0.3f;
        float levelAnimationDuration = backgroundAnimation.GetCurrentAnimatorStateInfo(0).length + 3f;

        levelDisplayTime = Time.time + backgroundAnimationDuration;
        hideTime = Time.time + backgroundAnimationDuration + levelAnimationDuration;
    }
}
