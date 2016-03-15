using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    float h;
    float v;
    public float speed;
    Rigidbody playerRB;

    void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
    }

	void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        playerRB.velocity = new Vector3(h * speed, 0f, v * speed);
    }
}
