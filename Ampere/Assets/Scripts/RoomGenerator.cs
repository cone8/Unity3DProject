using UnityEngine;
using System.Collections;

public class RoomGenerator : MonoBehaviour {

    public GameObject backWall;
    public GameObject frontWall;
    public GameObject leftWall;
    public GameObject rightWall;

    public GameObject floor;

    public float roomMaxSize;
    public float roomMinSize = 20;
    float roomWidthX;
    float roomLengthZ;

    void BuildWalls()
    {
        Instantiate(backWall, new Vector3(0f, 3.9f, roomLengthZ / 2), Quaternion.LookRotation(Vector3.back));
        Instantiate(frontWall, new Vector3(0f, 3.9f, -roomLengthZ / 2), Quaternion.LookRotation(Vector3.forward));
        Instantiate(leftWall, new Vector3(-roomWidthX / 2, 3.9f, 0f), Quaternion.LookRotation(Vector3.right));
        Instantiate(leftWall, new Vector3(roomWidthX / 2, 3.9f, 0f), Quaternion.LookRotation(Vector3.left));

    }

    void Awake()
    {
        roomMaxSize = floor.transform.localScale.x;
        roomWidthX = Random.RandomRange(roomMinSize, roomMaxSize);
        roomLengthZ = Random.RandomRange(roomMinSize, roomMaxSize);

        BuildWalls();
    }
}
