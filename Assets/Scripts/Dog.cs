using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class Dog : Animals
{
    /// <summary>
    /// Start to fetch for the object
    /// </summary>
    /// <param name="target">The object to fetch</param>
    public void Fetch(Vector3 target)
    {
        Walk(target);
    }

    /// <summary>
    /// Grab the object
    /// </summary>
    public void GrabToy()
    {
        GameObject toy = Player.GetComponent<UserControl>().toyToFetch;
        toy.transform.SetParent(transform, false);
        toy.transform.localPosition = new Vector3(0f, 0.12f, 0.14f);
        toy.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        toy.GetComponent<BoxCollider>().enabled = false;
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    /// <summary>
    /// Drop the object
    /// </summary>
    public void DropToy()
    {
        GameObject toy = Player.GetComponent<UserControl>().toyToFetch;
        toy.GetComponent<BoxCollider>().enabled = true;
        toy.transform.localScale = new Vector3(2f, 2f, 2f);
        toy.transform.SetParent(null, false);
        toy.SetActive(false);
    }
}
