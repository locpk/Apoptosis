using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{


    public const int MAX_CAP = 20;
    public static int cap = 0;
    public GameObject movePin;
    public GameObject attackPin;

    void MouseFeedback()
    {
        RaycastHit hit;
        Ray mousetoWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mousetoWorldRay, out hit, 2000))
        {
            //if (hit.collider.name == "Test_Ground")
            //{
            //    Debug.DrawRay(mousetoWorldRay.origin, mousetoWorldRay.direction * 1000);
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        //GameObject.Instantiate(attackPin, hit.point, Quaternion.identity);
            //    }
            //    if (Input.GetMouseButtonDown(1))
            //    {
            //      // GameObject.Instantiate(movePin, hit.point, Quaternion.identity);
            //    }

            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
       // MouseFeedback();

       
    }
}
