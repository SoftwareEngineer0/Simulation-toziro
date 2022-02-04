using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kender.uGUI;

namespace MyVR_Assets
{
    [System.Serializable]
    public struct FixPoint
    {
        public Transform Point;
        public string CamMode;
    }

    public class FixPointMove : MonoBehaviour
    {
        public GameObject Player;
        public Transform VerticalRotObj;
        public GameObject AutoRunPanel;

        public GameObject FixPointComboBox;
        private int FixPointSelectedIndex;
        private Transform GetFixPoint;
        public FixPoint[] FixPoint;

        private int FixPointSelectClickCount = 0;

        public void FixPointSelect()
        {
            if (AutoRun.AutoRunActive)
            {
                AutoRunPanel.GetComponent<AutoRun>().AutoRunQuitButton();
            }

            //セレクトボックスを開くため
            FixPointSelectClickCount++;

            if (FixPointSelectClickCount >= 2)
            {
                Player.GetComponent<Rigidbody>().isKinematic = true;
                Player.GetComponent<CapsuleCollider>().enabled = false;
                //ComboBoxの処理待ち
                StartCoroutine("FixPointSelectAction");
            }
        }
        //StartCoroutine("FixPointSelectAction")の処理
        private IEnumerator FixPointSelectAction()
        {
            yield return 0;
            FixPointSelectedIndex = FixPointComboBox.GetComponent<ComboBox>().SelectedIndex;
            GetFixPoint = FixPoint[FixPointSelectedIndex - 1].Point;

            //定点へ移動
            //Debug.Log("FixPointMove:" + GetFixPoint.position);
            Player.transform.position = GetFixPoint.position;
            //定点の回転
            VerticalRotObj.localEulerAngles = new Vector3(GetFixPoint.localEulerAngles.x, 0f, 0f);
            Player.transform.localEulerAngles = new Vector3(0f, GetFixPoint.localEulerAngles.y, 0f);

            if (FixPoint[FixPointSelectedIndex - 1].CamMode == "地上")
            {
                Player.GetComponent<VRCameraRig>().CamMode = "地上";
                Player.GetComponent<VRCameraRig>().ZoomSlider.value = 0.0f;
                //Player.GetComponent<VRCameraRig>().HighOffsetSlider.value = 1.6f;

                Player.GetComponent<Rigidbody>().isKinematic = false;
                Player.GetComponent<CapsuleCollider>().enabled = true;
            }
            else if (FixPoint[FixPointSelectedIndex - 1].CamMode == "上空")
            {
                Player.GetComponent<VRCameraRig>().CamMode = "上空";

                Player.GetComponent<Rigidbody>().isKinematic = true;
                Player.GetComponent<CapsuleCollider>().enabled = false;

                Player.GetComponent<VRCameraRig>().ZoomSlider.value = 0.0f;
                Player.GetComponent<VRCameraRig>().HighOffsetSlider.value = 0.0f;

                StartCoroutine("FixPointSkyPositionReset");
            }

            StartCoroutine("FixPointSelectReset");
            FixPointSelectClickCount = 0;
        }

        private IEnumerator FixPointSelectReset()
        {
            yield return null;
            FixPointComboBox.GetComponent<ComboBox>().SelectedIndex = 0;
        }

        private IEnumerator FixPointSkyPositionReset()
        {
            yield return null;
            Vector3 ResetPosition = Player.GetComponent<VRCameraRig>().PlayerPosReset();
            print(ResetPosition);
            Player.GetComponent<VRCameraRig>().ZoomSlider.value = Vector3.Distance(Camera.main.transform.position, ResetPosition);
            //Debug.Log("FixPointMovie_FixPointSkyPositionReset:" + ResetPosition);
            Player.transform.position = ResetPosition;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!Player.GetComponent<VRCameraRig>().MousePosOverUI)
            {
                FixPointSelectClickCount = 0;
            }
        }
    }
}
