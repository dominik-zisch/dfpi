using System;
using UnityEngine;

public class SetAnimationSpeedOnCollision : MonoBehaviour
{
    // REFERENCE (PLACEHOLDER) TO OUR ANIMATOR COMPONENT
    public Animator animator;

    // NAME OF THE ANIMATION WE WANT TO TRIGGER
    public string animationSpeedParameterName;

    [Range(0, 5)]
    public float animationSpeedWhenInsideCollider = 1;

    [Range(0,5)]
    public float defaultAnimationSpeed = 0;

    void Start()
    {
        animator.SetFloat(animationSpeedParameterName, defaultAnimationSpeed);
    }

    // WHEN A COLLIDER ENTERS OUR ZONE
    void OnTriggerEnter()
    {
        animator.SetFloat(animationSpeedParameterName, animationSpeedWhenInsideCollider);
    }

    // WHEN A COLLIDER EXITS OUR ZONE
    void OnTriggerExit()
    {
        animator.SetFloat(animationSpeedParameterName, defaultAnimationSpeed);
    }
}
