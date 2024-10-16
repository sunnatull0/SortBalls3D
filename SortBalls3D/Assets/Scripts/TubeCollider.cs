using UnityEngine;

public class TubeCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("Ball entered the tube!");
            // You can now handle the logic for the ball entering the tube
            // For example: Call AddBall() method or trigger an animation
        }
    }
}