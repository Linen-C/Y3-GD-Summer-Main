using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    // コンポーネント
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        // どうしてかこの方式だと上手くいかない
        // spriteRenderer = GetComponent<SpriteRenderer>();
        //Debug.Log("SpCh：" + spriteRenderer);
    }

    
    public void ChangeSprite(Sprite changeTarget,float offset)
    {
        transform.localPosition = new Vector3(0, offset, 0);
        spriteRenderer.sprite = changeTarget;
    }

    public void ChangeTransparency(float alpha)
    {
        spriteRenderer.color = new Color(1, 1, 1, alpha);
    }
}
