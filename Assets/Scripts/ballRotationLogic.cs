using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballRotationLogic : MonoBehaviour
{
    [SerializeField] private rotationLogic rl;

    void Start()
    {
        Debug.Log(rl.rotationVector);
    }
}
