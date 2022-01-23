#if UNITY_EDITOR
#if UNITY_STANDALONE_WIN

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics;

namespace GPS_Photo
{
    public class ThumbnailCTRL : MonoBehaviour
    {
        public Vector3 SelfPosition;
        private GameObject _3DCamera;

        public string[] LatLong = new string[2];
        public Vector3 XYZ;

        void Start()
        {
            _3DCamera = GameObject.Find("3DCamera");
        }

        void Update()
        {
		    Vector3 ViewportPos = _3DCamera.GetComponent<Camera>().WorldToScreenPoint(SelfPosition);

		    if (ViewportPos.z > 0.0f)
            {
                this.GetComponent<Image>().enabled = true;
                this.transform.Find("ThumbnailRawImage").gameObject.SetActive(true);
                this.transform.position = new Vector3(ViewportPos.x, ViewportPos.y, 0.0f);
            }
            else
            {
                this.GetComponent<Image>().enabled = false;
                this.transform.Find("ThumbnailRawImage").gameObject.SetActive(false);
            } 
	    }

        public void Clicked()
        {
            string FileName = this.gameObject.name;

            Process.Start(Application.streamingAssetsPath + "/GPS_Photo/" + FileName);
        }

        public void OnMouse()
        {
            GameObject PhotoInfo = GameObject.Find("PhotoInfo");

            PhotoInfo.GetComponent<Text>().text = this.gameObject.name +
                                                  "\n" +
                                                  "緯度(ddmmss.s)" +
                                                  "\n" + 
                                                  LatLong[0].Substring(0, 12) +
                                                  "\n" +
                                                  "経度(dddmmss.s)" +
                                                  "\n" + 
                                                  LatLong[1].Substring(0, 12) +
                                                  "\n" +
                                                  "\n" + "平面直角座標系第2系" +
                                                  "\n" + "X:" + XYZ.x.ToString("f2") +
                                                  "\n" + "Y:" + XYZ.y.ToString("f2") +
                                                  "\n" + "Z:" + XYZ.z.ToString("f2");
        }

        public void OutMouse()
        {
            GameObject PhotoInfo = GameObject.Find("PhotoInfo");

            PhotoInfo.GetComponent<Text>().text = "マウスポインタの重なった写真ファイルの情報を表示します";
        }
    }
}
#endif
#endif