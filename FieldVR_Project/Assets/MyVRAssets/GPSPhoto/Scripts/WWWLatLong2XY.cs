#if UNITY_EDITOR
#if UNITY_STANDALONE_WIN

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace GPS_Photo
{
    public class WWWLatLong2XY : MonoBehaviour
    {
        public int SelectCoordinate;        //平面直角座標系
        public WWW www;

        //国土地理院の平面直角座標換算サイト
        string url = "http://vldb.gsi.go.jp/sokuchi/surveycalc/surveycalc/bl2xy.pl?";

        public Vector2 LatLong2XY(string Lat, string Lng)
        {
            //60進法を10進法に変換
            double DecLat = LatLngToDec(Lat);
            double DecLng = LatLngToDec(Lng);

            StartCoroutine(WWWconvert(DecLat, DecLng));

            while (!www.isDone)
            {
                //ダウンロード中
            }

            string ReturnValue = www.text;
            string[] ReturnValues = ReturnValue.Split('"');

            Vector2 XY = new Vector2(float.Parse(ReturnValues[5]), float.Parse(ReturnValues[9]));

            return XY;
        }

        IEnumerator WWWconvert(double decLat, double decLng)
        {
            //国土地理院の平面直角座標換算サイトにパラメータを渡す
            www = new WWW(url + "outputType=json&refFrame=2&" + "zone=" + SelectCoordinate + "&" + "latitude=" + decLat + "&" + "longitude=" + decLng);

            yield return www;
        }

        //緯度経度の10進数変換
        public double LatLngToDec(string latlng)
        {
            string[] parts = latlng.Split(',');

            double ret = double.Parse(parts[0]) + (double.Parse(parts[1]) / 60) + (double.Parse(parts[2]) / 3600);

            return ret;
        }
    }
}

#endif
#endif
