using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnimationAction
{
    public GameObject targetObject;     // 애니메이션이 붙은 오브젝트
    public string methodName;           // 호출할 메서드 이름
    public float delayAfter = 0f;       // 실행 후 딜레이
    public bool runParallel = false;    // 동시에 실행 여부
    public bool waitForClick = false;   //  클릭 대기 여부
}
[System.Serializable]
public class AnimationActionGroup
{
    public bool runParallel = false;                // 그룹 실행 방식
    public List<AnimationAction> actions = new();   // 포함된 액션들
}

public class AnimationActionManager : MonoBehaviour
{
    public List<AnimationActionGroup> actionGroups;

    private void Start()
    {
        StartCoroutine(ExecuteGroups());
    }

    private IEnumerator ExecuteGroups()
    {
        foreach (var group in actionGroups)
        {
            if (group.runParallel)
            {
                List<Coroutine> coroutines = new();
                foreach (var action in group.actions)
                {
                    coroutines.Add(StartCoroutine(InvokeMethod(action)));
                }
                foreach (var co in coroutines)
                {
                    yield return co;
                }
            }
            else
            {
                foreach (var action in group.actions)
                {
                    yield return InvokeMethod(action);
                }
            }
        }
    }


    private IEnumerator WaitForClick()
    {
        Debug.Log("클릭을 기다리는 중...");
        while (!Input.GetMouseButtonDown(0))  // 마우스 좌클릭 기준 (Space로 바꿀 수도 있음)
        {
            yield return null;
        }
        Debug.Log("클릭 감지, 다음으로 진행!");
    }


    private IEnumerator InvokeMethod(AnimationAction action)
    {
        string fullMethod = action.methodName;
        string methodOnly = fullMethod.Contains("/") ? fullMethod.Split('/')[1] : fullMethod;

        action.targetObject.SendMessage(methodOnly, SendMessageOptions.DontRequireReceiver);
        yield return new WaitForSeconds(action.delayAfter);


        if (action.waitForClick)
        {
            yield return WaitForClick();
        }
    }



}
