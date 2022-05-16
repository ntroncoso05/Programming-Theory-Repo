using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    private Animator animator;
    private const float minSpeed = 1f;
    private const float maxSpeed = 10f;
    //ENCAPSULATION
    private float _speed;
    public virtual float Speed
    {
        get { return _speed; }
        set 
        {
            if (value >= minSpeed && value <= maxSpeed) _speed = value;
            else
            {
                _speed = maxSpeed;
                Debug.LogWarning($"Speed is set between {minSpeed} and {maxSpeed} units");
            }
        } 
    }

    //// Start is called before the first frame update
    //void Start()
    //{
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
    
    public void StopWalkAnimation()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void StartWalkAnimation()
    {
        animator = GetComponent<Animator>();
        animator.enabled = true;
    }
}
