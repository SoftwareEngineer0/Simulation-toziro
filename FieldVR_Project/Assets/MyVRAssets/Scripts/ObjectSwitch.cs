using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MyVR_Assets
{
    public class ObjectSwitch : MonoBehaviour
    {
        public GameObject Genkyo;
        public GameObject Kansei;
        //public GameObject KanseiA;

        // Use this for initialization
        void Start()
        {
            Genkyo.SetActive(true);
            Kansei.SetActive(false);
        }

        public void onClickGenkyo() {
            Genkyo.SetActive(true);
            Kansei.SetActive(false);
            AutoRun.isCurrentModel = true;
        }
        public void onClickKansei() {
            Genkyo.SetActive(false);
            Kansei.SetActive(true);
            AutoRun.isCurrentModel = false;
        }

    //     public void objectswitch()
    //     {
    //         if (Count >= 2)
    //         {
    //             Count = 0;
    //         }

    //         switch (Count)
    //         {
    //             case 0:
    //                 //処理A
    //                 ObjectSwitchButtonText.text = "現況";
    //                 //Debug.Log("ObjectSwitch:現況");
    //                 Genkyo.SetActive(true);
    //                 Kansei.SetActive(false);
    //                 Count++;
    //                 AutoRun.isCurrentModel = true;
    //                 break;
    //             case 1:
    //                 //処理B
    //                 ObjectSwitchButtonText.text = "完成";
    //                 //Debug.Log("ObjectSwitch:完成");
    //                 Genkyo.SetActive(false);
    //                 Kansei.SetActive(true);
    //                 Count++;
    //                 AutoRun.isCurrentModel = false;
    //                 break;
    //             case 2:
    //                 //処理C
    //                 ObjectSwitchButtonText.text = "C";

    //                 Count++;
    //                 break;
    //         }
    //     }
    }
}
