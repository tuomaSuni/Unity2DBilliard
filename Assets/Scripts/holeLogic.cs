using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holeLogic : MonoBehaviour
{
    [SerializeField] private stateManager sm;
    private AudioSource audiosource;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Bag(col);
        StartCoroutine(End(col));
        audiosource.Play();
    }

    private void Bag(Collision2D col)
    {
        col.gameObject.transform.position = transform.position;
        
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        sm.listOfBalls.Remove(rb);
        rb.velocity = Vector2.zero;

        col.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    private IEnumerator End(Collision2D col)
    {
        GameObject baggedBall = col.gameObject;

        yield return new WaitForSeconds(2.0f);
        while (sm.AllBallsHasStopped() == false)
        {
            yield return null;
        }
        
        baggedBall.SetActive(false);
    }
}
