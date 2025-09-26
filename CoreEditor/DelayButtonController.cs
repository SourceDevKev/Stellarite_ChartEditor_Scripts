using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayButtonController : MonoBehaviour
{
    public void OnRedirect()
    {
        GlobalVars.Save();
        SceneManager.LoadScene("TuneDelay");
    }
}
