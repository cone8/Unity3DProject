using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 10f;
    public float angularVelocity = 100f;
    Rigidbody playerRB;

    public float jumpHeight = 10f;
    public GameObject groundLevel;
    bool isInAir = false;

    public Transform firstPerson;
    Vector3 thirdPersonPos;
    Quaternion thirdPersonRot;
    bool isFirstPerson = false;

    public GameObject barrel;
    public GameObject bullet;
    public GameObject shoulder;

    public GameObject crosshair;
    public float cameraRotationSpeed;
    //public float minClamp;
    //public float maxClamp;

    public float fireRate;
    public float timeOfLastShot;
    Vector3 aimPoint;

    void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        thirdPersonPos = Camera.main.transform.localPosition;
        thirdPersonRot = Camera.main.transform.localRotation;

        Cursor.visible = false;
        crosshair.SetActive(true);
    }

    void FixedUpdate()
    {
        //move player
        playerRB.MovePosition(transform.position + transform.forward * Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime);

        //turn player
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, Input.GetAxis("Horizontal"), 0f) * Time.deltaTime * angularVelocity);
        playerRB.MoveRotation(rotation * transform.rotation);

        //jump
        Ray heightMeter = new Ray(groundLevel.transform.position, Vector3.down);
        RaycastHit groundHit;
        if (Physics.Raycast(heightMeter, out groundHit, 100))
        {
            if (Mathf.Abs(groundHit.point.y - groundLevel.transform.position.y) < 0.1f)
            {
                isInAir = false;
            }
            else
            {
                isInAir = true;
            }
        }

        if (Input.GetButtonDown("Jump") && isInAir == false)
        {
            playerRB.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }  
    }

    void Update()
    {
        //shoot
        if (Input.GetButtonDown("Fire1") && Time.time > timeOfLastShot + 0.2)
        {
            InvokeRepeating("Shoot", 0.001f, fireRate);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            CancelInvoke("Shoot");
        }


        //change camera
        if (Input.GetButtonDown("Fire2"))
        {
            if (isFirstPerson == false)
            {
                Camera.main.transform.position = firstPerson.position;
                Camera.main.transform.rotation = firstPerson.rotation;
                isFirstPerson = true;
            }
            else
            {
                Camera.main.transform.localPosition = thirdPersonPos;
                Camera.main.transform.localRotation = thirdPersonRot;
                isFirstPerson = false;
            }         
        }

        
        //rotate camera
        aimPoint = Camera.main.ScreenToWorldPoint(new Vector3(
                Mathf.Clamp(Input.mousePosition.x, Screen.width * 0.05f, Screen.width * 0.95f),
                Mathf.Clamp(Input.mousePosition.y, Screen.height * 0.4f, Screen.height * 0.8f),
                45f));

        shoulder.transform.LookAt(aimPoint);

        if (isFirstPerson)
        {
            //Vector3 minRotation = new Vector3(minClamp, 0f, 0f);
            //Vector3 maxRotation = new Vector3(maxClamp, 0f, 0f);

            if (Input.mousePosition.y >= Screen.height * 0.8f)
            {
                Camera.main.transform.Rotate(-10f * Time.deltaTime * cameraRotationSpeed, 0f, 0f, Space.Self);
            }
            if (Input.mousePosition.y <= Screen.height * 0.4f)
            {
                Camera.main.transform.Rotate(10f * Time.deltaTime * cameraRotationSpeed, 0f, 0f, Space.Self);
            }
            Debug.Log(Camera.main.transform.rotation.x);
        }
    }

    void Shoot()
    {
        Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
        timeOfLastShot = Time.time;
    }
}
