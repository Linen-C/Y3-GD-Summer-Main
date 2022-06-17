using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraShake : MonoBehaviour
{
    float amplitude = 0.0f;
    int frame = 0;
    Vector3 pos;

    private void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        if (Keyboard.current.iKey.IsPressed()){
            amplitude = 1.0f;
        }

        if (amplitude > 0.0f){
            float x = (float)((frame % 3) - 1) * amplitude;
            Quaternion q = Quaternion.identity;
            Vector3 p = pos;
            p.x = x;
            transform.SetPositionAndRotation(p, q);
        }

        amplitude *= 0.9f;

        if (amplitude < 0.001f)
        {
            amplitude = 0.0f;
        }
    }
}
