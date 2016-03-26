using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static CharacterController charController;
    public static PlayerController instance;

    public GameObject crosshair;

    public GameObject barrel;
    public GameObject bullet;
    public GameObject shoulder;

    public float fireRate;
    public Vector3 aimVector;
    public float aimDistance = 40f;

    private float timeOfLastShot;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
        instance = this;
        CameraController.UseOrCreateCamera();
        //CameraController.SetCrosshairCamera();

        Cursor.visible = false;
        //crosshair.SetActive(true);
    }

    void Update()
    {
        GetMovementInput();

        PlayerMover.instance.UpdateMovement();

        Aim();

        GetGunInput();
    }

    void GetMovementInput()
    {
        PlayerMover.instance.moveVector = Vector3.zero;

        PlayerMover.instance.moveVector += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    void Aim()
    {
        //aimVector = transform.InverseTransformVector(aimVector);
        //aimVector.y = Mathf.Clamp(aimVector.y, -50f, 50f);
        shoulder.transform.LookAt(aimVector);
    }

    void GetGunInput()
    {
        if (Input.GetButton("Fire1") && Time.time > timeOfLastShot + 0.2)
        {
            StartCoroutine("ShootCR");
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine("ShootCR");
        }
    }

    IEnumerator ShootCR()
    {
        Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
        timeOfLastShot = Time.time;

        yield return new WaitForSeconds(fireRate);
    }
}
