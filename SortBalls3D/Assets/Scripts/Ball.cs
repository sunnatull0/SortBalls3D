using UnityEngine;

public class Ball : MonoBehaviour
{
    private Renderer ballRenderer;  // Renderer component for changing the ball's material
    private int currentColorIndex;  // To store the current color index (if needed for comparison)

    void Awake()
    {
        // Get the renderer component to change the ball's material
        ballRenderer = GetComponentInChildren<Renderer>();
    }

    // Method to set the ball's color (called from TubeManager)
    public void SetColor(Material colorMaterial, int colorIndex)
    {
        ballRenderer.material = colorMaterial;  // Set the ball's material
        currentColorIndex = colorIndex;         // Store the color index (useful for matching later)
    }

    // Optional: Method to get the current color index (could be useful for comparing colors later)
    public int GetCurrentColorIndex()
    {
        return currentColorIndex;
    }
}