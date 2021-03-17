using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///挂载在箱子上，给 GameController 提供修改 Sprite 的方法 ：箱子在目标点上时， Sprite 改变
/// </summary>
public class Box : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite onTargetSprite;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSpriteToNormal()
    {
        spriteRenderer.sprite = normalSprite;
    }

    public void SetSpriteToOnTargetSprite()
    {
        spriteRenderer.sprite = onTargetSprite;
    }
}
