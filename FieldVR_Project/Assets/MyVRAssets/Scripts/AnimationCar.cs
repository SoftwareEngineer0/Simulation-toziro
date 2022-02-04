﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

namespace MyVR_Assets
{
    public class AnimationCar : SingletonMonoBehaviour<AnimationCar>
    {
        public int GenerateNum;
        private float Speed_km;

        // public AnimationClip Genkyo01;
        // public AnimationClip Genkyo02;

        // public AnimationClip Keiaku01;
        // public AnimationClip Keiaku02;

        public AnimationClip[] CarAnimClip;
        public int[] AnimationClipWeight;
        public GameObject[] CarModel;
        private GameObject cloneCar;
        public Animation cloneCarAnimation;
        private bool isGenerated = false;

        // public Text ObjectSwitchButtonText;

        // public Toggle CarToggle;

        void Start()
        {
            isGenerated = false;            
            Debug.Log("AnimationCar:" + AutoRun.Instance.AutoRunSpeedSlider.value);
            //CarGenerate_A();
        }
        void Update()
        {
            //ObjectSwitchButtonText.text
            //Debug.Log("AnimationCar!:" + AutoRun.Instance.AutoRunSpeedSlider.value);
            Speed_km = AutoRun.Instance.AutoRunSpeedSlider.value;
            if(isGenerated) {
                cloneCar = this.transform.GetChild(0).gameObject;
                //Debug.Log("cloneCar:"+cloneCar);
                if(cloneCar) {
                    cloneCarAnimation = cloneCar.GetComponents<Animation>()[0];
                    //Debug.Log("AnimationCar!:" + cloneCarAnimation + "speed:" + Speed_km);
                    cloneCarAnimation["CarAnim_A"].speed = Speed_km/3;
                }
                if(!AutoRun.Instance.getAutoRunStatus())
                {
                    cloneCarAnimation["CarAnim_A"].speed = 0;
                }
            }
            
            
            
            //cloneCarAnimation.speed = Speed_km;

        }
        public bool getStatusGeneration()
        {
            if(isGenerated == true)
                return true;
            else 
                return false;
        }
        public void setStatusGeneration(bool flag)
        {
            isGenerated = flag;
        }
        public void CarPlay()
        {
            cloneCarAnimation = GetComponentInChildren<Animation>();
            cloneCarAnimation.AddClip(CarAnimClip[0], "CarAnim_A");
            cloneCarAnimation["CarAnim_A"].speed = Speed_km/3;//Speed_km * 0.01f;
            cloneCarAnimation["CarAnim_A"].normalizedTime = 0f;
            cloneCarAnimation.Play("CarAnim_A");
            isGenerated = true;

            // for (int i = 0; i < GenerateNum; i++)
            // {
            //     Debug.Log("CarGenerate_A");
            //     GameObject GenerateClone = Instantiate(CarModel[Random.Range(0,CarModel.Length)]);
            //     GenerateClone.transform.localScale = new Vector3(1.1f,1.0f,1.0f);
            //     GenerateClone.tag = "AnimationCar";
            //     GenerateClone.transform.parent = this.transform;
            //     //GenerateClone.transform.parent.position = new Vector3(this.transform.position.x, 0.81f, this.transform.position.z);

            //     cloneCarAnimation = GenerateClone.AddComponent<Animation>();
            //     Debug.Log("CarGererate_A:" + this.transform.position.x + ":" + ClipWeight.GetRandomIndex(AnimationClipWeight) + ":" + CarAnimClip[0]);

            //     cloneCarAnimation.AddClip(CarAnimClip[0], "CarAnim_A");
            //     cloneCarAnimation["CarAnim_A"].speed = Speed_km/3;//Speed_km * 0.01f;
            //     cloneCarAnimation["CarAnim_A"].normalizedTime = Random.Range(0, 1f);
            //     cloneCarAnimation.Play("CarAnim_A");
            //     isGenerated = true;
            // }            
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
            CarPlay();
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
