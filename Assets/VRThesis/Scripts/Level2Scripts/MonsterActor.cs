using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterActor :IActor
{
    [SerializeField]
    Animator _animator;

    IEnumerator Attack()
    {
        _animator.Play("Attack");
        yield return null;
    }

    IEnumerator Die()
    {
        _animator.Play("Die");
        yield return null;
    }
}
