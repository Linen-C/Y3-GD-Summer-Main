using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCTRL : MonoBehaviour
{
    [Header("ピン位置(マニュアル)")]
    [SerializeField] GameObject pinMax;
    [SerializeField] GameObject pinMin;
    [Header("カメラ(オート)")]
    [SerializeField] new CameraCTRL camera;
    [Header("使い捨てかどうか")] 
    [SerializeField] bool _orInstant;

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
        camera.SetNewCamPoint(pinMaxPos, pinMinPos);
        if (_orInstant) { Destroy(gameObject); }
    }

}
