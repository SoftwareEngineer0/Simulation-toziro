using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Kender.uGUI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;

namespace MyVR_Assets
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]

    public class VRCameraRig : MonoBehaviour
    {

        public GameObject OparateMainPanel;
        public GameObject TwoPointMeasurePanel;

        public enum DefaultCam
        {
            地上,
            上空,
        }
        public DefaultCam DefaultCamMode;

        public string CamMode = null;

        public Transform VerticalRotObj;
        public Transform MainCamera;

        public GameObject ShowLandingToggle;
        private Vector3 LandingPosition;
        public GameObject LandingPoint;

        private float distance;
        private float NewDistance;
        private float SkyDistanceMemory = 100f;
        public Slider ZoomSlider;
        public Slider HighOffsetSlider;
        public float HighOffset = 1.2f;

        public Slider RotSpeedSlider;
        public float RotSensitivity = 30.0f;
        public float RotX = 0.0f;
        public float RotY = 0.0f;
        private bool TiltResetFlagSky = false;
        private bool TiltResetFlagGround = false;
        private float TiltReset = 0.0f;
        private bool AutoRunActive = false;

        public Slider MoveSpeedSlider;
        public float PosSensitivity = 6.0f;
        bool PosYUpButton = false;
        bool PosYDownButton = false;
        public float PosX = 0.0f;
        public float PosY = 0.0f;
        public float PosZ = 0.0f;

        private float DistLagAdjuster;
        private Vector3 ResetPosition;
        private Vector3 ResetRotation;
        private Vector3 FirstPosition;
        private Vector3 ApointPosition;
        public bool ApointPosButton = false;
        public Text ApointPosMoveButtonText;

        public bool MousePosOverUI;
        public static bool isTextInput;

        public bool JoystickActive = false;
        public Joystick leftJoystick;
        public Joystick rightJoystick;

        void Awake()
        {
            CamMode = DefaultCamMode.ToString();
        }

        void Start()
        {

            if (CamMode == "地上")
            {
                Debug.Log("VRCineraRig_Start@@_地上");
                ZoomSlider.value = 0.0f;
                HighOffsetSlider.value = HighOffset;

                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<CapsuleCollider>().enabled = true;

                ShowLandingToggle.GetComponent<Toggle>().isOn = false;
                ShowLandingToggle.SetActive(false);
            }
            else if (CamMode == "上空")
            {
                Debug.Log("VRCineraRig_Start@@_上空");
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<CapsuleCollider>().enabled = false;
                //-------------------------Modify PROCW-----------------------------------
                //ShowLandingToggle.SetActive(true);
                ResetPosition = PlayerPosReset();
                //transform.eulerAngles = new Vector3(40.0f, 246.6f, 0.0f);
                transform.eulerAngles = new Vector3(0.0f, 246.6f, 0.0f);
                VerticalRotObj.localEulerAngles = new Vector3(40.0f, 0.0f, 0.0f);
                ResetRotation = VerticalRotObj.eulerAngles;
                Debug.Log("ResetRotation:" + ResetRotation);
                Debug.Log("Start_ResetPos:" + ResetPosition);
                ZoomSlider.value = Vector3.Distance(MainCamera.position, ResetPosition);
                Debug.Log("Start_Distance:" + ZoomSlider.value);
                //transform.position = new Vector3(108f, 118.3f, 82.4f);
                transform.position = new Vector3(0f, 0f, 0f);
                Debug.Log("MainCamera_Start:" + MainCamera.position);
                MainCamera.position = new Vector3(-18.64f, -22.5f, -345.4f);
                MainCamera.localEulerAngles = new Vector3(0f, 0f, 0f);
                HighOffsetSlider.value = HighOffset;
            }
        }

        //カメラモード選択ボタン
        public void CameraMode()
        {
            //-------------------------Modify PROCW-----------------------------------
            //ShowLandingToggle.SetActive(true);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<CapsuleCollider>().enabled = false;
            MainCamera.position = new Vector3(-18.64f, -22.5f, -345.4f);
            MainCamera.localEulerAngles = new Vector3(0f, 0f, 0f);
            //transform.position = new Vector3(108f, 118.3f, 82.4f);
            transform.position = new Vector3(0f, 0f, 0f);
            transform.eulerAngles = ResetRotation;
            VerticalRotObj.localEulerAngles = new Vector3(40.0f, 0.0f, 0.0f);
            ResetPosition = transform.position;
            ZoomSlider.value = 347.5f;
            Debug.Log("CameraMode:" + ZoomSlider.value + MainCamera.position);

            //TiltResetFlagSky = true;

            //上空モードに切り替えた時の処理,主に初期化
            // if (CamMode == "地上")
            // {
            //     if (!AutoRunActive)
            //     {
            //         CamMode = "上空";

            //         ShowLandingToggle.SetActive(true);
            //         HighOffset = HighOffsetSlider.value;
            //         GetComponent<Rigidbody>().isKinematic = true;
            //         GetComponent<CapsuleCollider>().enabled = false;

            //         TiltResetFlagSky = true;
            //         iTween.ValueTo(gameObject, iTween.Hash("from", distance, "to", SkyDistanceMemory, "time", 0.35f, "onupdate", "DistanceReset"));
            //         iTween.ValueTo(gameObject, iTween.Hash("from", HighOffsetSlider.value, "to", 0.0f, "time", 0.4f, "onupdate", "HighOffsetReset"));

            //         StartCoroutine("TiltResetFlagReset");
            //     }
            // }
            // //地上モードに切り替えた時の処理,主に初期化
            // else if (CamMode == "上空")
            // {
            //     CamMode = "地上";

            //     ShowLandingToggle.GetComponent<Toggle>().isOn = false;
            //     ShowLandingToggle.SetActive(false);
            //     SkyDistanceMemory = distance;
            //     ResetPosition = PlayerPosReset();
            //     ZoomSlider.value = Vector3.Distance(MainCamera.position, ResetPosition);
            //     transform.position = ResetPosition;

            //     TiltResetFlagGround = true;
            //     iTween.ValueTo(gameObject, iTween.Hash("from", distance, "to", 0.0f, "time", 0.35f, "onupdate", "DistanceReset"));
            //     iTween.ValueTo(gameObject, iTween.Hash("from", HighOffsetSlider.value, "to", HighOffset, "time", 0.4f, "onupdate", "HighOffsetReset"));

            //     StartCoroutine("TiltResetFlagReset");
            // }
        }

        private IEnumerator TiltResetFlagReset()
        {
            yield return new WaitForSeconds(0.5f);

            TiltResetFlagSky = false;
            TiltResetFlagGround = false;

            if (!AutoRunActive && CamMode == "地上")
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<CapsuleCollider>().enabled = true;
            }
        }
        //distanceリセット時の値を受け取るメソッド
        private void DistanceReset(float value)
        {
            ZoomSlider.value = value;
        }

        //HighOffsetリセット時の値を受け取るメソッド
        private void HighOffsetReset(float value)
        {
            HighOffsetSlider.value = value;
        }

        // playerポジションのリセットボタン
        public void TagetPosReset()
        {
            ResetPosition = PlayerPosReset();
            transform.position = ResetPosition;
        }

        // playerポジションのリセットを行うメソッド
        public Vector3 PlayerPosReset()
        {
            RaycastHit hit;
            Ray ray;

            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            //とりあえずdefaltレイヤーに設定
            var layerMask = 1 << 0;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                ResetPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
            return ResetPosition;
        }

        //指定座標へ移動ボタン
        public void ApointPosMoveButton()
        {
            if (ApointPosButton == true)
            {
                ApointPosButton = false;
            }
            else if (ApointPosButton == false)
            {
                ApointPosButton = true;
                ApointPosMoveButtonText.text = "任意の場所をクリック";
                ApointPosMoveButtonText.color = Color.red;
            }
        }

        //指定座標を返すメソッド
        static Vector3 ApointPosMove(Vector3 ApointPos)
        {
            RaycastHit hit;
            Ray ray;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //とりあえずdefaltレイヤーに設定
            var layerMask = 1 << 0;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                ApointPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }

            return ApointPos;
        }

        //マウスがUIにかぶっているかどうか
        private bool UpdateMouseOverUI;

        public void MousePosInUI()
        {
            UpdateMouseOverUI = true;
        }
        public void MousePosOutUI()
        {
            UpdateMouseOverUI = false;
        }
        void MouseOverUIUpdateCheck()
        {
            if (UpdateMouseOverUI && !Input.anyKey)
            {
                MousePosOverUI = true;
            }
            else if (!UpdateMouseOverUI && !Input.anyKey)
            {
                MousePosOverUI = false;
            }
        }

        //テキスト入力中
        public void TextInput(bool isInput)
        {
            isTextInput = isInput;
        }

        //上空カメラモード時のYupボタンがおされているかどうか
        public void PosYUpButtonPush(bool isPush)
        {
            PosYUpButton = isPush;
        }

        //上空カメラモード時のYdownボタンがおされているかどうか
        public void PosYDownButtonPush(bool isPush)
        {
            PosYDownButton = isPush;
        }

        //ジョイスティックが使用可能かどうか
        public void VirtualJoystickEnable()
        {
            JoystickActive = true;
        }

        public void VirtualJoystickDisable()
        {
            JoystickActive = false;
        }

        [HideInInspector]
        public bool CameraCtrlStopFlag = false;

        void Update()
        {
            MouseOverUIUpdateCheck();
            AutoRunActive = AutoRun.AutoRunActive;

            //共通入力
            if (CamMode != "2D")
            {
                Debug.Log("UPdate1");
                //キーボード移動入力
                if (!AutoRunActive || MousePosOverUI == false && !JoystickActive)
                {
                    Debug.Log("UPdate2");
                    if (!isTextInput)
                    {
                        Debug.Log("UPdate3");
                        PosX = Input.GetAxis("Horizontal") * PosSensitivity;
                        PosZ = Input.GetAxis("Vertical") * PosSensitivity;
                    }
                }
                //マウス回転入力
                if (MousePosOverUI == false && !JoystickActive)
                {
                    Debug.Log("UPdate4");
                    if (Input.GetMouseButton(0) || Input.GetMouseButton(2))
                    {
                        Debug.Log("UPdate5");//!!!
                        RotX += Input.GetAxis("Mouse X") * RotSensitivity;
                        RotY -= Input.GetAxis("Mouse Y") * RotSensitivity;

                        if (Input.GetMouseButton(0) && !AutoRunActive)
                        {
                            Debug.Log("UPdate6");//!!!
                            if (!CameraCtrlStopFlag)
                            {
                                Debug.Log("UPdate7");//!!!
                                if (!Input.GetMouseButton(1))
                                {
                                    Debug.Log("UPdate8");//!!!
                                    PosZ = 1.0f * PosSensitivity;
                                }
                                else if (Input.GetMouseButton(1))
                                {
                                    Debug.Log("UPdate9");
                                    PosZ = -1.0f * PosSensitivity;
                                }
                            }
                        }
                    }
                }

                //タッチパネル入力
                if (MousePosOverUI == false)
                {
                    Debug.Log("UPdate10");
                    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        Debug.Log("UPdate11");
                        // Get movement of the finger since last frame
                        Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                        RotX = touchDeltaPosition.x * RotSensitivity * 0.02f;
                        RotY = touchDeltaPosition.y * RotSensitivity * -0.015f;
                    }
                }

                //バーチャルジョイスティック入力
                if (JoystickActive)
                {
                    Debug.Log("UPdate12");
                    //移動
                    if (!AutoRunActive)
                    {
                        Debug.Log("UPdate13");
                        PosX = leftJoystick.virtualAxes.x * PosSensitivity;
                        PosZ = leftJoystick.virtualAxes.y * PosSensitivity;
                    }
                    //回転
                    RotX = rightJoystick.virtualAxes.x * Time.deltaTime * RotSensitivity * 20f;
                    RotY = rightJoystick.virtualAxes.y * Time.deltaTime * RotSensitivity * 10f * -1.0f;
                }

                distance = ZoomSlider.value;
                //Debug.Log("VRCameraRig_JoyStick_distance:" + distance);
                MainCamera.localPosition = new Vector3(-18.64f, -22.5f, distance * -1.0f);
                VerticalRotObj.localPosition = new Vector3(0f, HighOffsetSlider.value, 0f);
            }

            //カメラモードごとの入力
            if (CamMode == "地上")
            {
                //jump
                if (Input.GetButton("Jump"))
                {
                    Debug.Log("UPdate14");
                    //print("A");
                    GetComponent<Rigidbody>().AddForce(transform.up * Time.deltaTime * 1000f);
                }
            }
            else if (CamMode == "上空")
            {
                Debug.Log("UPdate15");
                //Debug.Log("VRCameraRig_CamMode:" + LandingPosition);
                //着地点の表示
                if (ShowLandingToggle.GetComponent<Toggle>().isOn)
                {
                    Debug.Log("UPdate16");
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        Debug.Log("UPdate17");
                        LandingPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        Debug.Log("VRCameraRig_CamMode1:" + LandingPosition);
                    }

                    Vector3 LandingPointScreenPos = Camera.main.WorldToScreenPoint(LandingPosition);
                    if (LandingPointScreenPos.z > 0.0f)
                    {
                        Debug.Log("UPdate18");
                        LandingPoint.transform.position = new Vector3(LandingPointScreenPos.x, LandingPointScreenPos.y, 0.0f);
                        Debug.Log("VRCameraRig_CamMode2:" + LandingPoint.transform.position);
                    }
                }
                Debug.Log("Update_CamMode_上空" + ZoomSlider.value);
                if (ZoomSlider.value < 0.0f)
                {
                    Debug.Log("UPdate19");
                    ZoomSlider.value = 0.0f;
                }

                ZoomSlider.value -= Input.GetAxis("Mouse ScrollWheel") * 0.4f * distance;

                //右クリックでの指定座標移動
                if (Input.GetMouseButtonDown(1) && !Input.GetMouseButton(0) && !MousePosOverUI)
                {
                    Debug.Log("UPdate20");
                    ApointPosition = ApointPosMove(ApointPosition);
                    Debug.Log("VRCameraRig_RightClick:" + ApointPosButton);
                    NewDistance = Vector3.Distance(MainCamera.position, ApointPosition);
                    iTween.MoveTo(this.gameObject, ApointPosition, 1.0f);
                    StartCoroutine("DistanceAdjust");
                }
                //ボタンでの指定座標移動
                if (Input.GetMouseButtonDown(0) && !MousePosOverUI && ApointPosButton)
                {
                    Debug.Log("UPdate21");
                    ApointPosition = ApointPosMove(ApointPosition);
                    Debug.Log("VRCameraRig_LeftClick:" + ApointPosButton);
                    NewDistance = Vector3.Distance(MainCamera.position, ApointPosition);
                    iTween.MoveTo(this.gameObject, ApointPosition, 1.0f);
                    StartCoroutine("DistanceAdjust");
                    ApointPosButton = false;
                    ApointPosMoveButtonText.text = "指定座標へ移動";
                    ApointPosMoveButtonText.color = Color.white;
                }

                //上空カメラモード時のYposの処理
                if (PosYUpButton && !PosYDownButton)
                {
                    Debug.Log("UPdate22");
                    PosY = 1f * PosSensitivity;
                }
                if (!PosYUpButton && PosYDownButton)
                {
                    Debug.Log("UPdate23");
                    PosY = -1f * PosSensitivity;
                }
                if (!PosYUpButton && !PosYDownButton)//!!!
                {
                    Debug.Log("UPdate24");
                    PosY = 0.0f;
                }
            }

            if (TiltResetFlagSky)
            {
                Debug.Log("UPdate25");
                TiltReset = Mathf.MoveTowardsAngle(VerticalRotObj.localEulerAngles.x, 40.0f, Time.deltaTime * 200f);
                VerticalRotObj.localEulerAngles = new Vector3(TiltReset, 0f, 0f);
            }
            else if (TiltResetFlagGround)
            {
                Debug.Log("UPdate26");
                TiltReset = Mathf.MoveTowardsAngle(VerticalRotObj.localEulerAngles.x, 0.0f, Time.deltaTime * 100f);
                VerticalRotObj.localEulerAngles = new Vector3(TiltReset, 0f, 0f);
            }

            //共通処理

            //調整
            DistLagAdjuster = (float)System.Math.Sqrt(distance) * 0.4f;
            if (DistLagAdjuster <= 1.0f)
            {
                MainCamera.localPosition = new Vector3(0f, 0f, 0f);
                Debug.Log("UPdate27");
                DistLagAdjuster = 1.0f;
            }

            //移動
            float MoveSpeed = MoveSpeedSlider.value;
            // transform.Translate((PosX * Time.deltaTime * DistLagAdjuster * MoveSpeed) / 3.6f,
            //                     (PosY * Time.deltaTime * DistLagAdjuster * MoveSpeed) / 3.6f,
            //                     (PosZ * Time.deltaTime * DistLagAdjuster * MoveSpeed) / 3.6f);

            //回転
            float RotateSpeed = RotSpeedSlider.value;
            transform.Rotate(0.0f, RotX * RotateSpeed, 0.0f);
            VerticalRotObj.transform.Rotate(RotY * RotateSpeed, 0.0f, 0.0f);

            //Rigidbodyのせいで余計な値が入ってしまう為値をリセット
            transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);

            //フレーム毎の値リセット
            StartCoroutine("InputValueReset");
        }

        private IEnumerator InputValueReset()
        {
            yield return null;

            PosX = 0.0f;
            PosY = 0.0f;
            PosZ = 0.0f;

            RotX = 0f;
            RotY = 0f;
        }

        //指定座標へ移動の時に起こるdistanceの不具合対策
        public IEnumerator DistanceAdjust()
        {
            yield return null;

            if (distance > NewDistance)
            {
                iTween.ValueTo(gameObject, iTween.Hash("from", distance, "to", NewDistance, "time", 0.4f, "onupdate", "DistanceReset"));
            }
        }
    }
}
