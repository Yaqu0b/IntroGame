using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    // Define the DebugMode enum at the beginning of the class
    public enum DebugMode {
        Normal,
        Distance,
        Vision
    }

    public GameObject player;  // Reference to the player object
    public TextMeshProUGUI distanceText;  // Reference to the TextMeshProUGUI for displaying distance
    public TextMeshProUGUI positionText;  // Text for position (for Distance mode)
    public TextMeshProUGUI velocityText;  // Text for velocity (for Distance mode)

    private LineRenderer lineRenderer;  // LineRenderer to draw the line to the closest pickup
    private GameObject closestPickup;   // To store the closest pickup
    private DebugMode currentMode = DebugMode.Distance;  // Default mode is Distance
    private Vector3 playerVelocity;

    void Start() {
        // Initialize the distance text
        distanceText.text = "Distance to closest pickup: N/A";

        // Add the LineRenderer component to the GameController object
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set up the LineRenderer settings
        lineRenderer.startWidth = 0.1f;  // Line start width
        lineRenderer.endWidth = 0.1f;    // Line end width
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));  // Default material for the line
        lineRenderer.positionCount = 2;  // Two positions: start (player) and end (closest pickup)

        // Start in Distance mode
        UpdateDebugMode();
    }

    void Update() {
        // Check if the Space key is pressed to switch modes
        if (Input.GetKeyDown(KeyCode.Space)) {
            SwitchDebugMode();
        }

        // Update the debug behavior based on the current mode
        UpdateDebugMode();
    }

    private void SwitchDebugMode() {
        // Switch between Normal, Distance, and Vision modes
        currentMode = (DebugMode)(((int)currentMode + 1) % 3);
    }

    private void UpdateDebugMode() {
        switch (currentMode) {
            case DebugMode.Normal:
                DisableDebugInformation();
                break;

            case DebugMode.Distance:
                EnableDistanceDebug();
                break;

            case DebugMode.Vision:
                EnableVisionDebug();
                break;
        }
    }

    private void DisableDebugInformation() {
        // Turn off the line and hide debug information
        distanceText.enabled = false;
        positionText.enabled = false;
        velocityText.enabled = false;
        lineRenderer.enabled = false;

        // Reset all pickups to white
        ResetPickupsColor(Color.white);
    }

    private void EnableDistanceDebug() {
        // Enable the distance mode and show the line to the closest pickup
        distanceText.enabled = true;
        positionText.enabled = true;
        velocityText.enabled = true;

        DisplayClosestPickupDistance();
    }

    private void EnableVisionDebug() {
        // Vision mode: Show player's velocity as a line and highlight the pickup player is approaching
        distanceText.enabled = false;
        positionText.enabled = false;
        velocityText.enabled = true;

        // Draw a line indicating the player's velocity
        playerVelocity = player.GetComponent<Rigidbody>().velocity;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, player.transform.position);  // Start at the player's position
        lineRenderer.SetPosition(1, player.transform.position + playerVelocity);  // End at the velocity direction

        // Highlight the pickup the player is moving towards and stop its rotation
        HighlightPickupInDirection();
    }

    private void DisplayClosestPickupDistance() {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");

        float closestDistance = Mathf.Infinity;
        GameObject closestPickup = null;

        // Find the closest pickup
        foreach (GameObject pickup in pickups) {
            float distance = Vector3.Distance(player.transform.position, pickup.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestPickup = pickup;
            }

            // Reset the color of all pickups to white
            pickup.GetComponent<Renderer>().material.color = Color.white;
        }

        if (closestPickup != null) {
            // Highlight the closest pickup by changing its color to blue
            closestPickup.GetComponent<Renderer>().material.color = Color.blue;

            // Display the distance to the closest pickup
            distanceText.text = "Distance to closest pickup: " + closestDistance.ToString("F2") + " units";

            // Draw a line from the player to the closest pickup
            lineRenderer.SetPosition(0, player.transform.position);  // Start point (player)
            lineRenderer.SetPosition(1, closestPickup.transform.position);  // End point (closest pickup)
        } else {
            distanceText.text = "No more pickups!";
            lineRenderer.enabled = false;  // Hide the line if there are no pickups left
        }
    }

    private void HighlightPickupInDirection() {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");

        float maxDotProduct = -Mathf.Infinity;
        GameObject approachingPickup = null;

        // Find the pickup the player is moving towards most directly
        foreach (GameObject pickup in pickups) {
            Vector3 toPickup = (pickup.transform.position - player.transform.position).normalized;
            float dotProduct = Vector3.Dot(toPickup, playerVelocity.normalized);  // Measure direction alignment

            if (dotProduct > maxDotProduct) {
                maxDotProduct = dotProduct;
                approachingPickup = pickup;
            }

            // Reset the color of all pickups to white
            pickup.GetComponent<Renderer>().material.color = Color.white;

            // Ensure pickups continue rotating
            pickup.GetComponent<Rotator>().enabled = true;
        }

        // Highlight the pickup the player is moving towards most directly in green
        if (approachingPickup != null) {
            approachingPickup.GetComponent<Renderer>().material.color = Color.green;
            approachingPickup.GetComponent<Rotator>().enabled = false;  // Stop rotating

            // Orient the green pickup towards the player
            approachingPickup.transform.LookAt(player.transform.position);
        }
    }

    private void ResetPickupsColor(Color color) {
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");
        foreach (GameObject pickup in pickups) {
            pickup.GetComponent<Renderer>().material.color = color;
        }
    }
}
