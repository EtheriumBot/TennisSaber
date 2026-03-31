using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float reflectForce = 15f;
    public float upwardBonus = 2f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the thing we hit is actually a ball
        if (other.CompareTag("Ball")) 
        {
            Rigidbody ballRb = other.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // 1. Reset its current velocity so it doesn't "fight" the hit
                ballRb.linearVelocity = Vector3.zero;

                // 2. Calculate the "Reflect" direction (away from the paddle face)
                // Using transform.forward assuming the paddle face is the 'front'
                Vector3 bounceDir = transform.forward + (Vector3.up * upwardBonus);
                
                // 3. Apply the new velocity
                ballRb.AddForce(bounceDir.normalized * reflectForce, ForceMode.Impulse);
                
                Debug.Log("Clean Hit!");
            }
        }
    }
}