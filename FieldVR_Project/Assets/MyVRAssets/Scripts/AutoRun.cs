using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kender.uGUI;

namespace MyVR_Assets
{
    public class AutoRun : SingletonMonoBehaviour<AutoRun>
    {
        public GameObject Player;
        public Transform VerticalRotObj;

        static public bool AutoRunActive = false;
        static public bool isCurrentModel;
        private bool AutoRunPauseFlag = false;
        private bool AutoRunRotReset = false;
        public GameObject AutoRunComboBox;
        private int AutoRunSelectedIndex;
        private GameObject GetAutoRun = null;
        private Animator GetAutoRunAnim = null;
        public Slider AutoRunSpeedSlider;
        public Text AutoRunSpeedText;
        public GameObject[] AutoRunObject;
        public GameObject AutoRunCtrlPanel;
        public Text AutoRunPauseButtonText;


        void Start()
        {
            isCurrentModel = true;
            AutoRunCtrlPanel.SetActive(false);
        }

        private int AutoRunSelectClickCount = 0;
        float PauseSpeed;

        public void AutoRunSelect()
        {
            //Debug.Log("AutoRunSelect1");
            if(isCurrentModel) {
                AutoRunComboBox.GetComponent<ComboBox>().isCurrentModel = true;
                //AutoRunComboBox.GetComponent<ComboBox>().Items[1].IsDisabled = false;
                //AutoRunComboBox.GetComponent<ComboBox>().Items[2].IsDisabled = false;
                //AutoRunComboBox.GetComponent<ComboBox>().Items[3].IsDisabled = true;
                //AutoRunComboBox.GetComponent<ComboBox>().Items[4].IsDisabled = true;
            }
            else {
                //AutoRunComboBox.GetComponent<ComboBox>().isCurrentModel = false;
                //AutoRunComboBox.GetComponent<ComboBox>().Items[1].IsDisabled = true;
                //AutoRunComboBox.GetComponent<ComboBox>().Items[2].IsDisabled = true;
                //AutoRunComboBox.GetComponent<ComboBox>().Items[3].IsDisabled = false;
                //AutoRunComboBox.GetComponent<ComboBox>().Items[4].IsDisabled = false;
            }
            if (AutoRunActive)
            {
                //Debug.Log("AutoRunSelect2_AutoRunQuitButton");
                AutoRunQuitButton();
            }

            //Debug.Log("AutoRunSelect3");
            //セレクトボックスを開くため
            AutoRunSelectClickCount++;

            if (AutoRunSelectClickCount >= 2)
            {
                //Debug.Log("AutoRunSelect4");
                Player.GetComponent<Rigidbody>().isKinematic = true;
                Player.GetComponent<CapsuleCollider>().enabled = false;
                //ComboBoxの処理待ち
                StartCoroutine("AutoRunSelectAction");
            }
        }

        private IEnumerator AutoRunSelectAction()
        {
            yield return null;
            AutoRunSelectedIndex = AutoRunComboBox.GetComponent<ComboBox>().SelectedIndex;
            //Debug.Log("AutoRunSelectedIndex:" + AutoRunSelectedIndex);
            GetAutoRun = AutoRunObject[AutoRunSelectedIndex - 1];
            //Debug.Log("AutoRun_GetAutoRun:" + GetAutoRun.transform.position);
            AutoRunActive = true;

            if (Player.GetComponent<VRCameraRig>().CamMode == "上空")
            {
                //-------------------------Modify PROCW-----------------------------------
                //Player.GetComponent<VRCameraRig>().CamMode = "地上";
                Player.GetComponent<VRCameraRig>().ZoomSlider.value = 0.0f;
            }
            //オートランスタート地点へ移動
            Player.transform.position = GetAutoRun.transform.position;
            //オートランスタート地点へ回転
            Player.transform.parent = GetAutoRun.transform;
            VerticalRotObj.localEulerAngles = new Vector3(0f, 0f, 0f);
            Player.transform.localEulerAngles = new Vector3(0f, -90f, 0f);

            GetAutoRunAnim = GetAutoRun.GetComponent<Animator>();
            AutoRunSelectClickCount = 0;
            StartCoroutine("AutoRunStart");
        }

