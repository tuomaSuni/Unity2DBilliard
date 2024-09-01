using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockLogi : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float pushForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - rb.position).normalized;
            rb.velocity = direction * pushForce;
        }
    }
}