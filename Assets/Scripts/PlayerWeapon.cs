using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    Player player;
    Animator animator;
    public SpriteRenderer sr;

    

    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        sr = GetComponent<SpriteRenderer>();

        animator.SetTrigger("Ready Up");
    }

    void AnimationEnd() //Called at the end of swipe clips
    {
        if (CurrentAnimationClip() == "Reverse Swipe Animation")
        {
            animator.SetTrigger("Ready Up");
            player.AnimationEnd();
        }
        else if (CurrentAnimationClip() == "Swipe Animation")
        {
            animator.SetTrigger("Ready Down");
            player.AnimationEnd();
        }
        
      
    }

   

    public string CurrentAnimationClip()
    {
        AnimatorClipInfo[] currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (currentClipInfo.Length == 0)
            return "";
        return currentClipInfo[0].clip.name;
    }


}
