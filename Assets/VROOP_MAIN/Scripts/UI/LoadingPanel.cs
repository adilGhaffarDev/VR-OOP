using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingPanel : Panel
{
    [SerializeField]
    TextMeshProUGUI _loadingText;

    public override void ShowSelf(object data)
    {
        if(data != null)
            _loadingText.text = (string)data;

        gameObject.SetActive(true);
    }

    public override void HideSelf()
    {
        _loadingText.text = "Loading";
        gameObject.SetActive(false);
    }
}
