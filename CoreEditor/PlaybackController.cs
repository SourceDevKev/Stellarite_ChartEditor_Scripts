using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaybackController : MonoBehaviour
{
    public GameObject editGroup;
    public Transform songPosition;

    AudioSource audioSource;

    IEnumerator Start()
    {
        audioSource = GetComponent<AudioSource>();
        yield return LoadSongCoroutine(GlobalVars.musicFilePath);
    }

    IEnumerator LoadSongCoroutine(string audioPath)
    {
        string url = string.Format("file://{0}", audioPath);
        WWW www = new WWW(url);
        yield return www;

        GlobalVars.currentSongAudio = www.GetAudioClip(false, false);

        audioSource.clip = GlobalVars.currentSongAudio;
    }

    void Update()
    {
        if (audioSource.clip == null)
            return;
        if (audioSource.time > audioSource.clip.length)
        {
            audioSource.time = 0f;
            songPosition.position = Vector3.zero;
            audioSource.Stop();
            GlobalVars.editorIsPlaying = false;
            editGroup.SetActive(true);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnTogglePlayPause();
        }
        if (GlobalVars.editorIsPlaying)
        {
            songPosition.localPosition = new Vector3(songPosition.localPosition.x,
                songPosition.localPosition.y - GlobalVars.TimeToBeat(1f) * GlobalVars.beatHeight * Time.deltaTime,
                songPosition.localPosition.z);
        }
        else
        {
            songPosition.localPosition = new Vector3(songPosition.localPosition.x,
                Mathf.Max(-Mathf.CeilToInt(GlobalVars.TimeToBeat(audioSource.clip.length)) * GlobalVars.beatHeight,
                          Mathf.Min(songPosition.localPosition.y -
                              Input.mouseScrollDelta.y * GlobalVars.beatHeight / GlobalVars.nDivEachBeat, 0)),
                songPosition.localPosition.z);
        }
    }

    public void OnTogglePlayPause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            GlobalVars.editorIsPlaying = false;
            editGroup.SetActive(true);
        }
        else
        {
            audioSource.Play();
            audioSource.time = GlobalVars.BeatToTime(-songPosition.localPosition.y / GlobalVars.beatHeight);
            GlobalVars.editorIsPlaying = true;
            editGroup.SetActive(false);
        }
    }
}
