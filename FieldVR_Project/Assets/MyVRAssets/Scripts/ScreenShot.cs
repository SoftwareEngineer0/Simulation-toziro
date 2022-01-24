using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

namespace MyVR_Assets
{
    public class ScreenShot : MonoBehaviour
    {

        public GameObject UI;
        public Text SuperSizeText;

        int ShotCount;
        string path;
        string SavePath;
        int SuperSize = 1;

        bool isFileComplete = false;

        void Start()
        {
            System.IO.Directory.CreateDirectory("ScreenShot");
            path = Path.GetDirectoryName(Application.dataPath);
        }

        public void screenshot()
        {
            SuperSizeText.text = "保存中";
            SuperSizeText.color = Color.red;

            ShotCount++;
            StartCoroutine("UISwitch");
        }

        private IEnumerator UISwitch()
        {
            yield return null;

            UI.GetComponent<Canvas>().enabled = false;

            yield return new WaitForEndOfFrame();

            SavePath = path + "/ScreenShot/ScreenShot" + ShotCount.ToString("000") + ".jpg";
            ScreenCapture.CaptureScreenshot(SavePath, SuperSize);


            // ファイルが存在していて且つアクセス可能になったらファイルの保存処理が終了している.
            while (!isFileComplete)
            {
                //Debug.Log("Saving !! ");
                yield return new WaitForEndOfFrame();

                bool isFileExists = File.Exists(SavePath);
                bool isFileLocked = false; ;
                FileStream stream = null;
                try
                {
                    stream = new FileStream(SavePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch
                {
                    Debug.Log("ファイルがロックされているか又は開けない.");
                    isFileLocked = true;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }


                isFileComplete = (isFileExists == true && isFileLocked != true) ? true : false;
            }


            yield return new WaitForEndOfFrame();

            UI.GetComponent<Canvas>().enabled = true;
            SuperSizeText.text = "保存完了";
            isFileComplete = false;
        }

        public void SuperSizeChange(Slider SuperSizeSlider)
        {
            SuperSize = (int)SuperSizeSlider.value;
        }
        
        void LateUpdate()
        {
            if (SuperSizeText.text != "保存中")
            {
                SuperSizeText.color = Color.white;
                SuperSizeText.text = "解像度" + SuperSize.ToString() + "倍";
            }
        } 
    }
}
