using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NotesItem
{
    public int type;
    public float time;
    public float duration;
    public float speed;
    public float x;
    public float sizex;
}

[System.Serializable]
public class ChartObject
{
    public int noteNum;
    public List<NotesItem> notes;
}
