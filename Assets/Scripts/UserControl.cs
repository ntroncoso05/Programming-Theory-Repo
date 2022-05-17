using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UserControl : MonoBehaviour
{
    //Private Fields
    private const float xRandomRange = 35f, yRandomRange = 0f, zLowerRandomRange = 10f, zUpperRandomRange = 45f;
    private Camera GameCamera;
    private GameObject target;
    private Rigidbody playerRigidbody;

    //Public Fields
    public bool isMount = false;
    public GameObject toyToFetch;
    public GameObject[] foods;
    public bool isPlayerAvailable = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        GameCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isPlayerAvailable)
        {
            isPlayerAvailable = false;
            MakeAnimalEatSomething();
        }
        if (Input.GetMouseButtonDown(1) && isPlayerAvailable)
        {
            //isPlayerAvailable = false;
            PlayWithAnimal();
        }
        if (target != null && target.GetComponent<Horse>() && isPlayerAvailable)
        {
            isPlayerAvailable = false;
            HorseRiding();
        }
        if (target != null && target.GetComponent<Dog>() && isPlayerAvailable)
        {
            isPlayerAvailable = false;
            PlayFetch();
        }

        if (isMount)
        {
            playerRigidbody.velocity = Vector3.zero;
            isMount = false;
        }

    }

    /// <summary>
    /// Play selected animal
    /// </summary>
    private void PlayWithAnimal()
    {
        Ray ray = GameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Animal")) target = hit.collider.gameObject;
            else target = null;
        }
    }

    /// <summary>
    /// Ride the horse
    /// </summary>
    private void HorseRiding()
    {
        Vector3 targetPosition = target.GetComponent<Horse>().StartingPosition;
        target = null;
        //create the rotation we need to be in to look at the target
        transform.LookAt(targetPosition);

        //find the vector pointing from our position to the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        playerRigidbody.AddForce(direction * 2f, ForceMode.Impulse);
        var animator = GetComponent<Animator>();
        animator.SetBool("Static_b", false);
        animator.SetFloat("Speed_f", 0.3f);
    }

    /// <summary>
    /// Play Fetch with Dog
    /// </summary>
    private void PlayFetch()
    {
        toyToFetch.SetActive(true);
        toyToFetch.transform.Translate(RandomPosition());
        target.transform.LookAt(toyToFetch.transform);
        target.GetComponent<Dog>().Fetch(toyToFetch.transform.position);
        target = null;
    }

    /// <summary>
    /// place food for animal to eat
    /// </summary>
    private void MakeAnimalEatSomething()
    {
        Ray ray = GameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Animal"))
            {
                hit.collider.gameObject.GetComponent<Animals>().IsSmelling = true;
                foreach (GameObject item in foods)
                {
                    item.SetActive(true);
                    item.transform.position = RandomPosition();
                }
            }
        }
    }

    /// <summary>
    /// Creates random position for x, y , z
    /// </summary>
    /// <returns> a random Vector3 values</returns>
    private Vector3 RandomPosition()
    {
        return new Vector3(
            Random.Range(-xRandomRange, xRandomRange),
            yRandomRange,
            Random.Range(-zLowerRandomRange, -zUpperRandomRange)
            );
    }
}
