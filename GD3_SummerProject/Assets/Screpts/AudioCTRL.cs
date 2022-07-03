using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCTRL : MonoBehaviour
{
    [Header("ƒNƒŠƒbƒv")]
    [SerializeField]public AudioClip[] clips;
    [Header("‰¹—Ê")]
    [Range(0,1)] public float defVolume = 0.5f;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = defVolume;
    }
}
