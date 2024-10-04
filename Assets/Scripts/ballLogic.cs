using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballLogic : MonoBehaviour
{
    [SerializeField] protected stateManager sm;
    [SerializeField] private settingsLogic sl;
    [SerializeField] private AudioClip[] audioclips;
    private AudioSource ballimpact;
    private Rigidbody2D rb;
    private float baseVolume = 0.0f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ballimpact = GetComponent<AudioSource>();
    }

    protected void Start()
    {
        sm.listOfBalls.Add(rb);

        sl.OnSoundSet += UpdateVolume;
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        baseVolume = PlayerPrefs.GetFloat("Sound");
    }

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball") && !ballimpact.isPlaying)
        {
            ballimpact.clip = audioclips[Random.Range(0, 2)];
            ballimpact.volume = baseVolume * rb.velocity.magnitude / 10;
            ballimpact.pitch = Random.Range(0.9f, 1.0f);
            ballimpact.Play();
        }
    }

    protected virtual void OnDisable()
    {
        #if UNITY_EDITOR
        if (sm.listOfBalls.Contains(rb))
        sm.listOfBalls.Remove(rb);
        #endif
    }

    void OnDestroy()
    {
        sl.OnSoundSet -= UpdateVolume;

        #if UNITY_EDITOR
        if (sm.listOfBalls.Contains(rb))
        sm.listOfBalls.Remove(rb);
        #endif
    }
}
