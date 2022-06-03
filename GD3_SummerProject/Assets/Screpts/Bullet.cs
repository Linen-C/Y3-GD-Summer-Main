using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] string owner;

    // コンポーネント
    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        body.velocity = new Vector2(
            transform.up.x * speed,
            transform.up.y * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == owner) return;
        Destroy(gameObject);
    }
}
