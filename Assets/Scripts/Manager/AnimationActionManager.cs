using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnimationAction
{
    public GameObject targetObject;     // �ִϸ��̼��� ���� ������Ʈ
    public string methodName;           // ȣ���� �޼��� �̸�
    public float delayAfter = 0f;       // ���� �� ������
    public bool runParallel = false;    // ���ÿ� ���� ����
    public bool waitForClick = false;   //  Ŭ�� ��� ����
}
[System.Serializable]
public class AnimationActionGroup
{
    public bool runParallel = false;                // �׷� ���� ���
    public List<AnimationAction> actions = new();   // ���Ե� �׼ǵ�
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
        Debug.Log("Ŭ���� ��ٸ��� ��...");
        while (!Input.GetMouseButtonDown(0))  // ���콺 ��Ŭ�� ���� (Space�� �ٲ� ���� ����)
        {
            yield return null;
        }
        Debug.Log("Ŭ�� ����, �������� ����!");
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
