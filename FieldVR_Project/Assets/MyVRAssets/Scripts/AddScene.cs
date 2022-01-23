using UnityEngine;
using System.Collections;

namespace MyVR_Assets
{
    public class AddScene : MonoBehaviour
    {

        void Awake()
        {
            #if UNITY_EDITOR

            #else

                Application.LoadLevelAdditive("SampleScene");
            
            #endif
        }

    }
}
