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

    [SerializeField] GameObject _plBullet; // �e��

    // �R���|�[�l���g
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
        // ���ł��Ȃ��ꍇ
        if (collision.tag == owner || collision.tag == gameObject.tag) { return; }


        // �G�l�~�[�e
        if (owner == "Enemy")
        {
            // ����
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

            // �v���C���[�ɖ���������
            if (collision.tag == "Player")
            {
                // none
            }
        }


        // �v���C���[�e
        if (owner == "Player")
        {
            // �v���C���[�̍U���ɔ������Ȃ��悤��
            if (collision.tag == "PlayerAttack") { return; }

            // �G�ɖ���������
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
