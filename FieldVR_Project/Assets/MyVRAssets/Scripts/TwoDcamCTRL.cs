using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MyVR_Assets
{
    public class TwoDcamCTRL : MonoBehaviour
    {
        private Camera TwoDCamera;
        public GameObject Player;
        private string CamModeMemory;

        public float PanMoveSensitivity;
        public float ScrollSensitivity;

        public GameObject ShowLandingToggle;
        public GameObject AutoRunPanel;
        public GameObject OperatePanelToggle;
        private bool OperatePanelToggleMemory;
        public GameObject JoystickToggle;
        private bool JoystickToggleMemory;
        public GameObject MiniMapToggle;
        private bool MiniMapToggleMemory;

        void Start()
        {
            TwoDCamera = this.GetComponent<Camera>();
        }

        public void TwoDCamStart()
        {
            //-------------------------Modify PROCW-----------------------------------
            //ShowLandingToggle.GetComponent<Toggle>().isOn = false;
            //ShowLandingToggle.SetActive(false);

            if (Player.GetComponent<VRCameraRig>().CamMode != "2D")
            {
                CamModeMemory = Player.GetComponent<VRCameraRig>().CamMode;

                Player.GetComponent<VRCameraRig>().CamMode = "2D";
                this.transform.position = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
                this.GetComponent<Camera>().enabled = true;

                OperatePanelToggleMemory = OperatePanelToggle.GetComponent<Toggle>().isOn;
                OperatePanelToggle.GetComponent<Toggle>().isOn = false;
                OperatePanelToggle.SetActive(false);

                JoystickToggleMemory = JoystickToggle.GetComponent<Toggle>().isOn;
                JoystickToggle.GetComponent<Toggle>().isOn = false;
                JoystickToggle.SetActive(false);

                MiniMapToggleMemory = MiniMapToggle.GetComponent<Toggle>().isOn;
                MiniMapToggle.GetComponent<Toggle>().isOn = false;
                MiniMapToggle.SetActive(false);

                if (AutoRun.AutoRunActive)
                {
                    AutoRunPanel.GetComponent<AutoRun>().AutoRunQuitButton();
                }
            }
        }

        public void TwoDCamEnd()
        {
            //-------------------------Modify PROCW-----------------------------------
            //ShowLandingToggle.SetActive(true);

            // if (Player.GetComponent<VRCameraRig>().CamMode == "2D")
            // {
            //     RaycastHit hit;
            //     Ray ray = TwoDCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            //     if (Physics.Raycast(TwoDCamera.transform.position, ray.direction, out hit, Mathf.Infinity))
            //     {
            //         Player.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            //     }

            //     if (CamModeMemory == "上空")
            //     {
            //         ShowLandingToggle.SetActive(true);
            //     }

            //     this.GetComponent<Camera>().enabled = false;
            //     Player.GetComponent<VRCameraRig>().CamMode = CamModeMemory;

            //     OperatePanelToggle.GetComponent<Toggle>().isOn = OperatePanelToggleMemory;
            //     OperatePanelToggle.SetActive(true);

            //     JoystickToggle.GetComponent<Toggle>().isOn = JoystickToggleMemory;
            //     JoystickToggle.SetActive(true);

            //     MiniMapToggle.GetComponent<Toggle>().isOn = MiniMapToggleMemory;
            //     MiniMapToggle.SetActive(true);
            // }
        }

        private Vector3 LastPosition;

        void Update()
        {
            if (Player.GetComponent<VRCameraRig>().CamMode == "2D")
            {
                float MoveSensitivity = this.GetComponent<Camera>().orthographicSize * (PanMoveSensitivity / 1000f);

                if (Input.GetMouseButtonDown(2))
                {
                    LastPosition = Input.mousePosition;
                }

                if (Input.GetMouseButton(2))
                {
                    Vector3 delta = Input.mousePosition - LastPosition;
                    transform.Translate(delta.x * MoveSensitivity * -1.0f, delta.y * MoveSensitivity * -1.0f, 0);
                    LastPosition = Input.mousePosition;
                }

                this.GetComponent<Camera>().orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * (ScrollSensitivity * this.GetComponent<Camera>().orthographicSize);

                if (this.GetComponent<Camera>().orthographicSize < 1.0f)
                {
                    this.GetComponent<Camera>().orthographicSize = 1.0f;
                }
            }
        }
    }
}
