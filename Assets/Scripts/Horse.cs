using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class Horse : Animals
{
    //private Animator animator;
    bool isRiding, isWalking;
    private Transform player;
    public BoxCollider startLimit, endLimit;
    private float changeDirection = -1f;

    // Start is called before the first frame update
    void Start()
    {
        Speed = 10f;
    }

    private void Update()
    {
        if (isRiding) Ride();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) Mount(other);
        if (other.gameObject.CompareTag("Limit"))
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (other.transform.position.z == -2.5f)
            {
                transform.SetParent(null);
                StopWalkAnimation();
                player.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                player.position = Vector3.zero;
                isRiding = false;
                startLimit.enabled = false;
                changeDirection *= -1f;
            }
            if (other.transform.position.z == -25f)
            {                
                player.rotation = Quaternion.Euler(Vector3.zero);
                startLimit.enabled = true;
                endLimit.enabled = false;
                changeDirection *= -1f;
            }
        }
    }

    private void Mount(Collider other)
    {
        SetPlayerMountPosition(other);
        //Change face to opposite direction
        player.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        isRiding = true;
        endLimit.enabled = true;
    }

    private void SetPlayerMountPosition(Collider other)
    {
        StartWalkAnimation();
        player = other.transform;
        player.rotation = Quaternion.Euler(Vector3.zero);
        player.position = new Vector3(transform.position.x, 1.8f, transform.position.z);
        transform.SetParent(player.GetChild(0), true);
        other.GetComponent<UserControl>().isMount = true;
    }

    private void Ride()
    {
        player.GetComponent<Rigidbody>().AddForce(Vector3.forward * Time.deltaTime * Speed * changeDirection, ForceMode.Force);
    }

    //private void StopWalkAnimation()
    //{
    //    animator = GetComponent<Animator>();
    //    animator.enabled = isWalking;
    //}

    //private void StartWalkAnimation()
    //{
    //    animator = GetComponent<Animator>();
    //    animator.enabled = isWalking;
    //}
}
