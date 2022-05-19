using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDoAwake : MonoBehaviour
{
    public AreaCTRL area;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") { return; }
        if (area.DoClearFlag() == true) { return; }
        area.DoAwake();
    }
}
