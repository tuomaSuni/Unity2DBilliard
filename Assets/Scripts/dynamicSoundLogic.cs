using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class dynamicSoundLogic : MonoBehaviour
{
    [SerializeField] private settingsLogic sl;
    private AudioSource audioSource;
    private float audioSourceVolume;
    [HideInInspector] public float baseVolume;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSourceVolume = audioSource.volume;

        sl.OnSoundSet += SetBaseVolume;
        SetBaseVolume();
    }

    private void SetBaseVolume()
    {
        baseVolume = PlayerPrefs.GetFloat("Sound", 0.5f) * audioSourceVolume;
    }

    private void OnDestroy()
    {
        sl.OnSoundSet -= SetBaseVolume;
    }
}
