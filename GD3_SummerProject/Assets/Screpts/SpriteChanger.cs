using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    //public Sprite changeTarget;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    
    public void ChangeSprite(Sprite changeTarget,float offset)
    {
        Debug.Log("SPチェンジャー" + changeTarget.ToString());
        transform.localPosition = new Vector3(0, offset, 0);
        spriteRenderer.sprite = changeTarget;
    }

    public void ChangeTransparency(int alpha)
    {
        spriteRenderer.color = new Color(255, 255, 255, alpha);
    }
}
