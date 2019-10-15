using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Localization : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    [SerializeField] string _key;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        DontDestroyOnLoad(this);
    }

    // CHANGE LATER
    private void Update()
    {
        textMesh.text = LocalizationManager.instance.getText(_key);
    }

}
