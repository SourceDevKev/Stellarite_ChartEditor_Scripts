using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleFileBrowser;
using System.IO;
using TMPro;

public class FileSelectorManager : MonoBehaviour
{
    public TMP_InputField songNameInputField;
    public TMP_InputField difficultyInputField;
    public TMP_InputField difficultyNameInputField;
    public TMP_InputField chartDesignerInputField;

    IEnumerator OnShowDirDialog()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders);
        if (FileBrowser.Success)
        {
            GlobalVars.path = FileBrowser.Result[0];
        }
    }

    IEnumerator OnShowFileDialog(int state)
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files);
        if (FileBrowser.Success)
        {
            if (state == 0)
                GlobalVars.musicFilePath = FileBrowser.Result[0];
            else if (state == 1)
                GlobalVars.illustrationFilePath = FileBrowser.Result[0];
        }
    }

    public void OnSelectWorkspace()
    {
        StartCoroutine(OnShowDirDialog());
    }

    public void OnSelectMusicFile()
    {
        StartCoroutine(OnShowFileDialog(0));
    }

    public void OnSelectIllustrationFile()
    {
        StartCoroutine(OnShowFileDialog(1));
    }

    public void OnCreateChart()
    {
        // Check for a valid difficulty number; if not, clear the input field and do nothing
        DifficultiesItem difficultiesItem = new DifficultiesItem();
        difficultiesItem.chartDesigner = chartDesignerInputField.text;
        int difficultyNumber;
        bool isParsable = int.TryParse(difficultyInputField.text, out difficultyNumber);
        if (!isParsable)
        {
            difficultyInputField.text = "";
            return;
        }
        difficultiesItem.difficulty = difficultyNumber;
        difficultiesItem.difficultyName = difficultyNameInputField.text;
        
        // Copy files to workspace
        string fileName = Path.GetFileName(GlobalVars.musicFilePath);
        if (Path.GetFullPath(GlobalVars.musicFilePath) != 
            Path.GetFullPath(Path.Combine(GlobalVars.path, fileName)))
                File.Copy(GlobalVars.musicFilePath, Path.Combine(GlobalVars.path, fileName), true);
        GlobalVars.musicFilePath = Path.Combine(GlobalVars.path, fileName);

        fileName = Path.GetFileName(GlobalVars.illustrationFilePath);
        if (Path.GetFullPath(GlobalVars.illustrationFilePath) !=
            Path.GetFullPath(Path.Combine(GlobalVars.path, fileName)))
                File.Copy(GlobalVars.illustrationFilePath, Path.Combine(GlobalVars.path, fileName), true);
        GlobalVars.illustrationFilePath = Path.Combine(GlobalVars.path, fileName);

        // Finish the rest of the MetaObject and write it to a file
        MetaObject metaObject = new MetaObject();
        metaObject.displayName = songNameInputField.text;
        metaObject.musicFile = Path.GetFileName(GlobalVars.musicFilePath);
        metaObject.imageFile = Path.GetFileNameWithoutExtension(GlobalVars.illustrationFilePath);
        metaObject.difficulties = new List<DifficultiesItem>();
        metaObject.difficulties.Add(difficultiesItem);
        string outText = JsonUtility.ToJson(metaObject, true);
        File.WriteAllText(Path.Combine(GlobalVars.path, "meta.json"), outText);
        GlobalVars.currentMeta = metaObject;

        SceneManager.LoadScene("SelectDifficulty");
    }
}
