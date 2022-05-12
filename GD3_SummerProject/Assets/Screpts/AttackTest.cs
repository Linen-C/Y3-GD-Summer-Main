using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTest : MonoBehaviour
{
    // 変数
    public float defTime;

    // コンポーネント
    Rigidbody2D body;
    BoxCollider2D coll;

    // プライベート変数
    float time = 0.0f;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        coll.enabled = false;
    }

    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Space) && (time < 0))
        {
            coll.enabled = true;
            time = defTime;
            Debug.Log("起動");
        }
        */

        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            coll.enabled = false;
        }

        /*
        if (Input.GetMouseButtonDown(1))
        {
            transform.localPosition = new Vector3(0.0f, 3.0f, 0.0f);
            transform.localScale = new Vector3(1.0f, 3.0f, 1.0f);
        }
        */
    }

    public void Attacking()
    {
        coll.enabled = true;
        time = defTime;
        Debug.Log("判定発生");
    }

}
