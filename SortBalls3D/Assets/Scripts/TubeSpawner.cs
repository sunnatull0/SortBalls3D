using System.Collections.Generic;
using UnityEngine;

public class TubeSpawner : MonoBehaviour
{
    public GameObject tubePrefab; // The tube prefab to instantiate
    public GameObject ballPrefab; // The ball prefab to instantiate
    public Transform holder; // The central Holder GameObject for positioning tubes
    public Transform topPosition; // Global position 1 meter above all tubes (used later for falling ball logic)
    public int numberOfTubes = 3; // Number of tubes to spawn (T)
    public int ballsPerTube = 3; // Number of balls per tube (B)
    public Material[] availableColors; // Array of available colors (C)
    public float tubeRadius = 5f; // The radius of the circular arrangement of tubes

    private List<int> colorIndices = new List<int>(); // To track color indices
    private List<GameObject> tubes = new List<GameObject>(); // To track tube instances

    void Start()
    {
        if (availableColors.Length < numberOfTubes)
        {
            Debug.LogError("Number of colors must be greater than or equal to the number of tubes.");
            return;
        }

        InitializeColorList();
        SpawnTubes();
        SpawnBalls(); // Spawn all balls inside the tubes

        // Wait for all the tweens to complete before moving the bottom-most ball
        Invoke("SpawnBottomBallFromClosestTube", .6f); // Adjust delay based on the tween duration if necessary
    }

    // Method to initialize the color list with the proper distribution
    void InitializeColorList()
    {
        colorIndices.Clear();
        int totalBalls = numberOfTubes * ballsPerTube;

        // Use only the first T colors if C > T
        for (int i = 0; i < numberOfTubes; i++) // Limit to the first T colors
        {
            for (int j = 0; j < ballsPerTube; j++)
            {
                colorIndices.Add(i); // Add color indices for all balls in each tube
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
    
    void CheckLevelCompletion()
    {
        foreach (GameObject tube in tubes)
        {
            TubeManager tubeManager = tube.GetComponent<TubeManager>();
            if (!tubeManager.IsTubeSorted())
            {
                Debug.Log("Level not completed yet.");
                return;
            }
        }

        Debug.Log("Level completed! All tubes are sorted.");
        Time.timeScale = 0;
    }

    // Method to spawn tubes in a circular arrangement
    void SpawnTubes()
    {
        float angleStep = 360f / numberOfTubes; // Calculate the angle difference between tubes
        float yPosition = holder.position.y; // Set the y-axis based on the Holder's Y position or adjust as needed

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
            
            TubeManager tubeManager = newTube.GetComponent<TubeManager>();
            if (tubeManager != null)
            {
                // Subscribe to the event so we check level completion whenever a ball is added
                tubeManager.OnBallAdded += CheckLevelCompletion;
            }
        }
    }

    // Calculate tube positions in a circular formation
    Vector3 CalculatePositionOnCircle(float angleDegrees, float radius, float yPosition)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRadians) * radius;
        float z = Mathf.Sin(angleRadians) * radius;
        return new Vector3(x, yPosition, z); // Use the provided yPosition for vertical control
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
                int colorIndex = colorIndices[0];
                tubeManager.SpawnBall(ballPrefab, j, colorIndex, availableColors[colorIndex]);
                colorIndices.RemoveAt(0); // Remove the assigned color from the list
            }
        }
        
        CheckLevelCompletion(); 
    }

    // Find the closest tube to the camera/player and remove the bottom-most ball
    void SpawnBottomBallFromClosestTube()
    {
        if (tubes.Count > 0)
        {
            float minDistance = float.MaxValue;
            List<GameObject> closestTubes = new List<GameObject>();

            // Loop through each tube to find the closest one
            foreach (GameObject tube in tubes)
            {
                float distance = Vector3.Distance(tube.transform.position, Camera.main.transform.position);

                // If a new closer tube is found, update the minimum distance and clear the list
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTubes.Clear();
                    closestTubes.Add(tube);
                }
                // If a tube has the same distance, add it to the list
                else if (Mathf.Approximately(distance, minDistance))
                {
                    closestTubes.Add(tube);
                }
            }

            // Pick a random tube from the closest tubes if multiple are found
            GameObject chosenTube = closestTubes[Random.Range(0, closestTubes.Count)];

            // Get the TubeManager of the chosen tube and move the bottom-most ball to the TopPosition
            TubeManager tubeManager = chosenTube.GetComponent<TubeManager>();
            if (tubeManager != null)
            {
                tubeManager.MoveBottomBallToTop(topPosition); // Move the bottom ball of the closest tube to the top position
                tubeManager.AdjustRemainingBalls();
            }
        }
    }
}
