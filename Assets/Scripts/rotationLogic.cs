using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationLogic : MonoBehaviour
{
    [SerializeField] private rotationManager rm;
    public Vector2 rotationVector;
    public void ResetRotation()
    {
        rm.ResetRotationAmount();
    }

    void Update ()
    {
        rotationVector = rm.rotator.anchoredPosition / 10f;
    }
}
