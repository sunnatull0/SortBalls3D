using DG.Tweening;
using UnityEngine;

public class TubeCollider : MonoBehaviour
{
    private TubeManager tubeManager;

    void Start()
    {
        // Get the TubeManager from the parent
        tubeManager = GetComponentInParent<TubeManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ball ball = other.GetComponent<Ball>();
        // if (ball != null)
        // {
        //     if (ball.IsMainBall)
        //     {
        //         ball.transform.DOKill();
        //         // Disable the ball's physics when it enters the tube
        //         ball.SetPhysics(false);
        //         ball.isFalling = false;
        //
        //         // Add the ball to the TubeManager and adjust remaining balls
        //         tubeManager.AddBallToTube(ball);
        //
        //         // Set the main ball flag to false as it's no longer the active ball
        //         ball.IsMainBall = false;
        //     }
        // }
    }
}