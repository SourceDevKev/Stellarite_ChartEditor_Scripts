using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LineSpawner : MonoBehaviour
{
    public GameObject normalLine;
    public GameObject boldLine;
    public TMP_InputField bpmInputField;
    public TMP_InputField nDivideInputField;
    public TMP_InputField defaultSizeInputField;
    public TMP_InputField defaultDurationInputField;
    public TMP_Text defaultTypeDisplayField;
    public Slider beatHeightSlider;
    public Transform songPosition;

    float currentSongLength;
    int beatCount;
    float currentTime;

    void Start()
    {
        GlobalVars.Init();
        LoadAudio(GlobalVars.musicFilePath);
        bpmInputField.text = GlobalVars.bpm.ToString();
        nDivideInputField.text = GlobalVars.nDivEachBeat.ToString();
        defaultSizeInputField.text = GlobalVars.defaultSize.ToString();
    }

    IEnumerator LoadSongCoroutine(string audioPath)
    {
        string url = string.Format("file://{0}", audioPath);
        WWW www = new WWW(url);
        yield return www;

        GlobalVars.currentSongAudio = www.GetAudioClip(false, false);
        currentSongLength = GlobalVars.currentSongAudio.length;
    }

    void LoadAudio(string audioPath)
    {
        StartCoroutine(LoadSongCoroutine(audioPath));
    }

    void Update()
    {
        bool res = int.TryParse(nDivideInputField.text, out GlobalVars.nDivEachBeat);
        if (GlobalVars.nDivEachBeat == 0 || !res)
        {
            GlobalVars.nDivEachBeat = 4;
        }

        res = float.TryParse(bpmInputField.text, out GlobalVars.bpm);
        if (GlobalVars.bpm == 0 || !res)
        {
            GlobalVars.bpm = 120;
        }

        res = float.TryParse(defaultSizeInputField.text, out GlobalVars.defaultSize);
        if (GlobalVars.defaultSize == 0 || !res)
        {
            GlobalVars.defaultSize = 1;
        }

        res = float.TryParse(defaultDurationInputField.text, out GlobalVars.defaultDuration);
        if (!res)
        {
            GlobalVars.defaultDuration = 0;
        }

        if (Input.inputString.Length > 0)
        {
            res = int.TryParse(Input.inputString, out GlobalVars.defaultType);
            if (GlobalVars.defaultType < 0 || GlobalVars.defaultType > 5 || !res)
            {
                GlobalVars.defaultType = 1;
            }
        }
        defaultTypeDisplayField.text = "Current type: " + GlobalVars.defaultType.ToString();

        currentTime = GlobalVars.BeatToTime(-songPosition.localPosition.y / GlobalVars.beatHeight);
        GlobalVars.currentSeekPoint = currentTime;
        GlobalVars.beatHeight = beatHeightSlider.value * GlobalVars.BASE_BEAT_HEIGHT;
        Render();
    }

    void Render()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        beatCount = Mathf.CeilToInt(GlobalVars.TimeToBeat(currentSongLength));
        for (int i = 0; i < beatCount * GlobalVars.nDivEachBeat; i++)
        {
            float curHeight = transform.TransformPoint(new Vector3(0, i * GlobalVars.beatHeight / GlobalVars.nDivEachBeat, 0)).y;
            if (curHeight > GlobalVars.RADAR_HEIGHT || curHeight < -GlobalVars.RADAR_HEIGHT)
                continue;
            GameObject spawnedLine;
            if (i % GlobalVars.nDivEachBeat == 0)
            {
                spawnedLine = Instantiate(boldLine, transform);
                GameObject textObject = spawnedLine.transform.Find("Canvas/BeatNumberText").gameObject;
                TMP_Text textComponent = textObject.GetComponent<TMP_Text>();
                textComponent.text = (i / GlobalVars.nDivEachBeat).ToString();
            }
            else
                spawnedLine = Instantiate(normalLine, transform);
            spawnedLine.transform.localPosition = new Vector3(0f, i * GlobalVars.beatHeight / GlobalVars.nDivEachBeat, 0f);
        }

        Debug.Log(-GlobalVars.TimeToBeat(currentTime) * GlobalVars.beatHeight);
        songPosition.localPosition = new Vector3(songPosition.localPosition.x,
                -GlobalVars.TimeToBeat(currentTime) * GlobalVars.beatHeight,
                songPosition.localPosition.z);
    }
}
