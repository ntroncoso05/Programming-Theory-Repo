using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    private const float minSpeed = 1f;
    private const float maxSpeed = 5f;

    private float _speed;
    public virtual float Speed
    {
        get { return _speed; }
        set 
        {
            if (value <= maxSpeed && value >= minSpeed) _speed = value;
            else
            {
                _speed = maxSpeed;
                Debug.LogWarning($"Speed is set between {minSpeed} and {maxSpeed} units");
            }
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if(Input.GetMouseButtonDown(0)) transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        //Walk();
    }
    bool s = false;
    private void Walk()
    {
        //if(!s) transform.Rotate(transform.position + new Vector3(0f, 180f, 0f));
        Animator animator = GetComponent<Animator>();
        animator.enabled = true;
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        //s = true;
    }
}
