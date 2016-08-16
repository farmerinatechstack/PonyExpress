using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CursorScript : MonoBehaviour {
	private Text cursorText;
	private bool show = true;

	// Use this for initialization
	void Start () {
		cursorText = gameObject.GetComponent<Text> ();
		InvokeRepeating ("Flicker", 0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Flicker() {
		cursorText.enabled = show;
		show = !show;
	}
}
