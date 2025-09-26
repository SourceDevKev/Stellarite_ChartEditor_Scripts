using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnapController : MonoBehaviour
{
    public TMP_InputField snapInputField;
    public GameObject verticalLine;

    public void OnChange()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        bool res = float.TryParse(snapInputField.text, out GlobalVars.snapLines);
        if (GlobalVars.snapLines == 0 || !res)
            return;

        for (int i = 1; i < GlobalVars.snapLines; i++)
        {
            GameObject spawnedLine = Instantiate(verticalLine, transform);
            spawnedLine.transform.localPosition = new Vector3(GlobalVars.RADAR_WIDTH / GlobalVars.snapLines * i, 0f, 0f);
        }
    }
}
