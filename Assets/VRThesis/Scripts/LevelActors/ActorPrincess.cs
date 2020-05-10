using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorPrincess : IActor
{
    [SerializeField]
    Transform _speechBubble;

    IEnumerator ShowSpeechBubble()
    {
        _speechBubble.gameObject.SetActive(true);
        yield return null;
    }
}
