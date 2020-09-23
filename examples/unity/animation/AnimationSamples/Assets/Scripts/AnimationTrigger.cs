using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public Animator ourHappyAnimator;

    public bool IsInLoop;
    
    // Start is called before the first frame update
    void Start()
    {
        IsInLoop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (ourHappyAnimator == null)
        {
            Debug.Log("YOU FORGOT THE ANIMATOR YOU STUPID");
            return;
        }
        
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        
        
        if (IsInLoop)
        {
            ourHappyAnimator.SetTrigger("ExitAnimation");
            IsInLoop = false;
        }
        else
        {
            ourHappyAnimator.SetTrigger("EnterAnimation");
            IsInLoop = true;
        }
    }
}
