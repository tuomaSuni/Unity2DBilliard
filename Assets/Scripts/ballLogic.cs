using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool hasStopped = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (rb.velocity.magnitude < 0.01f) hasStopped = true;
        else hasStopped = false;
    }
}
