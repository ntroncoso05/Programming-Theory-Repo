using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UserControl : MonoBehaviour
{
    public Camera GameCamera;
    public bool isMount = false;

    private GameObject target;
    private Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var ray = GameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Animal")) target = hit.collider.gameObject;
                else target = null;
            }
        }
        if (target != null && target.GetComponent<Horse>())
        {
            target = null;
            //create the rotation we need to be in to look at the target
            transform.LookAt(new Vector3(-4f, 0f, -4f));

            //find the vector pointing from our position to the target
            Vector3 _direction = (new Vector3(-4f, 0f, -4f) - transform.position).normalized;

            playerRigidbody.AddForce(_direction * 2f, ForceMode.Impulse);
        }
        if(isMount)
        {
            playerRigidbody.velocity = Vector3.zero;
            isMount = false;
        }
    }
}
