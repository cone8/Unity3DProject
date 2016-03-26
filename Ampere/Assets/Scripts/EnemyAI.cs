using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public GameObject barrel;
    public GameObject bullet;

    Rigidbody enemyRB;
    //SphereCollider visionRadius;
    public float visionAngle = 120f;
    public float turnSpeed;
    public float speed;

    void Awake()
    {
        enemyRB = GetComponent<Rigidbody>();
        //visionRadius = GetComponent<SphereCollider>();
    }

    void Update()
    {
        Vector3 target = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        float angle = Vector3.Angle(other.transform.position - transform.position, transform.forward);
        if (other.tag == "Player" && angle < visionAngle / 2)
        {
            //transform.forward = Vector3.Slerp(transform.forward, other.transform.position, turnSpeed * Time.deltaTime);
            //transform.rotation = new Quaternion(0f, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            enemyRB.MoveRotation(Quaternion.LookRotation(other.transform.position));
            Invoke("Shoot", 1f);
            
            enemyRB.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CancelInvoke("Shoot");
        }
    }

    void Shoot()
    {
        Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
    }
}
