using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MyVR_Assets
{
    public class LandMarkTextCTRL : MonoBehaviour
    {

        public Transform TagetObj;

        void Start()
        {
            string text = GetComponent<Text>().text;
            string[] Line = text.Split('\n');

            int[] Lines = new int[Line.Length];

            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i] = Line[i].Length;
            }

            RectTransform rt = GetComponent(typeof(RectTransform)) as RectTransform;
            rt.sizeDelta = new Vector2(this.GetComponent<Text>().fontSize * Max(Lines) + (this.GetComponent<Text>().fontSize / 2),
                                       this.GetComponent<Text>().fontSize * Line.Length + (this.GetComponent<Text>().fontSize / 2));
        }

        //配列から最大値を返す
        public int Max(params int[] nums)
        {
            // 引数が渡されない場合
            if (nums.Length == 0) return 0;
            int max = nums[0];
            for (int i = 1; i < nums.Length; i++)
            {
                max = max > nums[i] ? max : nums[i];
                // Minの場合は不等号を逆にすればOK
            }
            return max;
        }

        void LateUpdate()
        {
            Vector3 ViewportPos = Camera.main.WorldToScreenPoint(TagetObj.position);

            if (ViewportPos.z > 0.0f)
            {
                this.GetComponent<Text>().enabled = true;
                gameObject.transform.position = new Vector3(ViewportPos.x, ViewportPos.y, 0.0f);
            }
            else
            {
                this.GetComponent<Text>().enabled = false;
            }
        }
    }
}
