using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MyVR_Assets
{
    public class HighPosInfo : MonoBehaviour
    {
        public GameObject Player;
        public Camera ThreeDCamera;
        public Camera TwoDCamera;
        public GameObject InfoObject;
        public Text InfoText;

        private bool HighPosInfoActive;

        Ray ray;
        RaycastHit hit;

        void Update()
        {
            HighPosInfoActive = this.GetComponent<Toggle>().isOn;

            if (HighPosInfoActive)
            {
                InfoObject.SetActive(true);
            }
            else if (!HighPosInfoActive)
            {
                InfoObject.SetActive(false);
            }
        }


        void LateUpdate()
        {
            bool MousePosOverUI = Player.GetComponent<VRCameraRig>().MousePosOverUI;

            if (!MousePosOverUI && HighPosInfoActive)
            {
                if (Player.GetComponent<VRCameraRig>().CamMode != "2D")
                {
                    ray = ThreeDCamera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        Vector3 SCpoint = ThreeDCamera.WorldToScreenPoint(hit.point);

                        InfoObject.transform.position = new Vector3(SCpoint.x, SCpoint.y, 0.0f);

                        InfoText.text = hit.point.y.ToString("f2") + "m";
                    }
                }
                else if (Player.GetComponent<VRCameraRig>().CamMode == "2D")
                {
                    ray = TwoDCamera.ScreenPointToRay(Input.mousePosition);
                    Vector3 ClickPositin2D = TwoDCamera.ScreenToWorldPoint(Input.mousePosition);

                    if (Physics.Raycast(ClickPositin2D, ray.direction, out hit, Mathf.Infinity))
                    {
                        Vector3 SCpoint = TwoDCamera.WorldToScreenPoint(hit.point);

                        InfoObject.transform.position = new Vector3(SCpoint.x, SCpoint.y, 0.0f);

                        InfoText.text = hit.point.y.ToString("f2") + "m";
                    }
                }
            }
            else if (MousePosOverUI)
            {
                InfoObject.SetActive(false);
            }

        }
    }
}
