using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective : Sense
{
    private Transform playerTrans;
    private RaycastHit hit;
    public int ViewDistance = 100;
    public GameObject button;
    public GameObject button2;
    public GameObject StickerPanel;
    public GameObject Check;

    // 현재 본 건물
    private string storeName = "";

    protected override void Initialise()
    {
        playerTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    protected override void UpdateSense()
    {
        Vector3 dir = playerTrans.forward;
        dir.y = 0;

        if (Physics.Raycast(playerTrans.position, dir, out hit))
        {
            Debug.Log("hit point:" + hit.point + ",distance:" + hit.distance + ",name:" + hit.collider.name);
            Debug.DrawRay(playerTrans.position, dir * hit.distance, Color.red);

            if (hit.distance < 40 && (storeName != hit.collider.name))
            {
                storeName = hit.collider.name;
                GameObject.Find("Review").GetComponent<ReviewButton>().ShowButton(storeName);
            }

        }

        //if(hit.distance <= 15 && StickerPanel.gameObject.activeSelf == false)
        //{
        //    //button.gameObject.SetActive(true);
        //    //button2.gameObject.SetActive(true);
        //}
        //else
        //{
        //    //button.gameObject.SetActive(false);
        //    //button2.gameObject.SetActive(false);
        //    //Check.gameObject.SetActive(false);
        //}
    }

    void OnDrawGizmos()
    {
        if (playerTrans == null)
            return;
        Vector3 frontRayPoint = playerTrans.position + (playerTrans.forward * ViewDistance);

        Debug.DrawLine(playerTrans.position, frontRayPoint, Color.green);
    }
}
