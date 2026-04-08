using UnityEngine;

public class BallBehavior : MonoBehaviour
{


    public float timeToLive = 2f; // Time in seconds before the ball destroys itself

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Destroy after 2 seconds to prevent too many balls in the scene
        Destroy(gameObject, timeToLive);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
