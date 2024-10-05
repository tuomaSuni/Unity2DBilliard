using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class songLogic : MonoBehaviour
{
    [SerializeField] private settingsLogic sl;
    private AudioSource audioSource;
    private float audioSourceVolume;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSourceVolume = audioSource.volume;

        sl.OnSongSet += SetSongVolume;
        SetSongVolume();
    }

    private void SetSongVolume()
    {
        audioSource.volume = PlayerPrefs.GetFloat("Song", 0.5f) * audioSourceVolume;
    }

    private void OnDestroy()
    {
        sl.OnSongSet -= SetSongVolume;
    }
}
