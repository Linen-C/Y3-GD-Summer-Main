using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainAudioCTRL : MonoBehaviour
{
    [Header("クリップ")]
    [SerializeField]public AudioClip[] clips_BPM;
    [SerializeField]public AudioClip[] clips_Progress;
    [SerializeField]public AudioClip[] clips_Player_Gun;
    [SerializeField]public AudioClip[] clips_Player_Weapon;
    [SerializeField]public AudioClip[] clips_Damage;
    [Header("音量")]
    [Range(0,1)] public float nowVolume = 0.5f;
    [Header("アップデート先")]
    [SerializeField] GC_BpmCTRL _GC_BpmCTRL;

    void Awake()
    {
        // オプションから音量設定を持ってくる
        var location = Application.streamingAssetsPath + "/jsons/OptionSave.json";
        string inputJson = File.ReadAllText(location).ToString();
        var optionData = JsonUtility.FromJson<OptionData>(inputJson);

        nowVolume = optionData.SEvolume;
    }

    public void VolumeSet(float volume)
    {
        nowVolume = volume;
        _GC_BpmCTRL.VolumeUpdate(nowVolume);
    }
}
