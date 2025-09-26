using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NoteSpawner : MonoBehaviour
{
    public GameObject noteTemplate;

    void Start()
    {
        GlobalVars.Init();

        List<NotesItem> notesItems;
        if (GlobalVars.currentChart.notes == null)
            GlobalVars.currentChart.notes = new List<NotesItem>();
        notesItems = GlobalVars.currentChart.notes;
        foreach (NotesItem note in notesItems)
        {
            GameObject spawnedNote = Instantiate(noteTemplate, transform);
            EditorNoteObject editorNoteObject = spawnedNote.GetComponent<EditorNoteObject>();
            editorNoteObject.type = note.type;
            editorNoteObject.time = note.time;
            editorNoteObject.speed = note.speed;
            editorNoteObject.x = note.x;
            editorNoteObject.sizex = note.sizex;
            editorNoteObject.duration = note.duration;
            GlobalVars.editorNotes.Add(spawnedNote);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Destroy(GlobalVars.currentSelected);
            GlobalVars.currentSelected = null;
        }
    }
}
