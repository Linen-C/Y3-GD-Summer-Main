using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCTRL : MonoBehaviour
{
    [Header("クリップ")]
    [SerializeField]public AudioClip[] clips_BPM;
    [SerializeField]public AudioClip[] clips_Progress;
    [SerializeField]public AudioClip[] clips_Player_Gun;
    [SerializeField]public AudioClip[] clips_Player_Weapon;
    [SerializeField]public AudioClip[] clips_Damage;
    [Header("音量")]
    [Range(0,1)] public float defVolume = 0.5f;

    void Awake()
    {
        // オプションから音量設定を持ってくる
    }
}
