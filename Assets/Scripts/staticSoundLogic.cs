using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class staticSoundLogic : MonoBehaviour
{
    [SerializeField] private settingsLogic sl;
    private AudioSource audioSource;
    private float audioSourceVolume;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSourceVolume = audioSource.volume;

        sl.OnSoundSet += SetSoundVolume;
        SetSoundVolume();
    }

    private void SetSoundVolume()
    {
        audioSource.volume = PlayerPrefs.GetFloat("Sound", 0.5f) * audioSourceVolume;
    }

    private void OnDestroy()
    {
        sl.OnSoundSet -= SetSoundVolume;
    }
}
