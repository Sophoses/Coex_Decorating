using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawingFunction : MonoBehaviour
{
    public List<Material> Palette = new List<Material>();
    public Camera cam;  //Gets Main Camera
    public Material defaultMaterial; //Material for Line Renderer

    private LineRenderer curLine;  //Line which draws now
    private int positionCount = 2;  //Initial start and end position
    private Vector3 PrevPos = Vector3.zero; // 0,0,0 position variable

    // Update is called once per frame
    void Update()
    {
        DrawMouse(); // 매 프레임마다 사용자가 마우스 클릭(화면터치)를 하는지 확인
    }

    void DrawMouse()
    {
        // 터치했을 때의 World좌표를 mousePos에 저장
        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.3f));
        if (Input.GetMouseButtonDown(0)) // 처음 클릭
        {
            createLine(mousePos); 
        }
        else if (Input.GetMouseButton(0)) // 드래그
        {
            connectLine(mousePos);
        }
    }

    void createLine(Vector3 mousePos)
    {
        positionCount = 2;
        GameObject line = new GameObject("Line");
        LineRenderer lineRender = line.AddComponent<LineRenderer>();

        line.transform.parent = cam.transform;
        line.transform.position = mousePos;

        lineRender.startWidth = 0.01f;
        lineRender.endWidth = 0.01f;
        lineRender.numCornerVertices = 5;
        lineRender.numCapVertices = 5;
        lineRender.material = defaultMaterial;
        lineRender.SetPosition(0, mousePos);
        lineRender.SetPosition(1, mousePos);

        curLine = lineRender;
    }
    void connectLine(Vector3 mousePos)
    {
        if (PrevPos != null && Mathf.Abs(Vector3.Distance(PrevPos, mousePos)) >= 0.001f)
        {
            PrevPos = mousePos;
            positionCount++;
            curLine.positionCount = positionCount;
            curLine.SetPosition(positionCount - 1, mousePos);
        }

    }
    public void OnClickTestButton()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        if (clickObject.name.Substring(0, 7) == "palette")
        {
            int index = int.Parse(clickObject.name.Substring(7, 1)) - 1;
            defaultMaterial = Palette[index];
        }
    }
}
