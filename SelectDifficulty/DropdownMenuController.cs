using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DropdownMenuController : MonoBehaviour
{
    TMP_Dropdown dropdown;
    List<string> difficultyNumbers = new List<string>();

    void Start()
    {
        dropdown = gameObject.GetComponent<TMP_Dropdown>();
        GlobalVars.currentMeta = JsonParser.ParseMeta(Path.Combine(GlobalVars.path, "meta.json"));
        Debug.Log(Path.Combine(GlobalVars.path, "meta.json"));
        foreach (DifficultiesItem difficultiesItem in GlobalVars.currentMeta.difficulties)
            difficultyNumbers.Add(difficultiesItem.difficulty.ToString());
        dropdown.AddOptions(difficultyNumbers);
    }

    public void OnValueChanged()
    {
        int.TryParse(difficultyNumbers[dropdown.value], out GlobalVars.currentDifficulty);
        SceneManager.LoadScene("CoreEditor");
    }
}
