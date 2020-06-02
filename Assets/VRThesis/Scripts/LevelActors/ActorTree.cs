using System.Collections;
using UnityEngine;

public class ActorTree : IActor
{
    Vector3 _position;
    IEnumerator set_Position()
    {
        transform.parent.GetComponent<ObjectInstantiator>().ChangePosition(gameObject);///new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 6);

        yield return null;
    }

    IEnumerator get_Position()
    {
        _position = transform.localPosition;
        yield return null;
    }
}
