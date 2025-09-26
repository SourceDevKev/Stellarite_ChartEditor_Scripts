using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldTrailController : MonoBehaviour
{
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x,
            transform.localScale.y / 100f / 2f, transform.localPosition.z);
    }
}
