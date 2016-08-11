using UnityEngine;
using UnityEngine.UI;

namespace VRAssets
{
	// A VR friendly reticle
	public class Reticle : MonoBehaviour
	{
		[SerializeField] private float defaultDistance = 3f; 	   	// The default distance away from the camera the reticle is placed
		[SerializeField] private bool useNormal;                  	// Whether the reticle should be placed parallel to a surface
		[SerializeField] private Image reticleImage;				// Reference to the image component that represents the reticle
		[SerializeField] private Transform reticleTransform;      	// We need to affect the reticle's transform
		[SerializeField] private Transform vrCamera;                // The reticle is always placed relative to the camera

		private Vector3 originalScale;                            	// Since the scale of the reticle changes, the original scale needs to be stored
		private Quaternion originalRotation;                      	// Used to store the original rotation of the reticle

		public bool UseNormal
		{
			get { return useNormal; }
			set { useNormal = value; }
		}
			
		public Transform ReticleTransform { get { return reticleTransform; } }

		private void Awake()
		{
			// Store the original scale and rotation
			originalScale = reticleTransform.localScale;
			originalRotation = reticleTransform.localRotation;
		}

		public void Hide()
		{
			reticleImage.enabled = false;
		}


		public void Show()
		{
			reticleImage.enabled = true;
		}


		// Used when the VREyeRaycaster hasn't hit anything
		public void SetPosition ()
		{
			reticleTransform.position = vrCamera.position + vrCamera.forward * defaultDistance;

			reticleTransform.localScale = originalScale * defaultDistance;

			reticleTransform.localRotation = originalRotation;
		}


		// Used when the VREyeRaycaster has hit something
		public void SetPosition (RaycastHit hit)
		{
			reticleTransform.position = hit.point;
			reticleTransform.localScale = originalScale * hit.distance;

			if (useNormal)
				reticleTransform.rotation = Quaternion.FromToRotation (Vector3.forward, hit.normal);
			else
				reticleTransform.localRotation = originalRotation;
		}
	}
}