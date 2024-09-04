using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballLogic : MonoBehaviour
{
    [SerializeField] protected stateManager sm;
    private AudioSource ballimpact;
    [SerializeField] private AudioClip[] audioclips;
    private Rigidbody2D rb;

    private void Awake()
    {
        sm.listOfBalls.Add(GetComponent<Rigidbody2D>());
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballimpact = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball") && !ballimpact.isPlaying)
        {
            ballimpact.clip = audioclips[Random.Range(0, 2)];
            ballimpact.volume = rb.velocity.magnitude / 10;
            ballimpact.pitch = Random.Range(0.8f, 0.9f);
            ballimpact.Play();
        }
    }

    protected virtual void OnDestroy()
    {
        #if UNITY_EDITOR // Function for Debugging.
        if (sm.listOfBalls.Contains(rb))
        sm.listOfBalls.Remove(rb);
        #endif
    }
}
