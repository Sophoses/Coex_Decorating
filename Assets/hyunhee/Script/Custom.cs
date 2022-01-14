using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using maxstAR;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;

public class Custom : MonoBehaviour
{
    public List<Texture> Heads = new List<Texture>();
    public List<Texture> Hairs = new List<Texture>();
    public List<Texture> Faces = new List<Texture>();

    public List<GameObject> Charts = new List<GameObject>();
    public List<Material> Headmats = new List<Material>();
    public List<Material> Hairmats = new List<Material>();
    public List<Material> Facemats = new List<Material>();
    public RawImage headObject;
    public RawImage hairObject;
    public RawImage faceObject;
    public Text text;
    public GameObject headSticker;
    public GameObject hairSticker;
    public GameObject faceSticker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Customize()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        if (clickObject.name.Substring(0, 5) == "cus_h")
        {
            int index = int.Parse(clickObject.name.Substring(5)) - 1;
            headObject.texture = Heads[index];
            headSticker.GetComponent<MeshRenderer>().material = Headmats[index];
        }

        if (clickObject.name.Substring(0, 5) == "cus_r")
        {
            int index = int.Parse(clickObject.name.Substring(5)) - 1;
            hairObject.texture = Hairs[index];
            hairSticker.GetComponent<MeshRenderer>().material = Hairmats[index];
        }

        if (clickObject.name.Substring(0, 5) == "cus_f")
        {
            int index = int.Parse(clickObject.name.Substring(5)) - 1;
            faceObject.texture = Faces[index];
            faceSticker.GetComponent<MeshRenderer>().material = Facemats[index];
        }
    }

    public void Left()
    {
        if(Charts[0].gameObject.activeSelf == true)
        {
            text.text = "얼굴";
        }
        else if(Charts [1].gameObject.activeSelf == true)
        {
            text.text = "얼굴";
            Charts[1].gameObject.SetActive(false);
            Charts[0].gameObject.SetActive(true);
        }
        else if (Charts[2].gameObject.activeSelf == true)
        {
            text.text = "머리";
            Charts[2].gameObject.SetActive(false);
            Charts[1].gameObject.SetActive(true);
        }
    }

    public void Right()
    {
        if (Charts[2].gameObject.activeSelf == true)
        {
            text.text = "표정";
        }
        else if (Charts[1].gameObject.activeSelf == true)
        {
            text.text = "표정";
            Charts[1].gameObject.SetActive(false);
            Charts[2].gameObject.SetActive(true);
        }
        else if (Charts[0].gameObject.activeSelf == true)
        {
            text.text = "머리";
            Charts[0].gameObject.SetActive(false);
            Charts[1].gameObject.SetActive(true);
        }
    }
}
