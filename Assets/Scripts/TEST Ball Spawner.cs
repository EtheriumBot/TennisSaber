using System.Threading;
using UnityEngine;

public class TESTBallSpawner : MonoBehaviour
{

    public GameObject ball;

    private int timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer % 100 == 0)
        {
            Vector3 spawnLoc = new Vector3(Random.Range(-10f, 10f), 10f, Random.Range(-10f, 10f));
            Instantiate(ball, spawnLoc, Quaternion.identity);
        }
        timer++;
    }
}
