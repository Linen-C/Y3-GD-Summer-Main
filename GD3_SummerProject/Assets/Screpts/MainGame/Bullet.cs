using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] string owner;
    [SerializeField] int damage;
    [SerializeField] int knockback;
    [SerializeField] int stanPower;

    [SerializeField] GameObject _plBullet; // 弾丸

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
        // 何でもない場合
        if (collision.tag == owner || collision.tag == gameObject.tag) { return; }


        // エネミー弾
        if (owner == "Enemy")
        {
            // 反射
            if (collision.tag == "PlayerAttack")
            {
                Instantiate(
                    _plBullet,
                    new Vector3
                    (transform.position.x,
                    transform.position.y,
                    transform.position.z),
                    collision.transform.rotation);

                Destroy(gameObject);
                return;
            }

            // プレイヤーに命中した時
            if (collision.tag == "Player")
            {
                // none
            }
        }


        // プレイヤー弾
        if (owner == "Player")
        {
            // プレイヤーの攻撃に反応しないように
            if (collision.tag == "PlayerAttack") { return; }

            // 敵に命中した時
            if (collision.tag == "Enemy")
            {
                collision.GetComponent<EnemyCTRL>().TakeDamage(
                    damage,
                    knockback,
                    stanPower,
                    5);
            }
        }


        Destroy(gameObject);
    }
}
