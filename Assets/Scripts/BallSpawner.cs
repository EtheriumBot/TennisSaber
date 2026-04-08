using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

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
    public GameObject ghostNotePrefab; // This note lets the user know the block is coming
    public float bpm = 120f;
    public string jsonFileName = "testsong.json";

    private List<NoteData> _upcomingNotes;
    private List<NoteData> _upcomingGhostNotes;
    private float _startTime;

    void Start()
    {
        LoadLevelData();
        _startTime = Time.time;
    }

    void Update()
    {
        if (_upcomingNotes == null || _upcomingNotes.Count == 0) return;

        _upcomingGhostNotes = new List<NoteData>(_upcomingNotes); // Create a copy for ghost notes

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

        while (_upcomingGhostNotes.Count > 0 && currentBeat + 1f >= _upcomingGhostNotes[0].beat) // Spawn ghost note 1 beat early
        {
            SpawnGhostNote(_upcomingGhostNotes[0]);
            _upcomingGhostNotes.RemoveAt(0); // Remove the ghost note once spawned
        }
    }

    void LoadLevelData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        StartCoroutine(LoadAsset(filePath));
    }
    IEnumerator LoadAsset(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error while loading asset: " + uwr.error);
            }
            else
            {
                // Get the downloaded text/data
                string data = uwr.downloadHandler.text;
                LevelRoot root = JsonUtility.FromJson<LevelRoot>(data);

                // Sort notes by beat to ensure we process them in order
                _upcomingNotes = root.notes;
                _upcomingNotes.Sort((a, b) => a.beat.CompareTo(b.beat));

                Debug.Log($"Loaded {_upcomingNotes.Count} notes.");
            }
        }
    }

    void SpawnNote(NoteData data)
    {
        // Ensure the noteType index exists in our prefab array
        if (data.noteType < notePrefabs.Length)
        {
            // Position uses the 'position' value from JSON for the X-axis
            Vector3 spawnPos = new Vector3(data.position, 5, 1f); // 10 units in front

            GameObject ballSpawn = notePrefabs[data.noteType];
            Instantiate(ballSpawn, spawnPos, Quaternion.identity);
        }
    }

    void SpawnGhostNote(NoteData data)
    {
        // Ensure the noteType index exists in our prefab array
        if (data.noteType < notePrefabs.Length)
        {
            // Position uses the 'position' value from JSON for the X-axis
            Vector3 spawnPos = new Vector3(data.position, 5, 1f); // 10 units in front

            GameObject ballSpawn = ghostNotePrefab;
            Instantiate(ballSpawn, spawnPos, Quaternion.identity);
        }
    }
}