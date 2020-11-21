using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SetTrigger : MonoBehaviour
{
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void Trigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
        Debug.Log($"TRIGGERED: {triggerName} ON {gameObject.name}");
    }
}
