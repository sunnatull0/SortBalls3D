using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TubeManager : MonoBehaviour
{
    public Transform TubeTopPosition;
    public Transform ballSpawnPosition; // Position at the bottom of the tube
    private List<GameObject> ballsInTube = new List<GameObject>(); // List to track the balls in the tube

    private Transform topPosition;
    public Action OnBallAdded; // Event to notify when a ball is added


    [Space] [SerializeField] private float _fromStartToItsPosition = 0.3f;
    [SerializeField] private float _fromOutsideToTubeTop = 0.05f;
    [SerializeField] private float _adjustBalls = 0.3f;

    [Header("Removed Ball")] [SerializeField]
    private float _scaleDownRemovedBallTime = 0.1f;

    [SerializeField] private float _scaleUpRemovedBallTime = 0.2f;

    // Find the TopPosition in the scene during Start
    private void Start()
    {
        topPosition = GameObject.Find("TOPpos").transform; // Find TopPosition by name in the scene
    }

    // Method to spawn a new ball in the tube
    public void SpawnBall(GameObject ballPrefab, int ballIndex, int colorIndex, Material ballMaterial)
    {
        // Instantiate the ball at the bottom of the tube
        GameObject newBall = Instantiate(ballPrefab, ballSpawnPosition.localPosition, Quaternion.identity, transform);

        // Set the ball's color using the assigned material
        Ball ballScript = newBall.GetComponent<Ball>();
        ballScript.SetColor(ballMaterial, colorIndex);

        // Add the ball to the list of balls in this tube immediately
        ballsInTube.Add(newBall);

        // Move the ball to its final position in the tube locally
        Vector3 targetPosition = ballSpawnPosition.localPosition + new Vector3(0, ballIndex, 0);

        // Use DoTween to move the ball smoothly to the local target position
        newBall.transform.DOLocalMove(targetPosition, _fromStartToItsPosition);
    }

    // Method to add a ball to the tube
    public void AddBallToTube(Ball ball)
    {
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), .1f).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, .1f);
        });

        // First, make the ball a child of the tube so its position will move relative to the tube's local space
        ball.transform.SetParent(transform);

        // Move the ball to the tube's TopPosition using DoTween with local movement
        ball.transform.DOLocalMove(TubeTopPosition.localPosition, _fromOutsideToTubeTop).OnComplete(() =>
        {
            // Apply a scale-down effect
            ball.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.1f).OnComplete(() =>
            {
                // Scale back up to its original size
                ball.transform.DOScale(Vector3.one, 0.1f);
            });

            // Add the ball to the list of balls in the tube
            ballsInTube.Add(ball.gameObject);

            // Use DoTween sequence to adjust the positions of all remaining balls in the tube
            Sequence ballSequence = DOTween.Sequence();
            for (int i = 0; i < ballsInTube.Count; i++)
            {
                Vector3 newPosition = ballSpawnPosition.localPosition + new Vector3(0, i, 0);
                ballSequence.Join(ballsInTube[i].transform.DOLocalMove(newPosition, _adjustBalls));
            }

            ballSequence.OnComplete(() =>
            {
                OnBallAdded?.Invoke(); // Only check for level completion after animations are done
                MoveBottomBallToTop(topPosition);
            });
        });
    }


    public bool IsTubeSorted()
    {
        if (ballsInTube.Count == 0) return false;

        Ball firstBall = ballsInTube[0].GetComponent<Ball>();
        int firstBallColor = firstBall.GetCurrentColorIndex();

        foreach (GameObject ballObj in ballsInTube)
        {
            Ball ball = ballObj.GetComponent<Ball>();
            if (ball.GetCurrentColorIndex() != firstBallColor)
            {
                return false;
            }
        }

        return true;
    }


    // Coroutine to delay the removal of the bottom-most ball
    private IEnumerator DelayRemoveBottomBall(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        // Now move the bottom-most ball to the top position
        MoveBottomBallToTop(topPosition);
        //AdjustRemainingBalls();
    }


    // Method to move the bottom-most ball to the top position
    public void MoveBottomBallToTop(Transform topPosition)
    {
        if (TubeSpawner.IsCompleted) return;

        GameObject bottomBall = RemoveBottomBall();
        if (bottomBall != null)
        {
            // Animate the ball scaling down to zero before moving it to the top
            bottomBall.transform.DOScale(Vector3.zero, _scaleDownRemovedBallTime).SetEase(Ease.InBack, 3f).OnComplete(
                () =>
                {
                    EffectsManager.Instance.OnBallRemoved(bottomBall.transform, transform);
                    AdjustRemainingBalls();

                    // After scaling down, move the ball to the top position
                    bottomBall.transform
                        .SetParent(null); // Un-parent the ball so it's no longer part of the tube hierarchy
                    bottomBall.transform.position = topPosition.position; // Move it to the TopPosition

                    bottomBall.GetComponent<Ball>().IsMainBall = true;

                    EffectsManager.Instance.OnBallSpawnedAtTop(bottomBall.transform, bottomBall);
                    // Scale the ball back up from zero to its original size at the top position
                    bottomBall.transform.DOScale(Vector3.one, _scaleUpRemovedBallTime).SetEase(Ease.OutBack, 3.5f);
                });
        }
    }

    // Method to adjust the remaining balls in the tube
    public void AdjustRemainingBalls()
    {
        for (int i = 0; i < ballsInTube.Count; i++)
        {
            // Calculate the new local position for each ball (adjust based on index in the list)
            Vector3 newPosition = ballSpawnPosition.localPosition + new Vector3(0, i, 0);

            // Move the ball smoothly to its new local position
            ballsInTube[i].transform.DOLocalMove(newPosition, _adjustBalls); // Use DOLocalMove for local space movement
        }
    }

    // Method to remove the bottom-most ball from the tube
    public GameObject RemoveBottomBall()
    {
        if (ballsInTube.Count > 0)
        {
            GameObject bottomBall = ballsInTube[0];
            ballsInTube.RemoveAt(0); // Remove the bottom ball from the list
            return bottomBall; // Return the bottom ball
        }

        return null;
    }
}