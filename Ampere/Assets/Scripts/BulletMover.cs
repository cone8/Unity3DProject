using UnityEngine;
using System.Collections;

public class BulletMover : MonoBehaviour
{
    public float bulletSpeed;
    Rigidbody bulletRB;

	void Start()
    {
        bulletRB = GetComponent<Rigidbody>();
        bulletRB.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
    }
}
