using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace VRAssets
{
	// Handles the reticle radial
	public class ReticleRadial : MonoBehaviour
	{
		public event Action OnSelectionComplete;					// Triggered when the bar has filled

		[SerializeField] private float selectionDuration = 2f;		// How long it takes for the bar to fill
		[SerializeField] private bool hideOnStart = true;			// Whether or not the bar should be visible at the start
		[SerializeField] private Image selection;					// Reference to the image who's fill amount is adjusted to display the bar
		[SerializeField] private VRInput vrInput;					// Reference to the VRInput so that input events can be subscribed to
		[SerializeField] private VREyeRaycaster raycaster;			// Reference to the VR raycaster

		[SerializeField] private AudioSource selectAudio;
		[SerializeField] private AudioSource backAudio;
		[SerializeField] private GameObject backText;

		private Coroutine selectionFillRoutine;						// Used to start and stop the filling coroutine based on input
		private bool isSelectionRadialActive;						// Whether or not the bar is currently useable
		private bool radialFilled;									// Used to allow the coroutine to wait for the bar to fill

		public float SelectionDuration { get { return selectionDuration; } }

		private void OnEnable()
		{
			vrInput.OnDown += HandleDown;
			vrInput.OnUp += HandleUp;
		}


		private void OnDisable()
		{
			vrInput.OnDown -= HandleDown;
			vrInput.OnUp -= HandleUp;
		}

		private void Start()
		{
			// Setup the radial to have no fill at the start and hide if necessary
			selection.fillAmount = 0f;

			if(hideOnStart)
				Hide();
		}
			
		public void Show()
		{
			selection.gameObject.SetActive(true);
			isSelectionRadialActive = true;
		}

		public void Hide()
		{
			selection.gameObject.SetActive(false);
			isSelectionRadialActive = false;

			// This effectively resets the radial for when it's shown again
			selection.fillAmount = 0f;            
		}

		public void ResetRadial() {
			selection.fillAmount = 0;
			Show ();
		}

		private IEnumerator FillSelectionRadial()
		{
			// At the start of the coroutine, the bar is not filled
			radialFilled = false;

			// Create a timer and reset the fill amount
			float timer = 0f;
			selection.fillAmount = 0f;

			// This loop is executed once per frame until the timer exceeds the duration
			while (timer < selectionDuration)
			{
				// The image's fill amount requires a value from 0 to 1 so we normalise the time
				selection.fillAmount = timer / selectionDuration;

				// Increase the timer by the time between frames and wait for the next frame
				timer += Time.deltaTime;
				yield return null;
			}

			// When the loop is finished set the fill amount to be full
			selection.fillAmount = 1f;

			// Turn off the radial so it can only be used once
			isSelectionRadialActive = false;

			// The radial is now filled so the coroutine waiting for it can continue
			radialFilled = true;

			// If there is anything subscribed to OnSelectionComplete call it
			if (OnSelectionComplete != null) {
				VRInteractiveItem item = raycaster.CurrentInteractible;
				if (item != null) {
					if (item.gameObject == backText) {
						backAudio.Play ();
					} else {
						selectAudio.Play ();
					}
				}
				ResetRadial ();
				OnSelectionComplete ();
			}
		}

		public IEnumerator WaitForSelectionRadialToFill ()
		{
			// Set the radial to not filled in order to wait for it
			radialFilled = false;

			// Make sure the radial is visible and usable
			Show ();

			// Check every frame if the radial is filled
			while (!radialFilled)
			{
				yield return null;
			}

			// Once it's been used make the radial invisible
			Hide ();
		}

		private void HandleDown()
		{
			// If the radial is active start filling it
			if (isSelectionRadialActive)
			{
				selectionFillRoutine = StartCoroutine(FillSelectionRadial());
			}
		}
			
		private void HandleUp()
		{
			// If the radial is active stop filling it and reset its amount
			if (isSelectionRadialActive)
			{
				if(selectionFillRoutine != null)
					StopCoroutine(selectionFillRoutine);

				selection.fillAmount = 0f;
			}
		}
	}
}