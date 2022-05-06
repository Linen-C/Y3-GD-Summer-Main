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
        time = defTime;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && (time < 0))
        {
            coll.enabled = true;
            time = defTime;
            Debug.Log("起動");
        }

        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            coll.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
