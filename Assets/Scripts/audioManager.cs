using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioclips;
    [SerializeField] private AudioSource audiosource;
    
    void Start()
    {
        audiosource.clip = audioclips[Random.Range(0, 2)];
        audiosource.Play();
    }
}
