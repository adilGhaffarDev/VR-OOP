using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstantiator : IWorldView
{
    [SerializeField]
    Transform _parentOfObjects;

    List<GameObject> _objectsInScene = new List<GameObject>();

    public override void Initialize(ManagerContainer managerContainer)
    {
        base.Initialize(managerContainer);
        EventManager.StartListening(EventNames.QuestionAnswered, OnQuestionAnswered);
        EventManager.StartListening(EventNames.OnLevelLoaded, CleanLastLevel);

    }

    void OnQuestionAnswered(object data)
    {
        if(data is QuestionAnsweredEvenData)
        {
            QuestionAnsweredEvenData questionAnsweredEvenData = data as QuestionAnsweredEvenData;
            if (questionAnsweredEvenData.IsCorrect)
            {
                if (questionAnsweredEvenData.QuestionData._objectsToBeInstantiated != null)
                {
                    foreach (var obj in questionAnsweredEvenData.QuestionData._objectsToBeInstantiated)
                    {
                        GameObject go = Instantiate(obj._prefab, _parentOfObjects);
                        go.name = obj._prefab.name;
                        go.transform.localPosition = obj._position;
                        go.transform.localScale = obj._scale;
                        go.transform.localRotation = Quaternion.Euler(obj._rotation);
                        go.SetActive(true);
                        _objectsInScene.Add(go);
                    }

                }
                if (!string.IsNullOrWhiteSpace(questionAnsweredEvenData.QuestionData._actor))
                {
                    Transform actor = transform.Find(questionAnsweredEvenData.QuestionData._actor);
                    actor.GetComponent<IActor>().PerformAction(questionAnsweredEvenData.QuestionData._action);
                }
            }
        }
       
    }

    void CleanLastLevel(object data)
    {
        foreach(var ob in _objectsInScene)
        {
            Destroy(ob);
        }
        _objectsInScene.Clear();
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        EventManager.StopListening(EventNames.QuestionAnswered, OnQuestionAnswered);
    }

    public Vector3 GetSecondTreePosition(Transform refObject)
    {
        float dist = float.MaxValue;
        Vector3 positionVector = Vector3.zero;
        foreach (var ob in _objectsInScene)
        {
            if(ob.name == "Palm_Tree")
            {
                float newDist = Vector3.Distance(refObject.localPosition,ob.transform.localPosition);
                if(newDist< dist)
                {
                    dist = newDist;
                    positionVector = ob.transform.localPosition;
                }
            }
        }
        return positionVector;
    }
}
