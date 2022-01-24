using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MyVR_Assets
{
    public class ObjectSwitch : MonoBehaviour
    {
        public GameObject Genkyo;
        //public GameObject Kansei;
        public GameObject KanseiA;
        public GameObject KanseiB;
        public GameObject KanseiC;
        public GameObject KanseiD;

        // Use this for initialization
        void Start()
        {
            Genkyo.SetActive(true);
            KanseiA.SetActive(false);
            KanseiB.SetActive(false);
            KanseiC.SetActive(false);
            KanseiD.SetActive(false);
        }

        public void onClickGenkyo() {
            Genkyo.SetActive(true);
            KanseiA.SetActive(false);
            KanseiB.SetActive(false);
            KanseiC.SetActive(false);
            KanseiD.SetActive(false);
            AutoRun.isCurrentModel = true;
        }
        public void onClickKanseiA() {
            Genkyo.SetActive(false);
            KanseiA.SetActive(true);
            KanseiB.SetActive(false);
            KanseiC.SetActive(false);
            KanseiD.SetActive(false);
            AutoRun.isCurrentModel = false;
        }

        public void onClickKanseiB() {
            Genkyo.SetActive(false);
            KanseiA.SetActive(false);
            KanseiB.SetActive(true);
            KanseiC.SetActive(false);
            KanseiD.SetActive(false);
            AutoRun.isCurrentModel = false;
        }

        public void onClickKanseiC() {
            Genkyo.SetActive(false);
            KanseiA.SetActive(false);
            KanseiB.SetActive(false);
            KanseiC.SetActive(true);
            KanseiD.SetActive(false);
            AutoRun.isCurrentModel = false;
        }

        public void onClickKanseiD() {
            Genkyo.SetActive(false);
            KanseiA.SetActive(false);
            KanseiB.SetActive(false);
            KanseiC.SetActive(false);
            KanseiD.SetActive(true);
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
    //                 Debug.Log("ObjectSwitch:現況");
    //                 Genkyo.SetActive(true);
    //                 Kansei.SetActive(false);
    //                 Count++;
    //                 AutoRun.isCurrentModel = true;
    //                 break;
    //             case 1:
    //                 //処理B
    //                 ObjectSwitchButtonText.text = "完成";
    //                 Debug.Log("ObjectSwitch:完成");
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
