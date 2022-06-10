using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] string owner;
    [SerializeField] int damage;
    [SerializeField] int knockback;

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

        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyCTRL>().TakeDamage(damage, knockback);
        }

        if (collision.tag == "Player")
        {
            // none
        }

        Destroy(gameObject);
    }
}
