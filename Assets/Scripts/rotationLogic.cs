using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationLogic : MonoBehaviour
{
    [SerializeField] private stateManager sm;
    [SerializeField] private rotationManager rm;
    [HideInInspector] public Vector2 rotationVector;
    
    public void ResetRotationVector()
    {
        rm.ResetRotatorLocation();
        rotationVector = Vector2.zero;
    }

    void Update ()
    {
        rotationVector = rm.rotator.anchoredPosition / 95f;
    }
}
