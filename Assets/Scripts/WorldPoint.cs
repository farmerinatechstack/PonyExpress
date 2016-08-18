using UnityEngine;
using System.Collections;

// Encapsulates the behavior for a point on a journey
public class WorldPoint : MonoBehaviour {
	private WorldSystem world;
	private Renderer rend;
	private bool ready;

	// Use this for initialization
	void Start () {
		ready = false;
		world = transform.parent.GetComponent<WorldSystem> ();
		if (world == null) {
			// TODO: throw exception
		}

		rend = GetComponent<Renderer> ();
		rend.material.color = world.offColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (ready)
			rend.material.color = Color.Lerp (world.offColor, world.onColor, world.lightBeat);
	}

	public void SetReady() {
		ready = true;
	}
}
