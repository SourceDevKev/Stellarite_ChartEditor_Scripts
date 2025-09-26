using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButtonController : MonoBehaviour
{
    public void OnClick()
    {
        Destroy(GlobalVars.currentSelected);
        GlobalVars.currentSelected = null;
    }
}
