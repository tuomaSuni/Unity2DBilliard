using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleLogic : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        Bag(col);
        StartCoroutine(End(col));
    }

    void Bag(Collision2D col)
    {
        col.gameObject.transform.position = transform.position;
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        
        col.gameObject.transform.localScale *= 0.75f;
    }

    IEnumerator End(Collision2D col)
    {
        GameObject currentDestroyable = col.gameObject;
        yield return new WaitForSeconds(2.0f);
        Destroy(currentDestroyable);
    }
}
