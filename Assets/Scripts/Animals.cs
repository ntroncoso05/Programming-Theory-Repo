using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class Animals : MonoBehaviour
{
    //Private and constant Fields
    private const float minSpeed = 1f, maxSpeed = 10f, smellRadius = 100f, timeToEat = 3.5f, horseSpeed = 6.5f, dogSpeed = 4f;
    private Animator animator;

    private float _speed;
    //ENCAPSULATION
    protected virtual float Speed
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
    protected BoxCollider StartLimit { get; private set; }
    protected BoxCollider EndLimit { get; private set; }
    protected Transform Player { get; set; }
    protected bool isRiding { get; set; }

    //Public Fields
    public Vector3 StartingPosition { get; private set; }
    public bool IsSmelling { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SetInitialParameters();
    }

    private void SetInitialParameters()
    {
        StartLimit = GameObject.Find("Start Limit").GetComponent<BoxCollider>();
        EndLimit = GameObject.Find("End Limit").GetComponent<BoxCollider>();
        StartingPosition = transform.position;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (this is Horse) Speed = horseSpeed;
        if (this is Dog) Speed = dogSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSmelling)
        {
            SmellFood(transform.position, smellRadius);
            IsSmelling = false;
        }
    }

    /// <summary>
    /// Stop the walk animation of the animal
    /// </summary>
    protected void StopWalkAnimation()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    /// <summary>
    /// Start the walk animation of the animal
    /// </summary>
    protected void StartWalkAnimation()
    {
        animator = GetComponent<Animator>();
        animator.enabled = true;
    }

    /// <summary>
    /// Base method for giving smell ability to an animal 
    /// </summary>
    /// <param name="center">The position of the animal</param>
    /// <param name="radius">Distance that it can smell</param>
    protected void SmellFood(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Food"))
            {
                if (hitCollider.name == "Carrot" && this is Horse)
                {
                    Walk(hitCollider.transform.position);
                }
                if (hitCollider.name == "Steak" && this is Dog)
                {
                    Walk(hitCollider.transform.position);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Only checking for horse because is the only using the Ride() method
        //POLYMORPHISM animal class can be cast to other derived types
        var horse = (this is Horse) ? (Horse)this : null;

        //ABSTRACTION (abstract out methods)
        if (other.gameObject.CompareTag("Player") && horse) horse.Mount(other);
        if (other.gameObject.CompareTag("Limit"))
        {
            if (other.transform.position.z == StartLimit.transform.position.z)
                ManageStartingPointLimit(horse);

            if (other.transform.position.z == EndLimit.transform.position.z)
                ManageEndingPointLimit();
        }

        if (other.gameObject.name == "Carrot")
        {
            StartCoroutine(Eat());
        }
        if (other.gameObject.name == "Steak")
        {
            StartCoroutine(Eat());
        }
        if (other.gameObject.name == "Bone" && this is Dog)
        {
            //POLYMORPHISM animal class can be cast to other derived types
            Dog dog = (Dog)this;
            dog.GrabToy();
            Walk(StartingPosition);
            StartLimit.enabled = true;
        }
    }

    /// <summary>
    /// Make decision to what do next after reaching the start limit
    /// </summary>
    /// <param name="horse">horse reference for animal class</param>
    private void ManageStartingPointLimit(Horse horse)
    {
        if (isRiding) horse.UnMount();

        StartLimit.enabled = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = StartingPosition;
        StopWalkAnimation();
        if (horse == null)
        {
            Dog dog = (Dog)this;
            dog.DropToy();
        }
        Player.GetComponent<UserControl>().isPlayerAvailable = true;
    }

    /// <summary>
    /// Make decision to what do next after reaching the end limit
    /// </summary>
    /// <param name="other"> </param>
    private void ManageEndingPointLimit()
    {
        Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Player.rotation = Quaternion.Euler(Vector3.zero);
        StartLimit.enabled = true;
        EndLimit.enabled = false;
        Ride(StartingPosition);
    }

    /// <summary>
    /// Eat the food
    /// </summary>
    /// <returns></returns>
    private IEnumerator Eat()
    {
        GetComponent<Animator>().SetBool("Eat_b", true);
        GetComponent<Animator>().SetFloat("Speed_f", 0f);
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(timeToEat);
        HideFoods();
    }

    /// <summary>
    /// Hide the food
    /// </summary>
    private void HideFoods()
    {
        GetComponent<Animator>().SetBool("Eat_b", false);
        GetComponent<Animator>().SetFloat("Speed_f", 0.5f);
        Walk(StartingPosition);
        StartLimit.enabled = true;
        foreach (GameObject food in Player.GetComponent<UserControl>().foods)
        {
            food.SetActive(false);
        }
    }

    /// <summary>
    /// Base method for walking an animal 
    /// </summary>
    /// <param name="target">Destination of walk</param>
    protected void Walk(Vector3 target)
    {
        transform.LookAt(target);
        StartWalkAnimation();
        Vector3 direction = (target - transform.position).normalized;

        transform.GetComponent<Rigidbody>().AddForce(direction * Speed, ForceMode.Impulse);
    }

    // POLYMORPHISM and ABSTRACTION
    /// <summary>
    /// Base method for riding an animal and giving control to the player
    /// </summary>
    /// <param name="target">Destination of ride</param>
    protected virtual void Ride(Vector3 target) { }
}
