#if UNITY_EDITOR
#if UNITY_STANDALONE_WIN


using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace GPS_Photo
{
    public class GetGPS : MonoBehaviour
    {
        [DllImport("System.Drawing.dll")]
        private static extern void Func();

        string FilePath;

        public class Point
        {
            public string r { get; set; }
            public double dd { get; set; }
            public double mm { get; set; }
            public double ss { get; set; }
        }

        // Use this for initialization
        void Awake()
        {
            FilePath = Application.dataPath + ("/StreamingAssets/GPS_Photo/");
        }

        public string[] GetGPSInfo(string filename)
        {
            string[] GPSInfo = new string[3];

            Point lat = new Point();
            Point lon = new Point();

            string fn = FilePath + filename;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(fn);
            int[] ids = bmp.PropertyIdList;

            // 緯度の南北（タグ番号 1.GPSLatitudeRef）を取得
            System.Drawing.Imaging.PropertyItem gpsLatitudeRef = bmp.PropertyItems[Array.IndexOf(ids, 0x0001)];
            lat.r = BitConverter.ToChar(gpsLatitudeRef.Value, 0).ToString();

            // 緯度（タグ番号 2.GPSLatitude）を取得
            System.Drawing.Imaging.PropertyItem gpsLatitude = bmp.PropertyItems[Array.IndexOf(ids, 0x0002)];
            lat.dd = BitConverter.ToUInt32(gpsLatitude.Value, 0) / BitConverter.ToUInt32(gpsLatitude.Value, 4);

            if (BitConverter.ToUInt32(gpsLatitude.Value, 12) == 100)
            {
                // DMM形式 → DMS形式
                double mmmmm = (double)BitConverter.ToUInt32(gpsLatitude.Value, 8) / (double)BitConverter.ToUInt32(gpsLatitude.Value, 12);
                lat.ss = (mmmmm % 1) * 60;
                lat.mm = (UInt32)mmmmm;
            }
            else
            {
                // DMS形式
                lat.mm = BitConverter.ToUInt32(gpsLatitude.Value, 8) / BitConverter.ToUInt32(gpsLatitude.Value, 12);
                lat.ss = (double)BitConverter.ToUInt32(gpsLatitude.Value, 16) / (double)BitConverter.ToUInt32(gpsLatitude.Value, 20);
            }

            // 経度の東西（タグ番号 3.GPSLongitudeRef）を取得
            System.Drawing.Imaging.PropertyItem gpsLongitudeRef = bmp.PropertyItems[Array.IndexOf(ids, 0x0003)];
            lon.r = BitConverter.ToChar(gpsLongitudeRef.Value, 0).ToString();

            // 経度（タグ番号 4.GPSLongitude）を取得
            System.Drawing.Imaging.PropertyItem gpsLongitude = bmp.PropertyItems[Array.IndexOf(ids, 0x0004)];
            lon.dd = BitConverter.ToUInt32(gpsLongitude.Value, 0) / BitConverter.ToUInt32(gpsLongitude.Value, 4);

            if (BitConverter.ToUInt32(gpsLongitude.Value, 12) == 100)
            {
                // DMM形式 → DMS形式
                double mmmmm = (double)BitConverter.ToUInt32(gpsLongitude.Value, 8) / (double)BitConverter.ToUInt32(gpsLongitude.Value, 12);
                lon.ss = (mmmmm % 1) * 60;
                lon.mm = (UInt32)mmmmm;
            }
            else
            {
                // DMS形式
                lon.mm = BitConverter.ToUInt32(gpsLongitude.Value, 8) / BitConverter.ToUInt32(gpsLongitude.Value, 12);
                lon.ss = (double)BitConverter.ToUInt32(gpsLongitude.Value, 16) / (double)BitConverter.ToUInt32(gpsLongitude.Value, 20);
            }

            GPSInfo[0] = lat.dd.ToString() + "," + lat.mm.ToString() + "," + lat.ss.ToString();
            GPSInfo[1] = lon.dd.ToString() + "," + lon.mm.ToString() + "," + lon.ss.ToString();

            //高度を取得
            System.Drawing.Imaging.PropertyItem GPSAltitude = bmp.PropertyItems[Array.IndexOf(ids, 0x0006)];
            double Altitude = (double)BitConverter.ToInt32(GPSAltitude.Value, 0) / (double)BitConverter.ToInt32(GPSAltitude.Value, 4);

            GPSInfo[2] = Altitude.ToString();

            bmp.Dispose();

            return GPSInfo;
        }
    }
}

#endif
#endif
