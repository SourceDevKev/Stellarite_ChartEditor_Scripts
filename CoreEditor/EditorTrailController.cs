using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTrailController : MonoBehaviour
{
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x,
            transform.localScale.y / 100f / 2f, transform.localPosition.z);
    }

    void OnMouseDown()
    {
        if (GlobalVars.mode != 1)
            return;
        GlobalVars.currentSelected = transform.parent.gameObject;
        GlobalVars.selectUpdated = true;
    }
}
