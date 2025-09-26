using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleFileBrowser;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    IEnumerator OnShowDialog()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders);
        if (FileBrowser.Success)
        {
            GlobalVars.path = FileBrowser.Result[0];
            SceneManager.LoadScene("SelectDifficulty");
        }
    }

    public void OnOpen()
    {
        StartCoroutine(OnShowDialog());
    }

    public void OnNew()
    {
        SceneManager.LoadScene("CreateNewChart");
    }
}
