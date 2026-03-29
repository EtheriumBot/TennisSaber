using UnityEngine;
using System.Collections.Generic;
using System.IO;

// 1. Data structures to match JSON format
[System.Serializable]
public class NoteData
{
    public float beat;
    public float position;
    public int noteType;
}

[System.Serializable]
public class LevelRoot
{
    public List<NoteData> notes;
}

public class BallSpawner : MonoBehaviour
{
    [Header("Setup")]
    public GameObject[] notePrefabs; // Assign your 3 note types here
    public float bpm = 120f;
    public string jsonFileName = "testsong.json";

    private List<NoteData> _upcomingNotes;
    private float _startTime;

    void Start()
    {
        LoadLevelData();
        _startTime = Time.time;
    }

    void Update()
    {
        if (_upcomingNotes == null || _upcomingNotes.Count == 0) return;

        // Calculate current beat based on time passed
        float elapsedSeconds = Time.time - _startTime;
        float currentBeat = elapsedSeconds * (bpm / 60f);

        // Check if the next note in the list is ready to spawn
        // We use a loop in case multiple notes land on the same beat
        while (_upcomingNotes.Count > 0 && currentBeat >= _upcomingNotes[0].beat)
        {
            SpawnNote(_upcomingNotes[0]);
            _upcomingNotes.RemoveAt(0); // Remove the note once spawned
        }
    }

    void LoadLevelData()
    {
        string filePath = Path.Combine("Assets/Resources\\", jsonFileName);

        if (File.Exists(filePath))
        {
            string jsonContents = File.ReadAllText(filePath);
            LevelRoot root = JsonUtility.FromJson<LevelRoot>(jsonContents);

            // Sort notes by beat to ensure we process them in order
            _upcomingNotes = root.notes;
            _upcomingNotes.Sort((a, b) => a.beat.CompareTo(b.beat));

            Debug.Log($"Loaded {_upcomingNotes.Count} notes.");
        }
        else
        {
            Debug.LogError("JSON file not found at " + filePath);
        }
    }

    void SpawnNote(NoteData data)
    {
        // Ensure the noteType index exists in our prefab array
        if (data.noteType < notePrefabs.Length)
        {
            // Position uses the 'position' value from JSON for the X-axis
            Vector3 spawnPos = new Vector3(data.position, 5, 10f); // 10 units in front
            Instantiate(notePrefabs[data.noteType], spawnPos, Quaternion.identity);
        }
    }
}