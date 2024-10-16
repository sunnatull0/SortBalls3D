using System.Collections.Generic;
using UnityEngine;

public class TubeSpawner : MonoBehaviour
{
    public GameObject tubePrefab;  // The tube prefab to instantiate
    public GameObject ballPrefab;  // The ball prefab to instantiate
    public Transform holder;       // The central Holder GameObject for positioning tubes
    public Transform topPosition;  // Global position 1 meter above all tubes (used later for falling ball logic)
    public int numberOfTubes = 3;  // Number of tubes to spawn
    public int ballsPerTube = 3;   // Number of balls per tube
    public Material[] availableColors;  // Array of available colors
    public float tubeRadius = 5f;  // The radius of the circular arrangement of tubes

    private List<int> colorIndices = new List<int>();  // To track color indices
    private List<GameObject> tubes = new List<GameObject>();  // To track tube instances

    void Start()
    {
        InitializeColorList();
        SpawnTubes();
        SpawnBalls();  // Spawn all balls inside the tubes

        // Wait for all the tweens to complete before moving the bottom-most ball
        Invoke("SpawnBottomBallFromClosestTube", 1f);  // Adjust delay based on the tween duration if necessary
    }

    // Method to initialize the color list with the proper distribution
    void InitializeColorList()
    {
        colorIndices.Clear();
        int totalBalls = numberOfTubes * ballsPerTube;
        int maxPerColor = Mathf.CeilToInt((float)totalBalls / availableColors.Length);  // Round up to ensure we cover all balls

        // Add each color index, ensuring we have enough colors for all balls
        for (int i = 0; i < availableColors.Length; i++)
        {
            for (int j = 0; j < maxPerColor; j++)
            {
                if (colorIndices.Count < totalBalls)  // Ensure we don't add more colors than necessary
                {
                    colorIndices.Add(i);
                }
            }
        }

        // Shuffle the color list for randomness
        ShuffleColorList();
    }

    // Method to shuffle the color list
    void ShuffleColorList()
    {
        for (int i = 0; i < colorIndices.Count; i++)
        {
            int randomIndex = Random.Range(0, colorIndices.Count);
            int temp = colorIndices[i];
            colorIndices[i] = colorIndices[randomIndex];
            colorIndices[randomIndex] = temp;
        }
    }

    // Method to spawn tubes in a circular arrangement
    void SpawnTubes()
    {
        float angleStep = 360f / numberOfTubes;  // Calculate the angle difference between tubes
        float yPosition = holder.position.y;     // Set the y-axis based on the Holder's Y position or adjust as needed

        for (int i = 0; i < numberOfTubes; i++)
        {
            // Calculate the angle for each tube
            float angle = i * angleStep;
        
            // Calculate the tube position around the holder
            Vector3 tubePosition = CalculatePositionOnCircle(angle, tubeRadius, yPosition);

            // Instantiate the tube at the calculated position
            GameObject newTube = Instantiate(tubePrefab, tubePosition, Quaternion.identity, holder);

            // Add the tube to the list of tubes
            tubes.Add(newTube);
        }
    }

    // Calculate tube positions in a circular formation
    Vector3 CalculatePositionOnCircle(float angleDegrees, float radius, float yPosition)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRadians) * radius;
        float z = Mathf.Sin(angleRadians) * radius;
        return new Vector3(x, yPosition, z);  // Use the provided yPosition for vertical control
    }

    // Method to spawn balls into each tube (all balls spawn inside the tubes at the start)
    void SpawnBalls()
    {
        foreach (GameObject tube in tubes)
        {
            TubeManager tubeManager = tube.GetComponent<TubeManager>();
            if (tubeManager == null)
            {
                Debug.LogError("TubeManager not found on the tube prefab!");
                continue;
            }

            // Spawn the given number of balls per tube
            for (int j = 0; j < ballsPerTube; j++)
            {
                tubeManager.SpawnBall(ballPrefab, j, colorIndices[0], availableColors[colorIndices[0]]);
                colorIndices.RemoveAt(0);  // Remove the assigned color from the list
            }
        }
    }

    // Find the closest tube to the camera/player and remove the bottom-most ball
    void SpawnBottomBallFromClosestTube()
    {
        if (tubes.Count > 0)
        {
            // Find the closest tube (for simplicity, let's assume the first tube is the closest for now)
            TubeManager closestTube = tubes[0].GetComponent<TubeManager>();  // Adjust this logic later if necessary

            // Smoothly move the bottom-most ball to the TopPosition
            closestTube.MoveBottomBallToTop(topPosition);
        }
    }
}
