using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  // For smooth animations

public class TubeManager : MonoBehaviour
{
    public Transform ballSpawnPosition;  // Position at the bottom of the tube
    private List<GameObject> ballsInTube = new List<GameObject>();  // List to track the balls in the tube

    // Method to spawn a ball in the tube
    public void SpawnBall(GameObject ballPrefab, int ballIndex, int colorIndex, Material ballMaterial)
    {
        // Instantiate the ball at the bottom of the tube
        GameObject newBall = Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity, transform);

        // Set the ball's color using the assigned material
        Ball ballScript = newBall.GetComponent<Ball>();
        ballScript.SetColor(ballMaterial, colorIndex);

        // Move the ball to its final position in the tube
        Vector3 targetPosition = ballSpawnPosition.position + new Vector3(0, ballIndex, 0);
        
        // Use DoTween to move the ball smoothly to the target position
        newBall.transform.DOMove(targetPosition, 0.5f).OnComplete(() =>
        {
            // Add the ball to the list of balls in this tube after the tween completes
            ballsInTube.Add(newBall);
        });
    }

    // Method to move the bottom-most ball to the top position
    public void MoveBottomBallToTop(Transform topPosition)
    {
        GameObject bottomBall = RemoveBottomBall();
        if (bottomBall != null)
        {
            // Smoothly move the bottom-most ball to the global TopPosition using DoTween
            bottomBall.transform.DOMove(topPosition.position, 1f).OnComplete(() =>
            {
                // After the bottom-most ball is removed, move the remaining balls down smoothly
                AdjustRemainingBalls();
            });
        }
    }

    // Adjust the positions of the remaining balls in the tube by moving them down smoothly
    private void AdjustRemainingBalls()
    {
        for (int i = 0; i < ballsInTube.Count; i++)
        {
            // Calculate the new position for each ball (one unit lower than its current position)
            Vector3 newPosition = ballSpawnPosition.position + new Vector3(0, i, 0);
            
            // Smoothly move each ball down to its new position
            ballsInTube[i].transform.DOMove(newPosition, 0.5f);
        }
    }

    // Remove the bottom-most ball from the tube
    public GameObject RemoveBottomBall()
    {
        if (ballsInTube.Count > 0)
        {
            GameObject bottomBall = ballsInTube[0];
            ballsInTube.RemoveAt(0);  // Remove the bottom ball from the list
            return bottomBall;  // Return the bottom ball
        }
        return null;
    }
}
