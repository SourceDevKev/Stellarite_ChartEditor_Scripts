using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteController : MonoBehaviour
{
    public GameObject radar;
    public GameObject[] notesToSpawn = new GameObject[6];

    List<NotesItem> notes;

    public AudioSource audioSource;
    public AudioSource tapSoundPlayer;

    // Runtime vars
    float sysOffset; // Offset to avoid negative time
    int ptr = 0;     // Index of next note to spawn
    float tick = 0;
    bool flag = false;
    List<(GameObject, int)> onScreenNotes;

    IEnumerator LoadSongCoroutine(string audioPath)
    {
        string url = string.Format("file://{0}", audioPath);
        WWW www = new WWW(url);
        yield return www;

        GlobalVars.currentSongAudio = www.GetAudioClip(false, false);
        audioSource.clip = GlobalVars.currentSongAudio;
    }

    void Start()
    {
        GlobalVars.Init();

        ptr = 0;
        tick = 0;
        
        onScreenNotes = new List<(GameObject, int)>();

        // Sort notes by their spawn time in ascending order
        GlobalVars.currentChart.notes.Sort((x, y) => (GetSpawnTime(x))
                                           .CompareTo(GetSpawnTime(y)));
        notes = GlobalVars.currentChart.notes;

        // Meanwhile get the smallest tick and set that to sysOffset
        sysOffset = Mathf.Max(0f, -GetSpawnTime(GlobalVars.currentChart.notes[0]));

        if (GlobalVars.currentMeta.delay < 0)
        {
            sysOffset -= GlobalVars.currentMeta.delay;
        }

        Debug.Log(sysOffset);

        StartCoroutine(LoadSongCoroutine(GlobalVars.musicFilePath));
    }

    float GetSpawnTime(NotesItem note)
    {
        return note.time - 1f / note.speed;
    }

    void Update()
    {
        tick += Time.deltaTime;
        Debug.Log(tick + " " + sysOffset);
        if (!flag && tick - GlobalVars.currentMeta.delay >= sysOffset && audioSource.clip != null)
        {
            audioSource.Play();
            flag = true;
        }
        if (!audioSource.isPlaying && tick > sysOffset && ptr >= GlobalVars.currentChart.noteNum && onScreenNotes.Count == 0 && flag)
        {
            SceneManager.LoadScene("CoreEditor");
            return;
        }
        while (ptr < GlobalVars.currentNoteNum && tick - GetSpawnTime(notes[ptr]) - sysOffset >= 0)
        {
            Vector3 pos = new Vector3(notes[ptr].x * GlobalVars.RADAR_WIDTH + GlobalVars.LEFT_X, GlobalVars.TOP_Y);
            GameObject spawnedNote = Instantiate(notesToSpawn[notes[ptr].type], pos, Quaternion.identity);
            spawnedNote.transform.localScale = new Vector3(notes[ptr].sizex, 1f, 1f);
            spawnedNote.transform.parent = transform;
            spawnedNote.transform.localPosition = pos;
            if (notes[ptr].type == 0)
            {
                onScreenNotes.Add((spawnedNote, ptr++));
                continue;
            }

            Transform trailTransform;
            trailTransform = spawnedNote.transform.Find("HoldTrail");
            float tempScale = notes[ptr].speed * GlobalVars.TOP_TO_LINE * notes[ptr].duration;
            trailTransform.localScale = new Vector3(1f, 100f * tempScale, 1f);
            trailTransform.localPosition = new Vector3(trailTransform.localPosition.x, tempScale / 2f, trailTransform.localPosition.z);
            onScreenNotes.Add((spawnedNote, ptr++));
        }

        List<(GameObject, int)> toBeDestroyedNotes = new List<(GameObject, int)>();
        foreach ((GameObject, int) note in onScreenNotes)
        {
            if (note.Item1 != null)
            {
                if (tick >= notes[note.Item2].time + sysOffset + notes[note.Item2].duration)
                {
                    tapSoundPlayer.Play();
                    Destroy(note.Item1);
                    toBeDestroyedNotes.Add(note);
                }
                note.Item1.transform.localPosition =
                    new Vector3(note.Item1.transform.localPosition.x,
                                note.Item1.transform.localPosition.y -
                                notes[note.Item2].speed * GlobalVars.TOP_TO_LINE * Time.deltaTime);
            }
            else
                toBeDestroyedNotes.Add(note);
        }
        foreach ((GameObject, int) note in toBeDestroyedNotes)
        {
            Destroy(note.Item1);
            onScreenNotes.Remove(note);
        }
    }

    public void OnCatch(GameObject collision)
    {
        tapSoundPlayer.Play();
        (GameObject, int) foundNote = onScreenNotes.Find(note =>
        {
            return note.Item1 == collision;
        });
        onScreenNotes.Remove(foundNote);
        Destroy(foundNote.Item1);
    }
}
