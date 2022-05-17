using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class Horse : Animals
{
    //Private Fields
    private readonly float mountHeight = 1.8f;
    private readonly Vector3 playerFaceAnimals = new Vector3(0f, 180f, 0f);

    /// <summary>
    /// Set the player mount position and animation
    /// </summary>
    /// <param name="other">The animal collider to ride</param>
    private void SetPlayerMountPosition(Collider other)
    {
        StartWalkAnimation();
        Player.rotation = Quaternion.Euler(Vector3.zero);
        Player.position = new Vector3(transform.position.x, mountHeight, transform.position.z);
        transform.SetParent(Player.GetChild(0), true);
        other.GetComponent<UserControl>().isMount = true;
    }

    /// <summary>
    /// Mount the player
    /// </summary>
    /// <param name="other">The animal collider to mount</param>
    public void Mount(Collider other)
    {
        other.GetComponent<Animator>().SetFloat("Speed_f", 0.20f);
        SetPlayerMountPosition(other);
        //Change face to opposite direction
        Player.rotation = Quaternion.Euler(playerFaceAnimals);
        isRiding = true;
        EndLimit.enabled = true;
        Ride(EndLimit.transform.position);
    }

    /// <summary>
    /// Unmount the player
    /// </summary>
    /// <param name="other">The animal collider to unmount</param>
    public void UnMount()
    {
        transform.SetParent(null);
        transform.GetComponent<Rigidbody>().isKinematic = false;

        Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Player.rotation = Quaternion.Euler(playerFaceAnimals);
        Player.position = Vector3.zero;
        isRiding = false;
    }

    /// <summary>
    /// Base method for riding an animal and giving control to the player (Simulated)
    /// </summary>
    /// <param name="target">Destination of ride</param>
    protected override void Ride(Vector3 target)
    {
        transform.GetComponent<Rigidbody>().isKinematic = true;
        Vector3 direction = (target - transform.position).normalized;
        Player.GetComponent<Rigidbody>().AddForce(direction * Speed, ForceMode.Impulse);
    }
}
