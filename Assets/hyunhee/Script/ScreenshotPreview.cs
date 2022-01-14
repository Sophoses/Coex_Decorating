using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ScreenshotPreview : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;
    public Text text;
    string[] files = null;
    int whichScreenShotIsShown = 0;

    // Start is called before the first frame update
    void Start()
    {
        //files = Directory.GetFiles(Application.dataPath + "/", "*.png");
        files = Directory.GetFiles(Application.persistentDataPath + "/", "*.png");
        Debug.Log(Application.dataPath);
        if (files.Length > 0)
        {
            Debug.Log("불러옴");
            whichScreenShotIsShown = files.Length - 1;
            GetPictureAndShowIt();
        }
    }

    void GetPictureAndShowIt()
    {
        string pathToFile = files[whichScreenShotIsShown];
        Debug.Log(pathToFile);
        Texture2D texture = GetScreenshotImage(pathToFile);
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        canvas.GetComponent<Image>().sprite = sp;

        string test = System.IO.Path.GetFileNameWithoutExtension(pathToFile);//날짜 시간 나타내기
        int index = test.IndexOf('t');
        test = test.Substring(index + 1);
        test = DateTime.ParseExact(test, "dd-MM-yyyy-HH-mm-ss", null).ToString("yyyy'/'MM'/'dd (HH:mm:ss)");

        text.text = test;
    }

    Texture2D GetScreenshotImage(string filePath)
    {
        Texture2D texture = null;
        byte[] fileBytes;
        if (File.Exists(filePath))
        {
            fileBytes = File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            texture.LoadImage(fileBytes);
        }
        return texture;
    }

    public void NextPicture()
    {
        if (files.Length > 0)
        {
            whichScreenShotIsShown -= 1;
            if (whichScreenShotIsShown < 0)
                whichScreenShotIsShown = files.Length - 1;
            GetPictureAndShowIt();
        }
    }

    public void PreviousPicture()
    {
        if (files.Length > 0)
        {
            whichScreenShotIsShown -= 1;
            if (whichScreenShotIsShown < 0)
                whichScreenShotIsShown = files.Length - 1;
            GetPictureAndShowIt();
        }
    }

    public void ClickShare()
    {
        string filePath = files[whichScreenShotIsShown];
        new NativeShare().AddFile(filePath).SetSubject("Arvis Calling").SetText("Arvis Calling").Share();
    }
}
