using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class Screenshot : MonoBehaviour {
	public KeyCode key = KeyCode.F12;
	public int scale = 1;

	public static string ScreenShotName(int width, int height) {
		return string.Format("screen_{0}x{1}_{2}.png",
			                     width, height,
			                     System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}

	void LateUpdate() {
		if (Input.GetKeyDown(key)) {
			Capture();
		}
	}

	public void Capture() {
		Application.CaptureScreenshot(ScreenShotName(Screen.width, Screen.height), scale);
		Debug.Log(string.Format("Saved new screenshot"));
	}
}
