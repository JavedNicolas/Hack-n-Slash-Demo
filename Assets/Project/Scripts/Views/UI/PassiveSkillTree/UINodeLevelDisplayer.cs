using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UINodeLevelDisplayer : MonoBehaviour
{
    [SerializeField] int _levelPerLine;
    [SerializeField] public List<GameObject> _levelDisplayers = new List<GameObject>();
    [SerializeField] GameObject _levelDisplayPrefab;
 
    /// <summary>
    /// Init the level displayer : 
    /// Hide it if there is only one level
    /// esle init the number of level element 
    /// </summary>
    /// <param name="maxLevel">the max level</param>
    public void initDisplayer(int maxLevel)
    {
        _levelDisplayers = new List<GameObject>();
        transform.clearChild();
        if (maxLevel == 1)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        for(int i = 0; i < maxLevel; i++)
        {
            GameObject nodeLevelInstance = Instantiate(_levelDisplayPrefab);
            nodeLevelInstance.transform.SetParent(transform);
            _levelDisplayers.Add(nodeLevelInstance);
        }

        GetComponent<GridLayoutGroup>().setCellSize(Orientation.Horizontal, _levelPerLine, GetComponent<RectTransform>());
    }

    public void displayLockLevel(bool locked, int currentLevel = 0)
    {
        for (int i = 0; i < _levelDisplayers.Count; i++)
        {
            if(i < currentLevel)
                setStyle(NodeStyle.Allocated, _levelDisplayers[i]);
            else
                setStyle(locked ? NodeStyle.Locked : NodeStyle.Unallocated, _levelDisplayers[i]);
        }
            
    }

    /// <summary>
    /// update the display to fit the current level
    /// </summary>
    /// <param name="currentLevel">the current level</param>
    public void updateLevel(int currentLevel)
    {
        for(int i = 0; i < _levelDisplayers.Count; i++)
        {
            if (i < currentLevel)
                setStyle(NodeStyle.Allocated, _levelDisplayers[i]);
            else
                setStyle(NodeStyle.Unallocated, _levelDisplayers[i]);
        }
    }

    public void setStyle(NodeStyle nodeStyle, GameObject levelDisplayer)
    {
        switch (nodeStyle)
        {
            case NodeStyle.Allocated: levelDisplayer.GetComponent<Image>().color = Color.blue; break;
            case NodeStyle.Unallocated: levelDisplayer.GetComponent<Image>().color = Color.white; break;
            case NodeStyle.Locked: levelDisplayer.GetComponent<Image>().color = Color.gray; break;
        }
    }

}
