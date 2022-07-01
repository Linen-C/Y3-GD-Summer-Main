using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public void Move(float knockBackCounter, int doStanCount, Rigidbody2D body, Vector2 diff, float moveSpeed)
    {
        if (knockBackCounter <= 0.0f)
        {
            if (doStanCount <= 0)
            {
                body.velocity = new Vector2(diff.x * moveSpeed, diff.y * moveSpeed);
            }
            else
            {
                body.velocity = (new Vector2(0, 0));
            }
        }
    }

    public float KnockBack(float knockBackCounter, Rigidbody2D body, Vector2 diff,float knockBackPower)
    {
        if (knockBackCounter > 0.0f)
        {
            body.AddForce(new Vector2(
                -diff.x * knockBackPower,
                -diff.y * knockBackPower),
                ForceMode2D.Impulse);

            knockBackCounter -= Time.deltaTime;
        }

        return knockBackCounter;
    }
}
