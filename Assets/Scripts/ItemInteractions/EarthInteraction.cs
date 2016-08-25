using UnityEngine;
using System.Collections;

// Handles interactions with the globe.
public class EarthInteraction : MonoBehaviour {
	public Transform cameraTransform;
	public StartScript startScript;
	public bool enableSpin;

	[SerializeField] private MenuDisplay menuDisplay;
	[SerializeField] private VRAssets.VRInteractiveItem interactiveItem;

	private float xRotation = 0f;
	private float yRotation = 0f;

	void OnEnable() {
		menuDisplay.MenuToggled += ToggleSpin;
	}

	void OnDisable() {
		menuDisplay.MenuToggled -= ToggleSpin;
	}

	void Start() {
		enableSpin = true;
	}

	private void Update() {
		if (enableSpin) {
			Vector3 rotateVector = GetRotationVector ();

			float yLower = 0.80f;

			transform.Rotate (new Vector3 (0, rotateVector.y, 0), Space.Self);

			transform.Rotate (new Vector3 (rotateVector.x, 0, 0), Space.World);
			if (transform.up.y < yLower) {
				transform.Rotate (new Vector3 (-rotateVector.x, 0, 0), Space.World);
			}
		}
	}

	private Vector3 GetRotationVector() {
		Vector3 gazeXYDirection = cameraTransform.forward - Vector3.forward;
		if (gazeXYDirection.magnitude > 0.1f) {
			return new Vector3 (-gazeXYDirection.y, gazeXYDirection.x, 0) * Mathf.Pow(1.2f + gazeXYDirection.magnitude, 2f);
		} else {	// Enable keypad spinning
			return new Vector3 (-Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0);
		}
		return Vector3.zero;
	}

	private void ToggleSpin() {
		enableSpin = !enableSpin;
	}
}
