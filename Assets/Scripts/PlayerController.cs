using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {

    public Vector2 moveValue;
    public float speed;
    private int count;
    private int numPickups = 3; // Put here the number of pickups you have
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI velocityText;
    private Vector3 lastPosition; // Declare lastPosition at the class level
    private Vector3 velocity;     // Declare velocity at the class level

    void Start() {
        count = 0;
        winText.text = "";
        SetCountText();
        Debug.Log("Pickup count: " + count);

        // Initialize last position to the starting position of the player
        lastPosition = transform.position;
    }

    void OnMove(InputValue value) {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate() {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);

        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);

        // Calculate the velocity as the difference between current and last position
        velocity = (transform.position - lastPosition) / Time.fixedDeltaTime;

        // Update the last position to the current position for the next frame
        lastPosition = transform.position;

        // Display player's position and velocity on the GUI
        DisplayPlayerStatus();
    }

    // This method is called when the player collides with an object
    void OnTriggerEnter(Collider other) {
        // Check if the object is tagged as "Pickup"
        if(other.gameObject.tag == "PickUp") {
            other.gameObject.SetActive(false);
            // Increment the count by 1 when a pickup is collected
            count++;
            SetCountText();
            Debug.Log("Pickup collected! Total count: " + count);
        }
    }

    private void SetCountText () {
        scoreText.text = "Score: " + count.ToString();
        if(count >= numPickups) {
            winText.text = "You win!";
        }
    }

    private void DisplayPlayerStatus() {
        // Display player's position
        positionText.text = "Position: " + transform.position.ToString("F2");

        // Display player's velocity (vector)
        velocityText.text = "Velocity: " + velocity.ToString("F2");

        // Display the scalar speed (magnitude of velocity)
        float speedMagnitude = velocity.magnitude;
        velocityText.text += "\nSpeed: " + speedMagnitude.ToString("F2") + " units/s";
    }
}