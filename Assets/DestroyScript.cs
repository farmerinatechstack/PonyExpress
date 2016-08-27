using UnityEngine;
using System.Collections;

public class DestroyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject dataObject = GameObject.FindGameObjectWithTag ("Data");
		if (dataObject != null) {
			DestroyImmediate (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
