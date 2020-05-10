using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorFox : IActor
{
    [SerializeField]
    Material _redMat;

    [SerializeField]
    SkinnedMeshRenderer _meshRender;

    IEnumerator ChangeColor()
    {
        _meshRender.material = _redMat;
        yield return null;

    }


    IEnumerator WalkNoTarget()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("walk");
        float startTime = Time.time;

        Vector3 targetPosition = new Vector3(transform.localPosition.x+20, transform.localPosition.y, transform.localPosition.z);

        Vector3 startPosition = transform.localPosition;

        float journeyLength = Vector3.Distance(targetPosition, startPosition);
        float distCovered = (Time.time - startTime) * 1;
        float fractionOfJourney = distCovered / journeyLength;

        while (transform.localPosition.x < targetPosition.x)
        {
            distCovered = (Time.time - startTime) * 1;
            fractionOfJourney = distCovered / journeyLength;
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);

            yield return null;
        }
        yield return null;
    }

    IEnumerator WalkTowardsTarget()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("walk");
        Vector3 targetPosition = transform.parent.GetComponent<ObjectInstantiator>().GetSecondTreePosition(gameObject.transform);///new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 6);

        Vector3 startPosition = transform.localPosition;
        while (transform.localPosition.z > targetPosition.z)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, 5*Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    IEnumerator Die()
    {
        StopAllCoroutines();
        Animator animator = GetComponent<Animator>();
        animator.Play("die");
        yield return null;
    }

    IEnumerator Destroy()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        yield return null;
    }
}
