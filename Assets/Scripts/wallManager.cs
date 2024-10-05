using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallManager : MonoBehaviour
{
    private AudioSource audiosource;
    private dynamicSoundLogic dsl;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        dsl = GetComponent<dynamicSoundLogic>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        audiosource.volume = dsl.baseVolume * (col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude / 100);
        audiosource.Play();
    }
}
