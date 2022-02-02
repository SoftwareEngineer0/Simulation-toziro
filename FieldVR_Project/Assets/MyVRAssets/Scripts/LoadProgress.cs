using UnityEngine;
using System.Collections;
using UnityEngine .UI;

namespace MyVR_Assets
{
    public class LoadProgress : MonoBehaviour
    {
        public Text ProgressText;

        void Awake()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            }
        }

        IEnumerator Start()
        {
            AsyncOperation async = Application.LoadLevelAsync("MyVRTemplate");
            //async.allowSceneActivation = false;

            do
            {
                ////Debug.Log("progress: " + async.progress + "isDone: " + async.isDone);
                ProgressText.text = "…" + (async.progress * 111f).ToString("f0") + "%";

                yield return new WaitForEndOfFrame();
            }
            while (async.isDone == false);

        }
    }
}
