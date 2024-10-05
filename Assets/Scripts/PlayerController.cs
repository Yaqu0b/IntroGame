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

    void Start() {
        count = 0;
        winText.text = "";
        SetCountText();
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
}