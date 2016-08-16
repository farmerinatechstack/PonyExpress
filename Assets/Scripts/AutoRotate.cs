using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour {
	Vector3 rotation;

	// Use this for initialization
	void Start () {
		rotation = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		float dt = Time.deltaTime;
		rotation.x = rotation.y = rotation.z = dt;
		transform.Rotate (rotation, Space.World);
	}
}
