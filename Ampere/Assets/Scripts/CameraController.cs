using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform lookAtTarget;
    public float distance = 15f;
    //public float distanceMin = 8f;
    //public float distanceMax = 20f;
    public float distanceSmooth = 0.05f;
    public float mouseSensX = 5f;
    public float mouseSensY = 5f;
    public float smoothX = 0.05f;
    public float smoothY = 0.1f;
    public float limitMinY = -15f;
    public float limitMaxY = 10f;

    // aim limits
    public float limitMinX = -50f;
    public float limitMaxX = 50f;
    private float mouseAimX;

    private float mouseX = 0f;
    private float mouseY = 0f;
    private float velX = 0f;
    private float velY = 0f;
    private float velZ = 0f;
    private float velDistance = 0f;
    //private float startDistance = 0f;
    private Vector3 position = Vector3.zero;
    private Vector3 desiredPosition = Vector3.zero;
    private float desiredDistance = 0f;

    void Awake()
    {
        instance = this;
    }	

    void Start()
    {
        //distance = Mathf.Clamp(distance, distanceMin, distanceMax);
        //startDistance = distance;
        Reset();
    }

    void LateUpdate()
    {
        ProccessInput();

        CalculateDesiredPosition();

        UpdatePosition();
    }

    public void ProccessInput()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSensX;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensY;

        mouseY = MouseYClamp.ClampAngle(mouseY, limitMinY, limitMaxY);
    }

    void CalculateDesiredPosition()
    {
        distance = Mathf.SmoothDamp(distance, desiredDistance, ref velDistance, distanceSmooth);

        desiredPosition = CalculatePosition(mouseY, mouseX, distance);

        ////////\\\\\\\\ aim input
        mouseAimX = MouseYClamp.ClampAngle(mouseX, limitMinX, limitMaxX);
  
        PlayerController.instance.aimVector = 
            CalculatePosition(mouseY * 2.5f, mouseX * 0.8f, -PlayerController.instance.aimDistance);
    }

    public Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

        return lookAtTarget.position + rotation * direction;
    }

    void UpdatePosition()
    {
        float posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velX, smoothX);
        float posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velY, smoothY);
        float posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velZ, smoothX);
        position = new Vector3(posX, posY, posZ);

        transform.position = position;

        transform.LookAt(lookAtTarget);
    }

    private void Reset()
    {
        mouseX = 0f;
        mouseY = 10f;
        //distance = startDistance;
        desiredDistance = distance;
    }

    public static void UseOrCreateCamera()
    {
        GameObject tempCamera;
        GameObject targetLookAt;
        CameraController myCamera;

        if (Camera.main != null)
        {
            tempCamera = Camera.main.gameObject;
        }
        else
        {
            tempCamera = new GameObject("Main Camera");
            tempCamera.AddComponent<Camera>();
            tempCamera.tag = "MainCamera";
        }

        tempCamera.AddComponent<CameraController>();
        myCamera = tempCamera.GetComponent<CameraController>();

        targetLookAt = GameObject.Find("TargetLookAt") as GameObject;

        myCamera.lookAtTarget = targetLookAt.transform;
    }

    //public static void SetCrosshairCamera()
    //{
    //    GameObject crosshairCamera;
    //    GameObject targetLookAt;
    //    CameraController myCamera;

    //    crosshairCamera = GameObject.Find("Crosshair Camera");
    //    crosshairCamera.AddComponent<CameraController>();
    //    myCamera = crosshairCamera.GetComponent<CameraController>();

    //    targetLookAt = GameObject.Find("TargetLookAt") as GameObject;

    //    myCamera.lookAtTarget = targetLookAt.transform;
    //}
}
