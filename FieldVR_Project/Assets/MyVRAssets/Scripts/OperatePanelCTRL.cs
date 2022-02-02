using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Kender.uGUI;

namespace MyVR_Assets
{
    public class OperatePanelCTRL : MonoBehaviour
    {
        public GameObject Player;
        public string CamMode;

        public Toggle HelpToggle;
        public Toggle FullScreenToggle;

        public GameObject SubPanelSky;
        //public GameObject HelpPanel;
        //public GameObject HelpText;

        public GameObject SubPanelGround;
        public Text MoveSpeedMeterText;
        private Vector3 prePosition; // １つ前のフレームの位置
        public InputField HighOffsetInputField;

        public GameObject AutoRunPanel;

        public GameObject ScreenShotButton;
        public bool GPSPhoto_isActive;
        public GameObject GPSPhoto_Panel;

        void Start()
        {
            CamMode = Player.GetComponent<VRCameraRig>().CamMode;

            if (CamMode == "地上")
            {
                //-------------------------Modify PROCW-----------------------------------
                //SubPanelSky.SetActive(false);
                //SubPanelGround.SetActive(true);
            }
            else if (CamMode == "上空")
            {
                //-------------------------Modify PROCW-----------------------------------
                //SubPanelSky.SetActive(true);
                SubPanelGround.SetActive(false);
            }

            //HelpPanel.GetComponent<CanvasRenderer>().SetAlpha(0f);
            //HelpText.GetComponent<CanvasRenderer>().SetAlpha(0f);

            prePosition = Player.transform.position; // 今の位置を入れておく

            HighOffsetInputField.text = Player.GetComponent<VRCameraRig>().HighOffset.ToString();

            FullScreenToggle.isOn = true;

            if(Application.platform == RuntimePlatform.WindowsEditor)
            {
                //ScreenShotButton.SetActive(true);
                GPSPhoto_Panel.SetActive(true);
            }
            else if(Application.platform == RuntimePlatform.WindowsPlayer)
            {
                //ScreenShotButton.SetActive(true);
                GPSPhoto_Panel.SetActive(true);
            }
            else
            {
                //ScreenShotButton.SetActive(false);
                GPSPhoto_Panel.SetActive(false);
            }

        }

        //distanceリセット時の値を受け取るメソッド
        private void DistanceReset(float value)
        {
            Player.GetComponent<VRCameraRig>().ZoomSlider.value = value;
        }

        //フルスクリーン
        public void FullScreen()
        {
            if (FullScreenToggle.isOn)
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            }
            else if (!FullScreenToggle.isOn)
            {
                Screen.SetResolution(Screen.currentResolution.width - 30, Screen.currentResolution.height - 50, false);
            }
        }

        //HighOffset
        public void HighOffsetValueChanged()
        {
            Player.GetComponent<VRCameraRig>().HighOffsetSlider.value = float.Parse(HighOffsetInputField.text);
        }
        public void HighOffsetSliderValueChanged()
        {
            HighOffsetInputField.text = Player.GetComponent<VRCameraRig>().HighOffsetSlider.value.ToString();
        }

        //パネルの閉じるボタン

        public void CloseButton(Toggle Toggles)
        {
            Toggles.isOn = false;
        }

        public void GPSPhotoPanel_isActive(bool isActive)
        {
            if (!GPSPhoto_isActive && isActive)
            {
                GPSPhoto_Panel.SetActive(false);
            }
        }

        void Update()
        {
            CamMode = Player.GetComponent<VRCameraRig>().CamMode;

            //if (CamMode == "上空")
            //{
                //-------------------------Modify PROCW-----------------------------------
                //SubPanelSky.SetActive(true);
                //SubPanelGround.SetActive(false);
                //-------------------------Modify PROCW-----------------------------------
                // GameObject ButtonTexGround = GameObject.Find("ButtonTexGround");
                // ButtonTexGround.GetComponent<Text>().color = Color.white;

                // GameObject ButtonTextSky = GameObject.Find("ButtonTextSky");
                // ButtonTextSky.GetComponent<Text>().color = Color.red;

                // GameObject ButtonTextTwoD = GameObject.Find("ButtonTextTwoD");
                // ButtonTextTwoD.GetComponent<Text>().color = Color.white;

            //}
            //else if (CamMode == "地上")
            //{
                //-------------------------Modify PROCW-----------------------------------
                //SubPanelSky.SetActive(false);
                SubPanelGround.SetActive(true);

            //     GameObject ButtonTexGround = GameObject.Find("ButtonTexGround");
            //     ButtonTexGround.GetComponent<Text>().color = Color.red;

            //     GameObject ButtonTextSky = GameObject.Find("ButtonTextSky");
            //     ButtonTextSky.GetComponent<Text>().color = Color.white;

            //     GameObject ButtonTextTwoD = GameObject.Find("ButtonTextTwoD");
            //     ButtonTextTwoD.GetComponent<Text>().color = Color.white;
            // }
            // else
            // {
            //     GameObject ButtonTexGround = GameObject.Find("ButtonTexGround");
            //     ButtonTexGround.GetComponent<Text>().color = Color.white;

            //     GameObject ButtonTextSky = GameObject.Find("ButtonTextSky");
            //     ButtonTextSky.GetComponent<Text>().color = Color.white;

            //     GameObject ButtonTextTwoD = GameObject.Find("ButtonTextTwoD");
            //     ButtonTextTwoD.GetComponent<Text>().color = Color.red;
            // }

            //スピードメーター

            // 現在の位置と1つ前のフレームの位置で移動した距離を求める
            // 1÷１つ前のフレームからの経過時間を掛けることで1秒当たりの移動量が算出できる
            float moveDistance = Vector3.Distance(Player.transform.position, prePosition) * (1.0f / Time.deltaTime);
            
            MoveSpeedMeterText.text = (moveDistance * 3.6f).ToString("f0") + "km/h";
            ////Debug.Log("MoveSpeed:"+MoveSpeedMeterText.text);
            // 次のフレームで1つ前のフレームの位置として使う為現在の位置を保存
            prePosition = Player.transform.position;
        }

        float PopUpTimer = 1.0f;
        float alpha = 0.0f;

        void FixedUpdate()
        {
            //ヘルプポップアップ
            if (HelpToggle.isOn && !Player.GetComponent<VRCameraRig>().MousePosOverUI && !Input.anyKey && Input.GetAxis("Mouse X") == 0f && Input.GetAxis("Mouse Y") == 0f)
            {
                PopUpTimer -= Time.deltaTime;

                if (PopUpTimer <= 0.0f)
                {
                    alpha += Time.deltaTime * 2.5f;
                    //HelpPanel.GetComponent<CanvasRenderer>().SetAlpha(alpha);
                    //HelpText.GetComponent<CanvasRenderer>().SetAlpha(alpha);
                    //HelpPanel.transform.position = new Vector3(Input.mousePosition.x + 20f, Input.mousePosition.y + 20f, 0f);

                    PopUpTimer = 0.0f;
                }
            }
            else if (!HelpToggle.isOn || Player.GetComponent<VRCameraRig>().MousePosOverUI || Input.anyKey || Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f)
            {
                alpha = 0.0f;
                //HelpPanel.GetComponent<CanvasRenderer>().SetAlpha(0f);
                //HelpText.GetComponent<CanvasRenderer>().SetAlpha(0f);
                PopUpTimer = 1.0f;
            }
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
