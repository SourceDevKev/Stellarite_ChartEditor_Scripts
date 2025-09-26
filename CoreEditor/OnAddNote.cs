using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnAddNote : MonoBehaviour
{
    public GameObject noteToSpawn;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            GlobalVars.mode = 0;
        else if (Input.GetKeyDown(KeyCode.S))
            GlobalVars.mode = 1;
        else if (Input.GetKeyDown(KeyCode.A))
            GlobalVars.Save();

        if (Input.GetButtonDown("Fire1") && GlobalVars.mode == 0)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.x -= Screen.width / 2;
            mousePos.y -= Screen.height / 2;
            mousePos.x /= 100f;
            mousePos.y /= 100f;
            mousePos = transform.InverseTransformPoint(mousePos);

            mousePos.x -= GlobalVars.LEFT_X;
            if (mousePos.x / GlobalVars.RADAR_WIDTH < 0 || mousePos.x / GlobalVars.RADAR_WIDTH > 1)
                return;

            GameObject spawnedNote = Instantiate(noteToSpawn, transform);
            EditorNoteObject editorNoteObject = spawnedNote.GetComponent<EditorNoteObject>();

            if (GlobalVars.snapLines == 0)
                editorNoteObject.x = mousePos.x / GlobalVars.RADAR_WIDTH;
            else
            {
                editorNoteObject.x = ((int) (mousePos.x / GlobalVars.RADAR_WIDTH / (1f / GlobalVars.snapLines)) + 0.5f)
                                     * 1f / GlobalVars.snapLines;
            }

            int stepNum = (int) (mousePos.y / (GlobalVars.beatHeight / GlobalVars.nDivEachBeat) + 0.5f);
            editorNoteObject.time = GlobalVars.BeatToTime((float) stepNum / GlobalVars.nDivEachBeat);

            editorNoteObject.speed = 1;
            editorNoteObject.sizex = GlobalVars.defaultSize;
            editorNoteObject.type = GlobalVars.defaultType;
            editorNoteObject.duration = GlobalVars.defaultDuration;

            GlobalVars.editorNotes.Add(spawnedNote);
        }
    }
}
