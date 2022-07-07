using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCTRL : MonoBehaviour
{
    [Header("ピン位置(マニュアル)")]
    [SerializeField] GameObject pinMax;
    [SerializeField] GameObject pinMin;
    [SerializeField] bool downToUp;
    [SerializeField] bool setCenter;
    [Header("カメラ(オート)")]
    [SerializeField] new CameraCTRL camera;

    Vector2 pinMaxPos;
    Vector2 pinMinPos;

    void Start()
    {
        GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
        camera = camObj.GetComponent<CameraCTRL>();
        pinMaxPos = pinMax.transform.position;
        pinMinPos = pinMin.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        if (setCenter)
        {
            camera.SetNewCenter(transform.position, pinMinPos, pinMaxPos);
            return;
        }

        camera.SetNewCamPoint(pinMaxPos, pinMinPos, downToUp);
    }

}
