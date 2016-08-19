using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Controls the light journey points from a travel start to destination
public class WorldSystem: MonoBehaviour {
	public float lightBeat;				// The 'heartbeat' of the journey lights, used for color and light
	// Used to lerp the colors of the enabled journey lights
	public Color offColor;					// Represent the off color of a journey light
	public Color onColor;					// Represents the on color of a journey light

	[SerializeField] private WorldPoint[] points;		// The series of points on the journey

	// Use this for initialization
	void Start () {
		lightBeat = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		float duration = 2.0f;
		lightBeat = Mathf.PingPong (Time.time, duration) / duration; // Ping pong the value of light beat between 0 and 1
	}

	void OnEnable() {
		points = transform.GetComponentsInChildren<WorldPoint> ();

		foreach (WorldPoint pt in points) {
			pt.SetReady ();
			PointInteraction ptInt = pt.gameObject.GetComponent<PointInteraction> ();
			ptInt.ToggleCanBeSelected();
		}
	}
}
