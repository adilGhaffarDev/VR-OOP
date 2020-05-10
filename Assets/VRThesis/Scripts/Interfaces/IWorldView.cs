using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWorldView : MonoBehaviour
{
    protected ManagerContainer _managerContainer;

    public virtual void Initialize(ManagerContainer managerContainer)
    {
        _managerContainer = managerContainer;
    }

    protected virtual void Cleanup()
    {
        _managerContainer = null;
    }
}
