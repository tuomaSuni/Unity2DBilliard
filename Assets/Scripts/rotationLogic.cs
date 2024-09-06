using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationLogic : MonoBehaviour
{
    [SerializeField] private stateManager sm;
    [SerializeField] private rotationManager rm;
    [HideInInspector] public Vector2 rotationVector;
    
    public void ResetRotation()
    {
        rm.ResetRotationAmount();
        rotationVector = Vector2.zero;
    }

    void Update ()
    {
        if (sm.isSettingRotation) rotationVector = rm.rotator.anchoredPosition;
    }
}
