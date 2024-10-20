using UnityEngine;
using DG.Tweening; // Ensure DoTween is imported for animations


public class BallFallTrigger : MonoBehaviour
{
    private TubeManager[] allTubes; // Reference to all tubes in the scene

    // Update the tube list whenever the ball enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null && ball.isFalling)
        {
            // Refresh the list of all tubes in case they were instantiated after Start
            allTubes = FindObjectsOfType<TubeManager>();

            // Log how many tubes were found
            Debug.Log($"Number of tubes found: {allTubes.Length}");

            // The ball is falling between tubes, find the closest tube
            TubeManager closestTube = FindClosestTube(ball.transform.position);

            if (closestTube != null)
            {
                // Snap the ball to the closest tube
                if (ball.IsMainBall)
                {
                    ball.transform.DOKill();
                    // Disable the ball's physics when it enters the tube
                    ball.SetPhysics(false);
                    ball.isFalling = false;

                    // Add the ball to the TubeManager and adjust remaining balls
                    closestTube.AddBallToTube(ball);

                    // Set the main ball flag to false as it's no longer the active ball
                    ball.IsMainBall = false;
                }
                //SnapBallToTube(ball, closestTube);
            }
            else
            {
                Debug.LogWarning("No closest tube found!");
            }
        }
    }

    // Find the closest tube to the ball's current position
    private TubeManager FindClosestTube(Vector3 ballPosition)
    {
        TubeManager closestTube = null;
        float minDistance = Mathf.Infinity;

        foreach (TubeManager tube in allTubes)
        {
            float distance = Vector3.Distance(ballPosition, tube.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestTube = tube;
            }
        }

        return closestTube;
    }


    private void SnapBallToTube(Ball ball, TubeManager closestTube)
    {
        ball.transform.DOMove(closestTube.TubeTopPosition.position, 0.1f);
    }
}