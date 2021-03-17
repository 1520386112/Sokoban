using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

/// <summary>
/// 挂载在 player 上，主要用于控制动画的播放
/// </summary>
public class Player : MonoBehaviour
{
    private Animator animator;

    public string anim_down = "WalkDown";
    public string anim_left = "WalkLeft";
    public string anim_right = "WalkRight";
    public string anim_up = "WalkUp";

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 根据 GameController 传入的输入方向来播放对应动画
    /// </summary>
    /// <param name="dir"></param>
    public void PlayPlayerAnimation(Direction dir)
    {
        switch (dir)
        {
            case Direction.DOWN:
                animator.Play(anim_down);
                break;
            case Direction.UP:
                animator.Play(anim_up);
                break;
            case Direction.LEFT:
                animator.Play(anim_left);
                break;
            case Direction.RIGHT:
                animator.Play(anim_right);
                break;
        }
    }
}
