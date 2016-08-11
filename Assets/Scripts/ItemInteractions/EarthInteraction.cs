using UnityEngine;
using System.Collections;

// Handles interactions with the globe.
public class EarthInteraction : MonoBehaviour {
	public Transform cameraTransform;

	[SerializeField] private VRAssets.VRInteractiveItem interactiveItem;
	[SerializeField] private VRAssets.ReticleRadial radial;

	private float xRotation = 0f;
	private float yRotation = 0f;

	private void OnEnable() {
		interactiveItem.OnEnter += HandleEnter;
		interactiveItem.OnExit += HandleExit;
		interactiveItem.OnDown += HandleDown;
	}

	private void OnDisable() {
		interactiveItem.OnEnter -= HandleEnter;
		interactiveItem.OnExit -= HandleExit;
		interactiveItem.OnDown -= HandleDown;
	}

	private void HandleDown() {

	}

	private void HandleEnter() {
		
	}

	private void HandleExit() {
		
	}

	private void Start() {
		print ("Start Up: " + transform.up);
	}

	private void Update() {
		Vector3 rotateVector = GetRotationVector ();

		float yLower = 0.80f;

		transform.Rotate (new Vector3 (0, rotateVector.y, 0), Space.Self);

		transform.Rotate (new Vector3 (rotateVector.x, 0, 0), Space.World);
		if (transform.up.y < yLower) {
			transform.Rotate (new Vector3 (-rotateVector.x, 0, 0), Space.World);
		}

		/*
		transform.Rotate (rotateVector, Space.World);

		if (transform.forward.y > yUpper || transform.forward.y < yLower) {
			transform.Rotate (-rotateVector, Space.World);
		}
		*/

		if (Input.GetMouseButtonDown (0)) {
			print ("Current Up: " + transform.up);
		}


		/*
		xRotation += rotateVector.x;
		xRotation = Mathf.Clamp (xRotation, -50f, 50f);
		yRotation += rotateVector.y;

		transform.eulerAngles = new Vector3 (transform.eulerAngles.x, yRotation, 0);
		transform.eulerAngles = new Vector3 (xRotation, transform.eulerAngles.y, 0);

		if (Input.GetMouseButtonDown (0)) {
			print ("Global Euler: " + transform.eulerAngles);
			print ("Local Euler: " + transform.localEulerAngles);
		}
		*/
	}

	private Vector3 GetRotationVector() {

		Vector3 gazeXYDirection = cameraTransform.forward - Vector3.forward;
		if (gazeXYDirection.magnitude > 0.2f) 
			return new Vector3 (-gazeXYDirection.y, gazeXYDirection.x, 0);

		return Vector3.zero;
	}
}
