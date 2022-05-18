using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCTRL : MonoBehaviour
{
    [SerializeField] GameObject pinMax;
    [SerializeField] GameObject pinMin;
    [SerializeField] new CameraCTRL camera;

    Vector2 pinMaxPos;
    Vector2 pinMinPos;

    void Start()
    {
        pinMaxPos = pinMax.transform.position;
        pinMinPos = pinMin.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        camera.SetNewCamPoint(pinMaxPos, pinMinPos);
    }

}
