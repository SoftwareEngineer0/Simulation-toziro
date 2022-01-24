using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

namespace MyVR_Assets
{
    public class AnimationCar : MonoBehaviour
    {
        public int GenerateNum;
        public float Speed_km;

        // public AnimationClip Genkyo01;
        // public AnimationClip Genkyo02;

        // public AnimationClip Keiaku01;
        // public AnimationClip Keiaku02;

        public AnimationClip[] CarAnimClip;
        public int[] AnimationClipWeight;
        public GameObject[] CarModel;

        // public Text ObjectSwitchButtonText;

        // public Toggle CarToggle;

        void Start()
        {
          CarGenerate_A();
        }

        public void CarGenerate_A()
        {
            for (int i = 0; i < GenerateNum; i++)
            {
                GameObject GenerateClone = Instantiate(CarModel[Random.Range(0,CarModel.Length)]);
                GenerateClone.transform.localScale = new Vector3(1.1f,1.0f,1.0f);
                GenerateClone.tag = "AnimationCar";
                GenerateClone.transform.parent = this.transform;
                GenerateClone.transform.parent.position = new Vector3(this.transform.position.x, 0.81f, this.transform.position.z);

                Animation CarAnim_A = GenerateClone.AddComponent<Animation>();
                Debug.Log("CarGererate_A:" + ClipWeight.GetRandomIndex(AnimationClipWeight) + ":" + CarAnimClip[0]);

                CarAnim_A.AddClip(CarAnimClip[ClipWeight.GetRandomIndex(AnimationClipWeight)], "CarAnim_A");
                CarAnim_A["CarAnim_A"].speed = Speed_km * 0.01f;
                CarAnim_A["CarAnim_A"].normalizedTime = Random.Range(0, 1f);
                CarAnim_A.Play("CarAnim_A");
            }
        }

        //void OnEnable()
        //{
            //var DestroyCar = GameObject.FindGameObjectsWithTag("AnimationCar");

            //foreach (var Obj in DestroyCar)
            //{
               //Destroy(Obj);
            //}

            //CarGenerate_A();
        //}

        void update()
        {
            //ObjectSwitchButtonText.text


        }

        public void AnimationChange()
        {
            // if (ObjectSwitchButtonText.text == "現況")
            // {
            //     CarAnimClip[0] = Genkyo01;
            //     CarAnimClip[1] = Genkyo02;

            //     var DestroyCar = GameObject.FindGameObjectsWithTag("AnimationCar");

            //     foreach (var Obj in DestroyCar)
            //     {
            //         Destroy(Obj);
            //     }

            //     CarGenerate_A();
            // }
            // else if(ObjectSwitchButtonText.text == "仮人道橋設置")
            // {
            //     var DestroyCar = GameObject.FindGameObjectsWithTag("AnimationCar");

            //     foreach (var Obj in DestroyCar)
            //     {
            //         Destroy(Obj);
            //     }
            // }
            // else if(ObjectSwitchButtonText.text == "工事完了後")
            // {
            //     CarAnimClip[0] = Keiaku01;
            //     CarAnimClip[1] = Keiaku02;

            //     var DestroyCar = GameObject.FindGameObjectsWithTag("AnimationCar");

            //     foreach (var Obj in DestroyCar)
            //     {
            //         Destroy(Obj);
            //     }

            //     CarGenerate_A();
            // }
            CarGenerate_A();
        }



        public void CarDestroy()
        {
            var DestroyCar = GameObject.FindGameObjectsWithTag("AnimationCar");

            foreach (var Obj in DestroyCar)
            {
                Destroy(Obj);
            }

            // if (CarToggle.isOn)
            // {
            //     StartCoroutine("GenerateAgain");
            // }
        }

        IEnumerator GenerateAgain()
        {
            yield return null;
            CarGenerate_A();
            // if (ObjectSwitchButtonText.text == "完成")//ObjectSwitchButton.GetComponent<ObjectSwitch>().Count == 1)
            // {
            //     CarGenerate_A();
            //     //CarKyoutsuGenerate();
            //     //CarGenkyoGenerate();
            // }
            // //else if (ObjectSwitchButton.GetComponent<ObjectSwitch>().Count == 2)
            // //{
            // //CarKyoutsuGenerate();
            // //CarKanseiGenerate();
            // //}
        }

        //アニメーションのルートをウエイトを付けて返す
        public class ClipWeight
        {
            //渡された重み付け配列からIndexを得る
            public static int GetRandomIndex(params int[] weightTable)
            {
                var totalWeight = weightTable.Sum();
                var value = Random.Range(1, totalWeight + 1);
                var retIndex = -1;
                for (var i = 0; i < weightTable.Length; ++i)
                {
                    if (weightTable[i] >= value)
                    {
                        retIndex = i;
                        break;
                    }
                    value -= weightTable[i];
                }
                return retIndex;
            }
        }
    }
}
