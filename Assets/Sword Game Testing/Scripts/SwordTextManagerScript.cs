using UnityEngine;
using System.Collections;

public class SwordTextManagerScript : MonoBehaviour {

    GameObject HMD;

    Transform[] Texts;

    private int HitCounts = 0;


    // Use this for initialization
    void Start () {
        HMD = GameObject.FindGameObjectWithTag("MainCamera");
        Texts = GetComponentsInChildren<Transform>();

        if (Texts[1].tag == "HitText")
        {
            Texts[1].GetComponent<TextMesh>().text = "Hits : " + HitCounts.ToString();
        }
        else
            Debug.Log("Texts[0] is NOT Hit Text!");


        //foreach (var text in Texts)
        //{
        //    if (text.CompareTag("HitText"))
        //    {
        //        text.GetComponent<TextMesh>().text = "Hits : " + HitCounts.ToString();
        //        return;
        //    }
        //}
    }

    void FixedUpdate()
    {
        foreach (var text in Texts)
        {
            text.transform.LookAt(HMD.transform.position);
        }
    }

    public void AddHitCount()
    {
        HitCounts++;
        UpdateText();
    }

    void UpdateText()
    {
        Texts[1].GetComponent<TextMesh>().text = "Hits : " + HitCounts.ToString();
    }
}
