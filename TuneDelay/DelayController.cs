using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DelayController: MonoBehaviour
{
    public TMP_InputField bpmInputField;
    public TMP_InputField delayInputField;
    public AudioSource metronomeAudioSource;
    public AudioSource musicAudioSource;

    float tick = 0;
    float currentDelay = 0;

    IEnumerator Start()
    {
        GlobalVars.Init();
        bpmInputField.text = GlobalVars.bpm.ToString();
        delayInputField.text = (1000 * GlobalVars.currentMeta.delay).ToString();
        currentDelay = GlobalVars.currentMeta.delay;
        yield return LoadSongCoroutine(GlobalVars.musicFilePath);
        musicAudioSource.Play();
    }

    IEnumerator LoadSongCoroutine(string audioPath)
    {
        string url = string.Format("file://{0}", audioPath);
        WWW www = new WWW(url);
        yield return www;

        GlobalVars.currentSongAudio = www.GetAudioClip(false, false);

        musicAudioSource.clip = GlobalVars.currentSongAudio;
    }

    void Update()
    {
        if (!musicAudioSource.isPlaying)
        {
            OnRestart();
        }
        float beatNum = (tick + currentDelay) / 60 * GlobalVars.bpm;
        Debug.Log(beatNum);
        if (Mathf.Abs(beatNum - ((int) (beatNum + 0.5f))) <= 0.01f)
            metronomeAudioSource.Play();
        tick += Time.deltaTime;
    }

    public void OnBack()
    {
        GlobalVars.currentMeta.delay = currentDelay;
        GlobalVars.SaveMeta();
        SceneManager.LoadScene("CoreEditor");
    }

    public void OnRestart()
    {
        tick = 0;
        musicAudioSource.Stop();
        musicAudioSource.time = 0;
        musicAudioSource.Play();
    }

    public void OnBPMChanged()
    {
        float bpm;
        bool ret = float.TryParse(bpmInputField.text, out bpm);
        if (bpm == 0 || !ret)
            return;
        GlobalVars.bpm = bpm;
    }

    public void OnDelayChanged()
    {
        float delay;
        bool ret = float.TryParse(delayInputField.text, out delay);
        if (!ret)
            return;
        currentDelay = delay / 1000f;
    }
}
