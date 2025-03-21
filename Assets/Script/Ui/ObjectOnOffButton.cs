using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnOffButton : MonoBehaviour
{
    public GameObject OnOffObject;


    public void ObjectOn()
    {
        OnOffObject.SetActive(true);
    }

    public void ObjectOff()
    {
        OnOffObject.SetActive(false);
    }

}
