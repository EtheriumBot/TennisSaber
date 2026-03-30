using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int score = 0;
     public TextMeshPro scoreText;

    void Awake()
    {
        instance = this;
    }

    public void AddPoint()
    {
        score++;
        scoreText.text = "Score: " + score;
    }
}