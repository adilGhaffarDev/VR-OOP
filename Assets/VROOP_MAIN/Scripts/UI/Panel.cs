using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    protected virtual void Start()
    {
        gameObject.SetActive(false);
    }

    public virtual void ShowSelf(object data)
    {
        gameObject.SetActive(true);
    }

    public virtual void HideSelf()
    {
        gameObject.SetActive(false);
    }

}
