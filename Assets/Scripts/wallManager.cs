using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallManager : MonoBehaviour
{
    private AudioSource audiosource;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        audiosource.volume = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude / 100;
        audiosource.Play();
    }
}
