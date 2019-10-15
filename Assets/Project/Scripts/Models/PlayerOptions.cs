using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Player Option")]
public class PlayerOptions : ScriptableObject
{
    [SerializeField] string _lang;


    public string lang => _lang;
}
