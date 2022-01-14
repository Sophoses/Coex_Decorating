using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// JSON 직렬화
[System.Serializable]
public class Store
{
    public string store_index;
    public string store_name;
    public string store_review_URL;
}

[System.Serializable]
public class StoreData
{
    public Store[] store;
}

// Review 버튼 생성 여부 결정
// JSON 파일 읽어오기
public class ReviewButton : MonoBehaviour
{

    public Button button; // review webview창을 여는 버튼
    private bool ButtonIsExist = false; // 리뷰 버튼의 존재 여부 
    public static string url; // webview에서 접근할 url
    public GameObject target; // webview 객체
    public Text storeName;
    StoreData data = null; // json에서 읽어온 정보들

    private string storeIndex = "";
    private string currentStoreName = "None";
  
    void Awake()
    {
        button.interactable = false;
        LoadData();
    }


    void Update()
    {
        if (storeIndex.Contains("Object"))
        {
            currentStoreName = " ";
            storeName.text = currentStoreName;
        }
        else if (currentStoreName == "None" || button.gameObject.activeSelf == false)
        {
            storeName.text = " ";
        }
        else
        {

            storeName.text = currentStoreName;

            int index = findStoreIndex();
            if (currentStoreName != data.store[index].store_name)
            {
                currentStoreName = data.store[index].store_name;
                storeName.text = currentStoreName;
                StartCoroutine(TextFadeInEffect());
            }
        }
    }

    public void ShowButton(string storeIndex)
    {
        Debug.Log("Hello" + storeIndex);
        this.storeIndex = storeIndex;
        int index = findStoreIndex();

        // 등록된 건물 아님. or 건물 리뷰창 없음.
        if (storeIndex.Contains("Object") || data.store[index].store_review_URL == "no data")
        {
            button.interactable = false;
            return;
        }

        button.interactable = true;

    }

    public void ClickReviewBtn() 
    {
        int index = findStoreIndex();
        url = data.store[index].store_review_URL;
        Debug.Log("Load Webview: " + url);
        GameObject.Find("SceneManager").GetComponent<MaxstSceneManager>().OnReviewButton(data.store[index].store_name);
        Instantiate(target);
    }

    // JSON -> StoreData 클래스로
    private void LoadData()
    {
        string json_fileName = "store_review.json";

        if (Application.platform == RuntimePlatform.Android)
        {
            string path = Path.Combine(Application.streamingAssetsPath, json_fileName);

            WWW reader = new WWW(path);
            while (!reader.isDone) { }

            string text = reader.text;
            data = JsonUtility.FromJson<StoreData>(text);
            Debug.Log("json if");

        }
        else
        {
            Debug.Log("json else");
            string path = File.ReadAllText(Application.dataPath + "/Resources/" + json_fileName);
            data = JsonUtility.FromJson<StoreData>(path);
        }

        Debug.Log(data.store.Length);
        Debug.Log(data.store[0].store_index + " " + data.store[0].store_name + " " + data.store[0].store_review_URL);
        Debug.Log(data.store[0].store_index + " " + data.store[1].store_name + " " + data.store[1].store_review_URL);

    }

    // 건물 정보(ex. shop_T01)를 인덱스 형태로 변환
    private int findStoreIndex()
    {
        int storeNumber = int.Parse(storeIndex.Substring(6, 2)); // "T01"에서 01만 뽑아내서 정수형으로 변환

        int index = 0; // json파일 store 배열의 index 값으로 쓸 것

        switch (storeIndex[5])
        {
            case 'T': index += 10; goto case 'S';
            case 'S': index += 8; goto case 'R';
            case 'R': index += 24; goto case 'Q';
            case 'Q': index += 21; goto case 'P';
            case 'P': index += 22; goto case 'O';
            case 'O': index += 13; goto case 'N';
            case 'N': index += 7; goto case 'M';
            case 'M': index += 6; goto case 'L';
            case 'L': index += 14; goto case 'K';
            case 'K': index += 13; goto case 'J';
            case 'J': index += 7; goto case 'I';
            case 'I': index += 6; goto case 'H';
            case 'H': index += 21; goto case 'G';
            case 'G': index += 7; goto case 'F';
            case 'F': index += 12; goto case 'E';
            case 'E': index += 10; goto case 'D';
            case 'D': index += 12; goto case 'C';
            case 'C': index += 10; goto case 'B';
            case 'B': index += 8; goto case 'A';
            case 'A':
                index += storeNumber - 1;
                break;
        }

        return index;
    }

    public IEnumerator TextFadeInEffect()
    {
        storeName.color = new Color(storeName.color.r, storeName.color.g, storeName.color.b, 0);
        while (storeName.color.a < 1.0f)
        {
            storeName.color = new Color(storeName.color.r, storeName.color.g, storeName.color.b, storeName.color.a + (Time.deltaTime * 2.0f));
            yield return null;
        }
    }
}
