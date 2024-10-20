using System;
using UnityEngine;

public class TubesHolder : MonoBehaviour
{
    public float rotationSpeed = 5f; // Adjust this for desired sensitivity
    private bool canRotate = true;

    private void Start()
    {
        TubeSpawner.OnLevelComplete += DisableRotation;
    }

    private void OnDestroy()
    {
        TubeSpawner.OnLevelComplete -= DisableRotation;
    }

    private void DisableRotation()
    {
        canRotate = false;
    }

    void Update()
    {
        // Only rotate if allowed (when the ball is not entering a tube)
        if (canRotate && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                // Rotate around the Y-axis based on touch movement
                float rotationAmount = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
                transform.Rotate(0, -rotationAmount, 0);
            }
        }
    }

    // Method to enable/disable rotation
    public void SetCanRotate(bool value)
    {
        canRotate = value;
    }
}