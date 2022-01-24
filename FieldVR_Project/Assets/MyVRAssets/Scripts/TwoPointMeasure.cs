using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MyVR_Assets
{
    public class TwoPointMeasure : MonoBehaviour
    {

        public bool ActiveMeasure = false;
        public GameObject Player;
        public Camera _2DCamera;
        public Camera _3DCamera;

        public Text ButtonText;
        public Text InfomationText;

        public GameObject Point01;
        public GameObject Point02;

        public int Count = 0;
        public float TwoPoinDist = 0.0f;

        private Vector3 Pos01;
        private Vector3 Pos02;
        private Vector3 MiddlePos;

        private GameObject Line;
        public Material LineMaterial;
        public GameObject TwoPointMeasureText;

        void Start()
        {

            ActiveMeasure = false;
            Point01.SetActive(false);
            Point02.SetActive(false);
            TwoPointMeasureText.SetActive(false);

            ButtonText.text = "2点間計測";
            InfomationText.text = "計測を開始できます";
        }

        public void TwoPointMeasureButton()
        {

            if (ActiveMeasure)
            {
                ActiveMeasure = false;

                Player.GetComponent<VRCameraRig>().CameraCtrlStopFlag = false;
                ButtonText.text = "2点間計測";
                ButtonText.color = Color.white;
                InfomationText.text = "計測を開始できます";
            }
            else if (!ActiveMeasure)
            {
                ActiveMeasure = true;

                Player.GetComponent<VRCameraRig>().CameraCtrlStopFlag = true;
                ButtonText.text = "計測中";
                ButtonText.color = Color.red;
                InfomationText.text = "1点目をクリック";

                Line = new GameObject();
                Line.AddComponent<LineRenderer>();
                Line.GetComponent<LineRenderer>().material = LineMaterial;
                Line.GetComponent<LineRenderer>().SetWidth(0.0f, 0.0f);
            }
        }

        void Update()
        {
            if (ActiveMeasure)
            {
                if (!Player.GetComponent<VRCameraRig>().MousePosOverUI)
                {
                    if (Player.GetComponent<VRCameraRig>().CamMode != "2D")
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Ray ray = _3DCamera.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit = new RaycastHit();

                            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                            {
                                Count++;
                                Vector3 ClickPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                                if (Count == 1)
                                {
                                    Pos01 = ClickPosition;
                                    Point01.SetActive(true);

                                    InfomationText.text = "2点目をクリック";
                                }
                                else if (Count == 2)
                                {
                                    Pos02 = ClickPosition;
                                    Point02.SetActive(true);
                                    MiddlePos = new Vector3((Pos01.x + Pos02.x) / 2, (Pos01.y + Pos02.y) / 2, (Pos01.z + Pos02.z) / 2);
                                    Line.GetComponent<LineRenderer>().SetPosition(0, Pos01);
                                    Line.GetComponent<LineRenderer>().SetPosition(1, Pos02);
                                    TwoPoinDist = Vector3.Distance(Pos01, Pos02);

                                    ButtonText.text = "計測地点をリセット";
                                    ButtonText.color = Color.white;
                                    InfomationText.text = "約:" + TwoPoinDist.ToString("f3") + "m";
                                    TwoPointMeasureText.GetComponent<Text>().text = "約:" + TwoPoinDist.ToString("f3") + "m";
                                    TwoPointMeasureText.SetActive(true);
                                }
                                else if (Count >= 3)
                                {
                                    Count = 3;
                                    Player.GetComponent<VRCameraRig>().CameraCtrlStopFlag = false;
                                }
                            }
                        }
                    }

                    if (Player.GetComponent<VRCameraRig>().CamMode == "2D")
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Ray ray = _2DCamera.ScreenPointToRay(Input.mousePosition);
                            Vector3 ClickPositin2D = _2DCamera.ScreenToWorldPoint(Input.mousePosition);
                            RaycastHit hit = new RaycastHit();

                            if (Physics.Raycast(ClickPositin2D, ray.direction, out hit, Mathf.Infinity))
                            {
                                Count++;
                                Vector3 ClickPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                                if (Count == 1)
                                {
                                    Pos01 = ClickPosition;
                                    Point01.SetActive(true);

                                    InfomationText.text = "2点目をクリック";
                                }
                                else if (Count == 2)
                                {
                                    Pos02 = ClickPosition;
                                    Point02.SetActive(true);
                                    MiddlePos = new Vector3((Pos01.x + Pos02.x) / 2, (Pos01.y + Pos02.y) / 2, (Pos01.z + Pos02.z) / 2);
                                    Line.GetComponent<LineRenderer>().SetPosition(0, Pos01);
                                    Line.GetComponent<LineRenderer>().SetPosition(1, Pos02);
                                    TwoPoinDist = Vector3.Distance(Pos01, Pos02);

                                    ButtonText.text = "計測地点をリセット";
                                    ButtonText.color = Color.white;
                                    InfomationText.text = "約:" + TwoPoinDist.ToString("f3") + "m";
                                    TwoPointMeasureText.GetComponent<Text>().text = "約:" + TwoPoinDist.ToString("f3") + "m";
                                    TwoPointMeasureText.SetActive(true);
                                }
                                else if (Count >= 3)
                                {
                                    Count = 3;
                                    Player.GetComponent<VRCameraRig>().CameraCtrlStopFlag = false;
                                }
                            }
                        }
                    }
                }
                //3D座標をスクリーン座標に変換
                if (Player.GetComponent<VRCameraRig>().CamMode != "2D")
                {
                    Vector3 Point01SCpos = _3DCamera.WorldToScreenPoint(Pos01);
                    if (Point01SCpos.z > 0.0f)
                    {
                        Point01.transform.position = new Vector3(Point01SCpos.x, Point01SCpos.y, 0.0f);
                    }

                    Vector3 Point02SCpos = _3DCamera.WorldToScreenPoint(Pos02);
                    if (Point02SCpos.z > 0.0f)
                    {
                        Point02.transform.position = new Vector3(Point02SCpos.x, Point02SCpos.y, 0.0f);
                    }

                    Vector3 MiddlePointSCpos = _3DCamera.WorldToScreenPoint(MiddlePos);
                    if (MiddlePointSCpos.z > 0.0f)
                    {
                        TwoPointMeasureText.transform.position = new Vector3(MiddlePointSCpos.x, MiddlePointSCpos.y, 0.0f);
                    }

                    if (Count >= 2)
                    {
                        float CameraToMiddlePosDist = Vector3.Distance(MiddlePos, _3DCamera.transform.position);
                        Line.GetComponent<LineRenderer>().SetWidth(CameraToMiddlePosDist * 0.003f, CameraToMiddlePosDist * 0.003f);
                    }
                }
                else if (Player.GetComponent<VRCameraRig>().CamMode == "2D")
                {
                    Vector3 Point01SCpos = _2DCamera.WorldToScreenPoint(Pos01);
                    if (Point01SCpos.z > 0.0f)
                    {
                        Point01.transform.position = new Vector3(Point01SCpos.x, Point01SCpos.y, 0.0f);
                    }

                    Vector3 Point02SCpos = _2DCamera.WorldToScreenPoint(Pos02);
                    if (Point02SCpos.z > 0.0f)
                    {
                        Point02.transform.position = new Vector3(Point02SCpos.x, Point02SCpos.y, 0.0f);
                    }

                    Vector3 MiddlePointSCpos = _2DCamera.WorldToScreenPoint(MiddlePos);
                    if (MiddlePointSCpos.z > 0.0f)
                    {
                        TwoPointMeasureText.transform.position = new Vector3(MiddlePointSCpos.x, MiddlePointSCpos.y, 0.0f);
                    }

                    if (Count >= 2)
                    {
                        float OrthographicSize = _2DCamera.GetComponent<Camera>().orthographicSize;
                        Line.GetComponent<LineRenderer>().SetWidth(OrthographicSize * 0.01f, OrthographicSize * 0.01f);
                    }
                }
            }
            else if (!ActiveMeasure)
            {
                Count = 0;
                TwoPoinDist = 0.0f;
                Point01.SetActive(false);
                Point02.SetActive(false);
                TwoPointMeasureText.SetActive(false);
                Pos01 = new Vector3(0.0f, 0.0f, 0.0f);
                Pos02 = new Vector3(0.0f, 0.0f, 0.0f);
                MiddlePos = new Vector3(0.0f, 0.0f, 0.0f);
                Destroy(Line);
            }
        }
    }
}