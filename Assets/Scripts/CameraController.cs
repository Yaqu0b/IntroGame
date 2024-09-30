using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Start () {
        offset = transform.position;
    }

    void LateUpdate () {
        transform.position = player.transform.position + offset;

        // Lock the camera's rotation to a specific value:
        transform.rotation = Quaternion.Euler(45, 0, 0);
    }
}
