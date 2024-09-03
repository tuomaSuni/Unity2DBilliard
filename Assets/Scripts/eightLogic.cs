using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eightLogic : MonoBehaviour
{
    [SerializeField] private stateManager sm;
    void OnDestroy()
    {
        if (transform.parent.transform.childCount == 1) sm.EndGame(true); else sm.EndGame(false);
    }
}
