using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleLogic : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        Bag(col);
        StartCoroutine(End(col));
    }

    void Bag(Collider2D col)
    {
        col.gameObject.transform.position = transform.position;
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        
        col.gameObject.transform.localScale *= 0.75f;
    }

    IEnumerator End(Collider2D col)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(col.gameObject);
    }
}
