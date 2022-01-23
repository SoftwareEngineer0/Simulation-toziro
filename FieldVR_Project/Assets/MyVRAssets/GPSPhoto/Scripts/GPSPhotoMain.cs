#if UNITY_EDITOR
#if UNITY_STANDALONE_WIN

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GPS_Photo
{
    //構造体
    public struct GPSPhoto
    {
        public string Name;
        public Vector3 Position;
    }

    public class GPSPhotoMain : MonoBehaviour
    {

        [DllImport("System.Drawing.dll")]
        private static extern void Func();

        public string PositionType = "3D";
        public int Coordinate = 2;
        public Vector2 OffsetXY;
        public GameObject ThumbnailPrefab;
        public GameObject GPSPhotoHideButton;
        public GameObject PhotoInfo;
        private string[] GPSInformation = new string[3];
        private GPSPhoto[] PhotoInfoList;

        int Count = 0;

        void Start()
        {
            PhotoInfo.SetActive(false);
            GPSPhotoHideButton.SetActive(false);
        }

        public void CheckNetwork()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                SelectFolder();
            }
            else
            {
                PhotoInfo.GetComponent<Text>().text = "ネットワークに接続されていません。";
            }
        }

        void SelectFolder()
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            // ダイアログに表示する説明文を設定
            fbd.Description = "ディレクトリを指定してください。";

            // 参照の開始位置（ルートフォルダ）を設定
            fbd.RootFolder = Environment.SpecialFolder.Desktop;

            // ディレクトリを初期選択
            fbd.SelectedPath = @"C:\tmp";

            // 新しいフォルダボタンを表示するかどうか
            fbd.ShowNewFolderButton = true;

            // ＯＫボタンが押されたらテキストボックスに表示
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string SelectFolder = fbd.SelectedPath;
                PhotoInfo.GetComponent<Text>().text = "ファイルを読込中";
                FolderCopy(SelectFolder);
            }
        }

        void FolderCopy(string SelectFolder)
        {
            string GPSPhotoFolder = UnityEngine.Application.streamingAssetsPath + ("/GPS_Photo/");
            string[] DeleteFiles = Directory.GetFiles(GPSPhotoFolder, "*", System.IO.SearchOption.AllDirectories);

            foreach(string file in DeleteFiles)
            {
                File.Delete(file); 
            }

            string[] CopyFiles = Directory.GetFiles(SelectFolder, "*.jpg");

            foreach(string file in CopyFiles)
            {
                System.IO.File.Copy(file, GPSPhotoFolder + Path.GetFileName(file), true);
            }
            LoadFiles();
        }

        public void LoadFiles()
        {
            //フォルダ内全てのJPGファイルの名前と座標のリストを作る

            string dir = UnityEngine.Application.streamingAssetsPath + ("/GPS_Photo/");
            string[] targetfiles = Directory.GetFiles(dir, "*.jpg");

            PhotoInfoList = new GPSPhoto[targetfiles.Length];

            foreach (string file in targetfiles)
            {
                //フルパス名からファイル名だけを取得
                PhotoInfoList[Count].Name = Path.GetFileName(file);

                //ファイルのGPS情報を取得
                //引数ファイル名、戻り値string型配列、緯度（60進法）、経度（60進法）、高度（10進法）
                GPSInformation = this.gameObject.GetComponent<GetGPS>().GetGPSInfo(PhotoInfoList[Count].Name);

                //平面直角座標系を指定1～19
                this.gameObject.GetComponent<WWWLatLong2XY>().SelectCoordinate = Coordinate;

                //緯度経度をXY平面直角座標に変換
                //引数string型、緯度（60進法）、経度（60進法）
                Vector2 XY = this.gameObject.GetComponent<WWWLatLong2XY>().LatLong2XY(GPSInformation[0], GPSInformation[1]);

                //測量座標系を数学座標系にXとYを入れ替える
                PhotoInfoList[Count].Position = new Vector3(XY.y, XY.x, float.Parse(GPSInformation[2]));

                print(PhotoInfoList[Count].Name);
                print("緯度" + GPSInformation[0] + "\n" + "経度" + GPSInformation[1] + "\n" + "高度" + GPSInformation[2]);
                print(PhotoInfoList[Count].Position);

                ++Count;
            }
            CreateThumbnail(targetfiles);
        }

        //サムネイル画像を作る
        int resizeWidth;
        int resizeHeight;

        void CreateThumbnail(string[] JpgFiles)
        {
            System.IO.Directory.CreateDirectory(UnityEngine.Application.streamingAssetsPath + "/GPS_Photo/Thumbnail");
       
            foreach (string file in JpgFiles)
            {
                Bitmap bmp = new Bitmap(file);

                //長辺のサイズを160、短辺を成り行き
                if (bmp.Width > bmp.Height)
                {
                    resizeWidth = 160;
                    resizeHeight = (int)(bmp.Height * ((double)resizeWidth / (double)bmp.Width));
                }
                else if (bmp.Width < bmp.Height)
                {
                    resizeHeight = 160;
                    resizeWidth = (int)(bmp.Width * ((double)resizeHeight / (double)bmp.Height));
                }

                Bitmap ResizeBmp = new Bitmap(bmp, resizeWidth, resizeHeight);
                ResizeBmp.Save(UnityEngine.Application.streamingAssetsPath + "/GPS_Photo/Thumbnail/" + Path.GetFileName(file));
            }

            CreateObject(JpgFiles);
        }

        //Texture2D tex;

        //サムネイルオブジェクトを作る
        void CreateObject(string[] JpgFiles)
        {
            GameObject CanvasObject = GameObject.Find("GPSPhotoCanvas");

            foreach (Transform n in CanvasObject.transform)
            {
                GameObject.Destroy(n.gameObject);
            }        

            foreach (string file in JpgFiles)
            {
                //CanvasObjectの子としてクローンを作る
                GameObject Clone = (GameObject)Instantiate(ThumbnailPrefab);
                Clone.transform.SetParent(CanvasObject.transform);
                Clone.name = (Path.GetFileName(file));
            
                //3次元座標にクローン配置
                Vector3 FilePosition = PositionSearch(Clone.name);

                if (PositionType == "2D")
                {
                    Clone.GetComponent<ThumbnailCTRL>().SelfPosition = new Vector3(((FilePosition.x *-1) + OffsetXY.x), 0, ((FilePosition.y *-1) + OffsetXY.y));
                }
                else if (PositionType == "3D")
                {
                    Clone.GetComponent<ThumbnailCTRL>().SelfPosition = new Vector3(((FilePosition.x *-1) + OffsetXY.x), FilePosition.z, ((FilePosition.y *-1) + OffsetXY.y));
                }


                string texpath = "file://" + UnityEngine.Application.streamingAssetsPath + "/GPS_Photo/Thumbnail/" + Path.GetFileName(file);
                StartCoroutine(SetTextrue(texpath,Clone));

                //GPS情報を渡す
                Clone.GetComponent<ThumbnailCTRL>().XYZ = new Vector3(FilePosition.x, FilePosition.y, FilePosition.z);

                string[] LatLong = this.gameObject.GetComponent<GetGPS>().GetGPSInfo(Clone.name);
                Clone.GetComponent<ThumbnailCTRL>().LatLong[0] = LatLong[0];
                Clone.GetComponent<ThumbnailCTRL>().LatLong[1] = LatLong[1];
            }

            PhotoInfo.SetActive(true);
            GPSPhotoHideButton.SetActive(true);
            PhotoInfo.GetComponent<Text>().text = "マウスポインタの重なった写真ファイルの情報を表示します";
            Count = 0;
        }

        IEnumerator SetTextrue(string texpath, GameObject Clone)
        {
            WWW www = new WWW(texpath);
            yield return www;

            while(true)
            {
                if(www.isDone)
                {
                    break;
                }
            }

            RectTransform FlameSize = Clone.GetComponent(typeof (RectTransform)) as RectTransform;
            FlameSize.sizeDelta = new Vector2(www.texture.width + 10, www.texture.height + 20);
            GameObject ThumbnailRawImage = Clone.transform.Find("ThumbnailRawImage").gameObject;

            ThumbnailRawImage.GetComponent<RawImage>().texture = www.texture;
        }

        //ファイル名からPhotoInfoListの中を検索し該当座標を返す

        Vector3 Pos;
        public Vector3 PositionSearch (string FileName)
        {
            for(int i =0 ; i <= PhotoInfoList.Length; ++i)
            {
                if(PhotoInfoList[i].Name == FileName)
                {
                    Pos = PhotoInfoList[i].Position;
                    break;
                }
            }
            return Pos;
        }

        public void HideButton (GameObject GPSPhotoCanvas)
        {
            GPSPhotoCanvas.SetActive(!GPSPhotoCanvas.activeSelf);

            GameObject GPSPhotoHideButtonText = GameObject.Find("GPSPhotoHideButtonText");
            string Text = GPSPhotoHideButtonText.GetComponent<Text>().text;

            if (Text == "写真サムネイル非表示")
            {
                GPSPhotoHideButtonText.GetComponent<Text>().text = "写真サムネイル表示";
            }
            else if (Text == "写真サムネイル表示")
            {
                GPSPhotoHideButtonText.GetComponent<Text>().text = "写真サムネイル非表示";
            }
        }
    }
}
#endif
#endif