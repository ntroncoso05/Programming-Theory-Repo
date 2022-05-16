using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UserControl : MonoBehaviour
{
    public Camera GameCamera;

    private float timeToTarget, timeToReachTarget = 10f;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if (target != null)
        {
            timeToTarget += Time.deltaTime / timeToReachTarget;

            transform.position = Vector3.Lerp(transform.position, target.transform.position, timeToTarget);
            Debug.Log(Vector3.Lerp(transform.position, target.transform.position, timeToTarget));
        }
    }
}