        private IEnumerator AutoRunStart()
        {
            Debug.Log("AutoRunStart");
            AutoRunCtrlPanel.SetActive(true);
            GetAutoRunAnim.SetTrigger("Run");
            AutoRunRotReset = true;
            yield return new WaitForSeconds(0.5f);
            if(!AnimationCar.Instance.getStatusGeneration()) {
                AnimationCar.Instance.CarPlay();
            }
            
        }

        //AutoRun一時停止ボタン
        public void AutoRunPauseButton()
        {
            if (AutoRunPauseFlag)
            {
                AutoRunPauseFlag = false;
                AutoRunSpeedSlider.value = PauseSpeed;
                AutoRunSpeedSlider.interactable = true;
                AutoRunPauseButtonText.color = Color.white;
            }
            else if (!AutoRunPauseFlag)
            {
                AutoRunPauseFlag = true;
                PauseSpeed = AutoRunSpeedSlider.value;
                AutoRunSpeedSlider.value = 0.0f;
                AutoRunSpeedSlider.interactable = false;
                AutoRunPauseButtonText.color = Color.red;
            }
        }

        //AutoRun終了ボタン
        public void AutoRunQuitButton()
        {
            //Debug.Log("Onclick_AutoRunQuitButton:");
            Player.transform.parent = GameObject.Find("VR_Rig").transform;
            Player.GetComponent<Rigidbody>().isKinematic = false;
            Player.GetComponent<CapsuleCollider>().enabled = true;
            AutoRunComboBox.GetComponent<ComboBox>().SelectedIndex = 0;
            StartCoroutine("AutoRunReset");
            AutoRunRotReset = false;
            AutoRunCtrlPanel.SetActive(false);
            AutoRunPauseFlag = false;
            AutoRunSpeedSlider.value = 0.30f;
            AutoRunSpeedSlider.interactable = true;
            AutoRunPauseButtonText.color = Color.white;
            AutoRunActive = false;
        }

        private IEnumerator AutoRunReset()
        {
            yield return null;
            Debug.Log("AutoRunReset");
            GetAutoRunAnim.SetTrigger("Idle");
            GetAutoRun = null;
            AutoRunActive = false;
        }
        public bool getAutoRunStatus()
        {
            if(AutoRunActive)
                return true;
            else
                return false;
        }

        void Update ()
        {
            if (AutoRunRotReset && !Input.anyKey)
            {
                float AutoRunRuntimeRotXReset = Mathf.MoveTowardsAngle(VerticalRotObj.transform.localEulerAngles.x, 0f, Time.deltaTime * 100f);
                float AutoRunRuntimeRotYReset = Mathf.MoveTowardsAngle(Player.transform.localEulerAngles.y, -90f, Time.deltaTime * 100f);

                VerticalRotObj.localEulerAngles = new Vector3(AutoRunRuntimeRotXReset, 0f, 0f);
                Player.transform.localEulerAngles = new Vector3(0f, AutoRunRuntimeRotYReset, 0f);
            }

            if (AutoRunActive)
            {
                //Debug.Log("AutoRunActive!!!" + AutoRunSpeedSlider.value);
                GetAutoRunAnim.SetFloat("Speed", AutoRunSpeedSlider.value/3);

                float SpeedText = AutoRunSpeedSlider.value * 100f;
                AutoRunSpeedText.text = SpeedText.ToString("f0") + "km";
            }
            else if (!AutoRunActive)
            {
                AutoRunSpeedText.text = null;
            }
            if((GetAutoRunAnim != null) && (GetAutoRunAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") == true)) {
                Debug.Log("Success!");
                if(AnimationCar.Instance.getStatusGeneration()) {
                    AnimationCar.Instance.cloneCarAnimation["CarAnim_A"].normalizedTime = 0f;
                    
                }
            }
        }
    }
}
