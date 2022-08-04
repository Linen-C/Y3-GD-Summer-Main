using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyCTRL;

public class EnemyMove : MonoBehaviour
{
    [Header("ˆÚ“®")]
    [SerializeField] float _moveSpeed;  // ˆÚ“®‘¬“x

    public void Move(float knockBackCounter, int doStanCount, Rigidbody2D body, Vector2 diff)
    {
        if (knockBackCounter <= 0.0f)
        {
            if (doStanCount <= 0)
            {
                body.velocity = new Vector2(diff.x * _moveSpeed, diff.y * _moveSpeed);
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


    public bool CanMove(State state, int nowCharge, int needCharge, Rigidbody2D body)
    {
        if (state == State.Dead ||
            state == State.Stop ||
            needCharge == -1 ||
            nowCharge >= (needCharge - 1))
        {
            body.velocity = new Vector2(0, 0);
            return false;
        }
        return true;
    }
}
