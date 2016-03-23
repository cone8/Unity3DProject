using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour
{

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);
        }
    }
}
