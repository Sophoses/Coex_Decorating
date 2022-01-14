using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{
    [SerializeField]
    GameObject blink;

    public void TakeAShot()
    {
        StartCoroutine("CaptureIt");
    }

    IEnumerator CaptureIt()
    {
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";
        string pathToSave = fileName;

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

        ScreenCapture.CaptureScreenshot(pathToSave);
        yield return new WaitForEndOfFrame();
        Instantiate(blink, new Vector3(0f, 0f, -43f), blink.transform.rotation);
        Destroy(GameObject.Find(blink.name + "(Clone)"), 0.3f);

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
    }
}