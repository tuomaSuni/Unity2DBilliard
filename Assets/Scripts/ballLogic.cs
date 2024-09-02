using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballLogic : MonoBehaviour
{
    [SerializeField] private stateManager sm;
    
    void Awake()
    {
        sm.listOfBalls.Add(GetComponent<Rigidbody2D>());
    }
    
    void OnDestroy()
    {
        sm.listOfBalls.Remove(GetComponent<Rigidbody2D>());
    }
}
