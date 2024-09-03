using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    private AudioSource audiosource;
    
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        audiosource.Play();
    }
}
