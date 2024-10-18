using System.Collections;
using UnityEngine;

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
            if (value && !IsFirstJump)
            {
                StartCoroutine(DelayedJump(0.5f));
            }
        }
    }


    public int GetCurrentColorIndex()
    {
        return currentColorIndex;
    }

    void Awake()
    {
        // Get the renderer component to change the ball's material
        ballRenderer = GetComponentInChildren<Renderer>();
        _rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        SetPhysics(false);
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
}