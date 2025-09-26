using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorNoteObject : MonoBehaviour
{
    public int type = 1;
    public float time;
    public float speed = 1;
    public float x;
    public float sizex = 1;
    public float duration = 0;

    public Sprite[] spritesToRender = new Sprite[13];
    public SpriteRenderer spriteRenderer;

    BoxCollider2D coll;

    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        transform.localScale = new Vector3(sizex, sizex, sizex);
        transform.localPosition = new Vector3(GlobalVars.LEFT_X + x * GlobalVars.RADAR_WIDTH,
            GlobalVars.TimeToBeat(time) * GlobalVars.beatHeight);
        spriteRenderer.sprite = spritesToRender[type];

        if (duration > 0)
        {
            Transform trailTransform;
            trailTransform = transform.Find("HoldTrail");
            float tempScale = GlobalVars.TimeToBeat(duration) * GlobalVars.beatHeight;
            trailTransform.localScale = new Vector3(1f, 100f * tempScale, 1f);
        }
    }

    void OnMouseDown()
    {
        if (GlobalVars.mode != 1)
            return;
        GlobalVars.currentSelected = gameObject;
        GlobalVars.selectUpdated = true;
    }

    void OnDisable()
    {
        GameObject foundNote = GlobalVars.editorNotes.Find(note =>
        {
            return note.GetComponent<EditorNoteObject>() == this;
        });
        GlobalVars.editorNotes.Remove(foundNote);
        Destroy(foundNote);
    }
}
