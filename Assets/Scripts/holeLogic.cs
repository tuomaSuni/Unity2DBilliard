using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loleLogic : MonoBehaviour
{
    private AudioSource audiosource;
    [SerializeField] private stateManager sm;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Bag(col);
        StartCoroutine(End(col));
        audiosource.Play();
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
        while (sm.AllBallsHasStopped() == false)
        {
            yield return null;
        }
        Destroy(currentDestroyable);
    }
}
