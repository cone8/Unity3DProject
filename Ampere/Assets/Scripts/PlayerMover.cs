using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour
{
    public static PlayerMover instance;

    public float moveSpeed = 5f;

    public Vector3 moveVector;

    Rigidbody playerRB;

    public float jumpForce = 10f;
    public float jumpHeight = 5f;

    void Awake()
    {
        instance = this;

        playerRB = GetComponent<Rigidbody>();
    }   

    public void UpdateMovement()
    {
        AlignWithCamera();
        ProccessMovement();
        CheckHeightOverFloor();
    }

    void ProccessMovement()
    {
        moveVector = transform.TransformDirection(moveVector);

        if (moveVector.magnitude > 1)
        {
            moveVector = Vector3.Normalize(moveVector);
        }

        moveVector *= moveSpeed * Time.deltaTime;

        PlayerController.charController.Move(moveVector);
    }

    void AlignWithCamera()
    {
        if (moveVector.x != 0 || moveVector.z != 0)
        {
            transform.rotation = Quaternion.Euler(
                transform.eulerAngles.x, 
                Camera.main.transform.eulerAngles.y, 
                transform.eulerAngles.z
            );
        }
    }

    //this is the jump function
    void OnCollisionStay(Collision collision)
    {
        PlayerController.charController.enabled = true;

        if (Input.GetButtonDown("Jump"))
        {
            PlayerController.charController.enabled = false;
            if (PlayerController.charController.enabled == false)
            {
                float jumpMultiplier = 1f;
                if (Mathf.Abs(playerRB.velocity.z) > 0)
                {
                    jumpMultiplier = 1.5f;
                }
                playerRB.AddForce(Vector3.up * jumpForce * jumpMultiplier, ForceMode.Impulse);
            }
        }
    }

    void CheckHeightOverFloor()
    {
        Ray heightMeter = new Ray(transform.position, Vector3.down);
        RaycastHit groundHit;
        if (Physics.Raycast(heightMeter, out groundHit, 100))
        {
            if (groundHit.distance > 0.36f)
            {
                PlayerController.charController.enabled = false;
                if (PlayerController.charController.enabled == false &&
                    playerRB.velocity.z == 0f &&
                    Input.GetAxis("Vertical") > 0f)
                {
                    playerRB.AddForce(transform.forward * 3f, ForceMode.Impulse);
                }
            }

            if (groundHit.distance > jumpHeight)
            {
                playerRB.AddForce(Vector3.down * 40f, ForceMode.Force);
            }
            else
            {
                playerRB.AddForce(Vector3.down * 20f, ForceMode.Force);
            }
            
            //Debug.Log("distance " + (groundHit.distance));
        }
    }
}
