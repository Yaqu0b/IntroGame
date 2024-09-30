using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public Vector2 moveValue;
    public float speed;
    private int count;

    void Start() {
        count = 0;
        Debug.Log("Pickup count: " + count);
    }

    void OnMove(InputValue value) {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate() {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);

        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime) ;
    }

    // This method is called when the player collides with an object
    void OnTriggerEnter(Collider other) {
        // Check if the object is tagged as "Pickup"
        if(other.gameObject.tag == "PickUp") {
            // Increment the count by 1 when a pickup is collected
            count += 1;
            Debug.Log("Pickup collected! Total count: " + count);
            other.gameObject.SetActive(false);
        }
    }
}