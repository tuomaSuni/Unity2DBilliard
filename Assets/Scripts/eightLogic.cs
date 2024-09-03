using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eightLogic : MonoBehaviour
{
    [SerializeField] private stateManager sm;
    void OnDestroy()
    {
        sm.EndGame();
    }
}
