using UnityEngine;

public class HoopTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ScoreManager.instance.AddPoint();
        }
    }
}