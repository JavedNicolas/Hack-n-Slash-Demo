using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraZOffset = 20;
    public GameObject playerController;
    private new Camera camera;

    // Start is called before the first frame update
    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        movement();
    }

    /// <summary>
    /// Move the camera with the player GameObject
    /// </summary>
    void movement()
    {
        Vector3 cameraPosition = camera.transform.position;
        Vector3 playerPosition = playerController.transform.position;
        camera.transform.position = new Vector3(playerPosition.x, cameraPosition.y, playerPosition.z - cameraZOffset);
        camera.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x, 0, 0);
    }
}
