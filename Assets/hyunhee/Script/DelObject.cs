using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using maxstAR;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;

public class DelObject : MonoBehaviour
{
    public GameObject btn1_go;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickDelButton()
    {
        RaycastHit vHit = GameObject.Find("SceneManager").GetComponent<MaxstSceneManager>().vHit;
        Destroy(vHit.transform.gameObject);
        btn1_go.gameObject.SetActive(false);
    }
}
