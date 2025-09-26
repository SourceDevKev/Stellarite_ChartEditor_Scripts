using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GlobalVars : MonoBehaviour
{
    public static bool ready = false;

    public static string path = "C:/Users/Kevin/Desktop/test";
    public static string musicFilePath = "C:/Users/Kevin/Desktop/test/music.mp3";
    public static float bpm = 120;
    public static AudioClip currentSongAudio;
    public static string illustrationFilePath = "C:/Users/Kevin/Desktop/test/tn.png";
    public static int currentDifficulty = 2;
    public static MetaObject currentMeta;
    public static ChartObject currentChart;
    public static int currentNoteNum = 0;

    public static bool isPaused = false;
    public static bool editorIsPlaying = false;

    public const float TOP_TO_LINE = 6.21f; // Top of radar to judgement line, in units
    public const float RADAR_WIDTH = 6.48f; // In units
    public const float RADAR_HEIGHT = 6.9f;
    public const float LEFT_X = -3.24f;     // Converts "0 ~ width" to "-width / 2 ~ width / 2"
    public const float TOP_Y = 3.45f;       // Y-coord of top of radar, in units

    // Dimensions for formatting lines
    public static int nDivEachBeat = 4;
    public static float beatHeight = 2.07f;
    public const float BASE_BEAT_HEIGHT = 2.07f;
    public static float snapLines = 0;

    public static int mode = 0; // 0: draw, 1: edit
    public static List<GameObject> editorNotes = new List<GameObject>();

    public static float defaultSize = 1f;
    public static int defaultType = 1;
    public static float defaultDuration = 0;
    public static GameObject currentSelected;
    public static bool selectUpdated = false;

    public static float currentSeekPoint = 0f;

    public static float BeatToTime(float beats)
    {
        return 60.0f / bpm * beats;
    }

    public static float TimeToBeat(float seconds)
    {
        return seconds / (60.0f / bpm);
    }

    public static void Init()
    {
        currentMeta = JsonParser.ParseMeta(Path.Combine(GlobalVars.path, "meta.json"));
        musicFilePath = Path.Combine(GlobalVars.path, currentMeta.musicFile);

        string path = Path.Combine(GlobalVars.path, "chart" + currentDifficulty.ToString() + ".json");
        Debug.Log(path);
        currentChart = JsonParser.ParseChart(path);
        if (currentChart == null)
            currentChart = new ChartObject();
        currentNoteNum = currentChart.noteNum;
    }

    public static void Save()
    {
        ChartObject chartObject = new ChartObject();
        chartObject.notes = new List<NotesItem>();
        foreach (GameObject editorNote in editorNotes)
        {
            EditorNoteObject editorNoteObject = editorNote.GetComponent<EditorNoteObject>();
            NotesItem notesItem = new NotesItem();
            notesItem.type = editorNoteObject.type;
            notesItem.time = editorNoteObject.time;
            notesItem.speed = editorNoteObject.speed;
            notesItem.x = editorNoteObject.x;
            notesItem.sizex = editorNoteObject.sizex;
            notesItem.duration = editorNoteObject.duration;
            chartObject.notes.Add(notesItem);
        }
        chartObject.noteNum = chartObject.notes.Count;
        string path = Path.Combine(GlobalVars.path, "chart" + currentDifficulty.ToString() + ".json");
        File.WriteAllText(path, JsonUtility.ToJson(chartObject, true));
        currentChart = chartObject;
    }

    public static void SaveMeta()
    {
        string path = Path.Combine(GlobalVars.path, "meta.json");
        File.WriteAllText(path, JsonUtility.ToJson(currentMeta, true));
    }
}
