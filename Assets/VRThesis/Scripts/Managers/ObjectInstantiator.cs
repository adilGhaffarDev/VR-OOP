using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstantiator : IWorldView
{
    [SerializeField]
    Transform _parentOfObjects;

    List<IActor> _objectsInScene = new List<IActor>();
    Dictionary<string,object> _sceneData = new Dictionary<string, object>();

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
                    int indexInstantiate = 1;
                    foreach (var obj in questionAnsweredEvenData.QuestionData._objectsToBeInstantiated)
                    {
                        GameObject go = Instantiate(obj._prefab, _parentOfObjects);
                        go.name = questionAnsweredEvenData.RecodedAnswerList[0] + indexInstantiate.ToString();
                        go.transform.localPosition = obj._position;
                        go.transform.localScale = obj._scale;
                        go.transform.localRotation = Quaternion.Euler(obj._rotation);
                        go.SetActive(true);
                        _objectsInScene.Add(go.GetComponent<IActor>());
                        indexInstantiate++;
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
            Destroy(ob.gameObject);
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
            if(ob is ActorTree)
            {
                float newDist = Vector3.Distance(refObject.localPosition,ob.transform.localPosition);
                if(newDist > dist)
                {
                    dist = newDist;
                    positionVector = ob.transform.localPosition;
                }
            }
        }
        return positionVector;
    }

    public void ChangePosition(GameObject go)
    {
        go.transform.localPosition = new Vector3(go.transform.localPosition.x + 3, go.transform.localPosition.y, go.transform.localPosition.z);
    }
}
