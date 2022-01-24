using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace MyVR_Assets
{
    public class UIinputCTRL : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject Player;

        public void OnPointerEnter(PointerEventData data)
        {
            if (!data.dragging)
            {
                Player.GetComponent<VRCameraRig>().MousePosOverUI = true;
            }
        }

        public void OnPointerExit(PointerEventData data)
        {
            if (!data.dragging)
            {
                Player.GetComponent<VRCameraRig>().MousePosOverUI = false;
            }
            //StartCoroutine(DragOut());
        }

        IEnumerator DragOut()
        {
            while (Input.anyKey)
            {
                yield return null;
            }

            Player.GetComponent<VRCameraRig>().MousePosOverUI = false;
        }
    }
}
