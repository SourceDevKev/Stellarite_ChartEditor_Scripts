using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PropertiesPanelController : MonoBehaviour
{
    public TMP_InputField typeInputField;
    public TMP_InputField speedInputField;
    public TMP_InputField xInputField;
    public TMP_InputField sizeInputField;
    public TMP_InputField durationInputField;

    void Update()
    {
        if (GlobalVars.currentSelected == null)
            return;
        EditorNoteObject editorNoteObject = GlobalVars.currentSelected.GetComponent<EditorNoteObject>();
        if (GlobalVars.selectUpdated)
        {
            typeInputField.text = editorNoteObject.type.ToString();
            speedInputField.text = editorNoteObject.speed.ToString();
            xInputField.text = editorNoteObject.x.ToString();
            sizeInputField.text = editorNoteObject.sizex.ToString();
            durationInputField.text = editorNoteObject.duration.ToString();
            GlobalVars.selectUpdated = false;
            return;
        }

        bool res = int.TryParse(typeInputField.text, out editorNoteObject.type);
        if (!res)
            editorNoteObject.type = 1;

        res = float.TryParse(speedInputField.text, out editorNoteObject.speed);
        if (!res)
            editorNoteObject.speed = 1;

        res = float.TryParse(xInputField.text, out editorNoteObject.x);
        if (!res)
            editorNoteObject.x = 0;

        res = float.TryParse(sizeInputField.text, out editorNoteObject.sizex);
        if (!res)
            editorNoteObject.sizex = 1;

        res = float.TryParse(durationInputField.text, out editorNoteObject.duration);
        if (!res || editorNoteObject.type == 0)
            editorNoteObject.duration = 0;
    }
}
