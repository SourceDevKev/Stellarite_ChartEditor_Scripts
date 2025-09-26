using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConverterController : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text textField;

    void Update()
    {
        float steps;
        bool res = float.TryParse(inputField.text, out steps);
        if (!res)
        {
            textField.text = "= 0 second(s)";
            return;
        }
        textField.text = "= " + GlobalVars.BeatToTime(steps / GlobalVars.nDivEachBeat).ToString("0.0000") + " second(s)";
    }
}
