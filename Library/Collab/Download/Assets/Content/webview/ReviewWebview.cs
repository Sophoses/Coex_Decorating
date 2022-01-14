using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewWebview : MonoBehaviour
{

    public WebViewObject target;
    private string URL = "";
    public GUISkin skin;
    private void StartWebView()
    {
#if UNITY_EDITOR
    return;
#endif

        try
        {
            Debug.Log("Webview init called "+URL);
            target.Init((msg) =>
            {
                Debug.Log("webview init "+msg);

            });

            target.LoadURL(URL);
            target.SetMargins(100, 100, 100, 100);
            target.SetVisibility(true);
        }
        catch (System.Exception e)
        {
            Debug.Log("web view error:" + e);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        URL = ReviewButton.url;
        StartWebView();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI() 
    {
        GUI.skin = skin;
        int sw = Screen.width;
        if (GUI.Button(new Rect(sw-170, 10, 160, 160), "¡¿"))
        {
            GameObject.Find("SceneManager").GetComponent<MaxstSceneManager>().OnClickBackButton();
            Destroy(this.gameObject);
        }

    }
}
