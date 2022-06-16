using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Vector2 MoveTypeA(Vector2 diff,float speed)
    {
        Vector2 velocity = new Vector2(diff.x * speed, diff.y * speed);
        return velocity;
    }
}
