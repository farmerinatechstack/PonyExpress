using UnityEngine;
using System.Collections;

// Used to test various functionality
public class TestScript : MonoBehaviour {
	public GameObject videoPrev2D;
	public Texture image;

	// Use this for initialization
	void Start () {
		Renderer rend = videoPrev2D.GetComponent<Renderer> ();
		rend.material.mainTexture = image;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
