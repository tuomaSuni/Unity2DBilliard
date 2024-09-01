using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockLogi : MonoBehaviour
{
    private Rigidbody2D rb;

    // Force multiplier
    public float pushForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse position in world coordinates
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Calculate the direction from the rock to the mouse position
            Vector2 direction = (mousePosition - rb.position).normalized;

            // Apply force in that direction
            rb.velocity = direction * pushForce;
        }
    }
}