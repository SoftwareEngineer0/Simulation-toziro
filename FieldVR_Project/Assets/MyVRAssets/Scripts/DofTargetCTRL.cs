using UnityEngine;
using System.Collections;

namespace MyVR_Assets
{
    public class DofTargetCTRL : MonoBehaviour
    {
        public GameObject Player;

        RaycastHit hit;
        Ray ray;

        void LateUpdate()
        {
            bool MousePosOverUI = Player.GetComponent<VRCameraRig>().MousePosOverUI;

            //if (!MousePosOverUI)
            //{
              //  ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //}
            //else if (MousePosOverUI)
            //{
                ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            //}

            //とりあえずdefaltレイヤーに設定
            var layerMask = 1 << 0;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("DoftargetCTRL:" + hit.point);
                this.transform.position = hit.point;

            }
        }
    }
}
