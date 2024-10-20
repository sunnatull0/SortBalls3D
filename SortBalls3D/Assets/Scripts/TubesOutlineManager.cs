using System.Collections;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public Transform ballPosition; // This is the transform where the main ball is located (the position to monitor)
    private TubeManager[] allTubes;
    private TubeManager currentHighlightedTube;

    void Start()
    {
        // We can delay the initialization to ensure the tubes are spawned before we try to find them
        StartCoroutine(InitializeAfterTubesSpawn());
    }

    IEnumerator InitializeAfterTubesSpawn()
    {
        // Wait for a frame to ensure all tubes are spawned (can adjust this delay if necessary)
        yield return new WaitForSeconds(0.1f); // Adjust this if needed depending on your tube spawning timing

        // Find all tubes in the scene after they are spawned
        allTubes = FindObjectsOfType<TubeManager>();

        // Disable outlines for all tubes at the start
        DisableAllOutlines();
    }

    void Update()
    {
        // If the tubes haven't been found yet, don't proceed
        if (allTubes == null || allTubes.Length == 0) return;

        // Continuously check which tube is closest to the ball's position
        TubeManager closestTube = FindClosestTube(ballPosition.position);

        // Highlight the closest tube
        if (closestTube != null)
        {
            HighlightClosestTube(closestTube);
        }
    }

    // Find the closest tube to the given position
    private TubeManager FindClosestTube(Vector3 position)
    {
        TubeManager closestTube = null;
        float minDistance = Mathf.Infinity;

        foreach (TubeManager tube in allTubes)
        {
            // Calculate only the horizontal distance (X and Z axis) between the position and the tube
            float distance = Vector3.Distance(new Vector3(tube.transform.position.x, 0, tube.transform.position.z),
                                              new Vector3(position.x, 0, position.z));

            if (distance < minDistance)
            {
                minDistance = distance;
                closestTube = tube;
            }
        }

        return closestTube;
    }

    // Highlight the closest tube and remove the outline from the previous one
    private void HighlightClosestTube(TubeManager closestTube)
    {
        if (currentHighlightedTube != null && currentHighlightedTube != closestTube)
        {
            // Remove outline from the previously highlighted tube
            currentHighlightedTube.GetComponent<Outline>().enabled = false;
        }

        // Apply outline to the new closest tube if it's not already highlighted
        if (closestTube != currentHighlightedTube)
        {
            closestTube.GetComponent<Outline>().enabled = true;
            currentHighlightedTube = closestTube;
        }
    }

    // Disable outlines for all tubes at the start
    private void DisableAllOutlines()
    {
        foreach (TubeManager tube in allTubes)
        {
            tube.GetComponent<Outline>().enabled = false;
        }
    }
}
