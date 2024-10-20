using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private Renderer ballRenderer; // Renderer component for changing the ball's material
    private int currentColorIndex; // To store the current color index (if needed for comparison)

    public float jumpForce = 5f; // Force applied to make the ball jump
    private Rigidbody _rb; // Reference to the Rigidbody component

    public bool isFalling;
    private static bool IsFirstJump = true;

    private bool _isMainBall; // Bool to track if this ball is the main ball

    public bool IsMainBall
    {
        get => _isMainBall;
        set
        {
            _isMainBall = value;

            // Enable outline for the main ball and disable for others
            SetOutlineActive(value);

            // Start delayed jump for the main ball
            if (value && !IsFirstJump)
            {
                StartCoroutine(DelayedJump(0.5f));
            }
        }
    }

    private Outline outline; // Reference to the Outline script
    
    [SerializeField] private bool _addTorque = true;
    [SerializeField] private float _torqueStrength = 0.5f;

    void Awake()
    {
        // Get the renderer component to change the ball's material
        ballRenderer = GetComponentInChildren<Renderer>();
        _rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        outline = GetComponent<Outline>(); // Get the Outline script

        SetPhysics(false);
    }

    private void Start()
    {
        // Disable the outline for all balls at the start
        SetOutlineActive(false);
        IsFirstJump = true;
    }

    void Update()
    {
        // Check if the ball is the main ball and should jump
        if (IsMainBall && Input.touchCount > 0 && !isFalling && IsFirstJump)
        {
            Jump();
            IsFirstJump = false;
        }
    }

    // Method to enable or disable the outline
    private void SetOutlineActive(bool isActive)
    {
        if (outline != null)
        {
            outline.enabled = isActive;
        }
    }

    // Method to set the ball's color (called from TubeManager)
    public void SetColor(Material colorMaterial, int colorIndex)
    {
        ballRenderer.material = colorMaterial; // Set the ball's material
        currentColorIndex = colorIndex; // Store the color index (useful for matching later)
    }

    

    // Method to make the ball jump by enabling physics and adding force
    void Jump()
    {
        isFalling = true;

        // Enable physics and apply a force upwards to simulate a jump
        SetPhysics(true);
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (!_addTorque) return;
        // Apply random torque to add rotation
        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f), // Random rotation around X-axis
            Random.Range(-1f, 1f), // Random rotation around Y-axis
            Random.Range(-1f, 1f) // Random rotation around Z-axis
        ) * _torqueStrength; // Adjust torque strength

        _rb.AddTorque(randomTorque, ForceMode.Impulse);
    }


    public void SetPhysics(bool value)
    {
        _rb.isKinematic = !value;
    }

    private IEnumerator DelayedJump(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the delay
        Jump(); // Execute the jump
    }

    public int GetCurrentColorIndex()
    {
        return currentColorIndex;
    }
}