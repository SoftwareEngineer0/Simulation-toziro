using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour {

	public Transform MainCamera;
	public Transform MiniMapCamera;
	public Transform MiniMapMarker;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		MiniMapCamera.position = new Vector3 (MainCamera.position.x, MainCamera.position.y +100.0f, MainCamera.position.z);

		MiniMapMarker.localEulerAngles = new Vector3 (0.0f, 180.0f, MainCamera.eulerAngles.y +180.0f);

	}

    private float Width;
    private float Hight;

    public void AspectAdjust ()
    {
        Width = this.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        Hight = this.gameObject.GetComponent<RectTransform>().sizeDelta.y;

        StartCoroutine("Resize");
    }

    private IEnumerator Resize ()
    {
        yield return null;

        if (Width >= Hight)
        {
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (Width, Width);
        }
        else if (Hight >= Width)
        {
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2 (Hight, Hight);
        }
    }
}
