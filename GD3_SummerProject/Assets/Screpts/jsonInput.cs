using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jsonInput : MonoBehaviour
{
    [System.Serializable]
    public class InputData
    {
        public string name;
        public int cool;
        public int height;
        public int wideth;
    }
}
