using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballLogic : MonoBehaviour
{
    [SerializeField] protected stateManager sm;
    [SerializeField] private AudioClip[] audioclips;
    private dynamicSoundLogic dsl;
    private AudioSource ballimpact;
    private Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ballimpact = GetComponent<AudioSource>();
        dsl = GetComponent<dynamicSoundLogic>();
    }

    protected void Start()
    {
        sm.listOfBalls.Add(rb);
    }

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball") && !ballimpact.isPlaying)
        {
            ballimpact.clip = audioclips[Random.Range(0, 2)];
            ballimpact.volume = dsl.baseVolume * (rb.velocity.magnitude / 10);
            ballimpact.pitch = Random.Range(0.9f, 1.0f);
            ballimpact.Play();
        }
    }

    // Functions for debugging.

    protected virtual void OnDisable()
    {
        #if UNITY_EDITOR
        if (sm.listOfBalls.Contains(rb))
        sm.listOfBalls.Remove(rb);
        #endif
    }

    #if UNITY_EDITOR
    void OnDestroy()
    {
        if (sm.listOfBalls.Contains(rb))
        sm.listOfBalls.Remove(rb);
    }
    #endif
}
