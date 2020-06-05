using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IActor : MonoBehaviour
{
    public virtual void PerformAction(string actionName)
    {
        StartCoroutine(actionName);
    }
}
