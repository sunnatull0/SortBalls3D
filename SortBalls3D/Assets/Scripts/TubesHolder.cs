using System;
using UnityEngine;

public class TubesHolder : MonoBehaviour
{
    public float rotationSpeed = 5f; // Base rotation speed
    private bool canRotate = true;
    private float sensitivityMultiplier = 1f; // Sensitivity adjustment based on screen DPI

    private void Start()
    {
        TubeSpawner.OnLevelComplete += DisableRotation;

        // Adjust sensitivity based on DPI
        float dpi = Screen.dpi;

        // If DPI is zero (sometimes happens on PC), use a default value
        if (dpi == 0)
        {
            dpi = 160; // Assume a default DPI
        }

        // Sensitivity adjustment based on screen DPI
        sensitivityMultiplier = dpi / 160f;
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
                // Rotate around the Y-axis based on touch movement, time, and sensitivity
                float rotationAmount = touch.deltaPosition.x * rotationSpeed * sensitivityMultiplier * Time.deltaTime;
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