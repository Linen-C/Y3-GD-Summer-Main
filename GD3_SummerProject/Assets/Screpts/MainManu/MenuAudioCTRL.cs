using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MenuAudioCTRL : MonoBehaviour
{
    [Header("クリップ")]
    [SerializeField] public AudioClip[] _clips;

    [Header("音量")]
    [Range(0, 1)] public float nowVolume = 0.5f;

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
    }
}
