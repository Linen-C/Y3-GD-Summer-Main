using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class TestScrept : MonoBehaviour
{
    void Start()
    {
        // ファイルパスを指定(Assetsから始まるっぽい)
        //string filePath = Application.dataPath + @"\Screpts\File\test.txt";
        string filePath = Application.dataPath + @"\Files\test.txt";

        // データストリームで読み出し
        StreamReader sr = new StreamReader(filePath, Encoding.UTF8);

        // 中身がなくなるまで繰り返す
        while (!sr.EndOfStream)
        {
            // デバッグ出力ぅ
            Debug.Log("読み込み内容：" + sr.ReadLine());
        }
    }
}
