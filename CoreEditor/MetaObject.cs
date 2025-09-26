using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultiesItem
{
    public int difficulty;
    public string difficultyName;
    public string chartDesigner;
}

[System.Serializable]
public class MetaObject
{
    public string displayName;
    public string musicFile;
    public string imageFile;
    public float delay;
    public List<DifficultiesItem> difficulties;
}
