using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MyVR_Assets
{
    public class SunLightCTRL : MonoBehaviour
    {

        public Slider TimeSlider;

        public GameObject SunLight;
        public Material SkyBoxMaterial;

        public Transform GeshiSunRotObj;
        public Transform ToujiSunRotObj;
        private Transform ActiveSunRotObj;

        public float SunLightPower = 1.0f;

        private float Hour24;
        public float Hour12;

        private int ShowHour;
        private int ShowMinute;

        public Text TimeCounter;

        private string Season = null;
        public Text SeasonText;

        public bool AutoCycle = false;
        public Text AutoTimeText;

        public Color DayTimeFog;
        public Color NightTimeFog;

        float min;
        float max;

        void Start()
        {

            AutoCycle = false;
            Season = "夏至";
            ActiveSunRotObj = GeshiSunRotObj;
            GeshiSunRotObj.GetComponent<Animation>()["Take 001"].speed = 0.0F;
            ToujiSunRotObj.GetComponent<Animation>()["Take 001"].speed = 0.0F;

        }

        public void SeasonChange()
        {

            if (Season == "夏至")
            {
                Season = "冬至";
                ActiveSunRotObj = ToujiSunRotObj;
            }
            else if (Season == "冬至")
            {
                Season = "夏至";
                ActiveSunRotObj = GeshiSunRotObj;
            }
        }

        public void AutoTime()
        {

            if (AutoCycle == false)
            {
                AutoTimeText.color = Color.red;
                AutoCycle = true;
            }
            else if (AutoCycle == true)
            {
                AutoTimeText.color = Color.white;
                AutoCycle = false;
            }
        }

        void FixedUpdate()
        {

            SeasonText.text = Season;

            Hour24 = TimeSlider.value;

            if (AutoCycle == true)
            {

                TimeSlider.value += Time.deltaTime;

                if (TimeSlider.value >= 24.0f)
                {
                    TimeSlider.value = 0.0f;
                }
            }

            if (Hour24 <= 12.0f)
            {
                Hour12 = Hour24;
            }
            else if (Hour24 > 12.0f)
            {
                Hour12 = (24.0f - Hour24);
            }

            //ScaleValue

            if (Season == "夏至")
            {
                min = 2.5f;
                max = 7.0f;
            }
            else if (Season == "冬至")
            {
                min = 4.5f;
                max = 10.0f;
            }

            SunLight.GetComponent<Light>().intensity = ((Mathf.Clamp(Hour12, min, max) - min) / (max - min)) * SunLightPower;
            RenderSettings.ambientIntensity = Mathf.Clamp(SunLight.GetComponent<Light>().intensity, 0.5f, 1.5f);
            RenderSettings.reflectionIntensity = Mathf.Clamp(SunLight.GetComponent<Light>().intensity, 0.06f, 1.0f);
            RenderSettings.fogColor = Color.Lerp(NightTimeFog, DayTimeFog, SunLight.GetComponent<Light>().intensity * SunLight.GetComponent<Light>().intensity);
            SkyBoxMaterial.SetFloat("_AtmosphereThickness", Mathf.Clamp(SunLight.GetComponent<Light>().intensity * 1.7f, 0.13f, 0.55f));

            SunLight.transform.localEulerAngles = new Vector3(ActiveSunRotObj.localEulerAngles.x + 180, ActiveSunRotObj.localEulerAngles.y, ActiveSunRotObj.localEulerAngles.z);
            SunLight.transform.position = GeshiSunRotObj.position;

            ActiveSunRotObj.GetComponent<Animation>()["Take 001"].time = Hour24 * 2.0f;

            //タイムカウンター
            ShowHour = (int)Hour24;
            ShowMinute = (int)((Hour24 - ShowHour) * 60);
            TimeCounter.text = (ShowHour + "時" + ShowMinute + "分");
        }

        void OnApplicationQuit()
        {
            SkyBoxMaterial.SetFloat("_AtmosphereThickness", 0.55f);
        }
    }
}
