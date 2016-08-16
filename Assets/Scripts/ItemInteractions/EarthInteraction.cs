using UnityEngine;
using System.Collections;

// Handles interactions with the globe.
public class EarthInteraction : MonoBehaviour {
	public Transform cameraTransform;
	public StartScript startScript;

	[SerializeField] private VRAssets.VRInteractiveItem interactiveItem;

	private float xRotation = 0f;
	private float yRotation = 0f;

	private void Update() {
		Vector3 rotateVector = GetRotationVector ();

		float yLower = 0.80f;

		transform.Rotate (new Vector3 (0, rotateVector.y, 0), Space.Self);

		transform.Rotate (new Vector3 (rotateVector.x, 0, 0), Space.World);
		if (transform.up.y < yLower) {
			transform.Rotate (new Vector3 (-rotateVector.x, 0, 0), Space.World);
		}
	}

	private Vector3 GetRotationVector() {

		Vector3 gazeXYDirection = cameraTransform.forward - Vector3.forward;
		if (gazeXYDirection.magnitude > 0.2f) 
			return new Vector3 (-gazeXYDirection.y, gazeXYDirection.x, 0);

		return Vector3.zero;
	}
}
