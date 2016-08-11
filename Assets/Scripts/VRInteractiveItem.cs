using UnityEngine;
using System;

namespace VRAssets {
	// Attach to any GameObject which reacts to gaze inputs 
	public class VRInteractiveItem : MonoBehaviour {
		public event Action OnEnter;
		public event Action OnExit;
		public event Action OnDown;

		protected bool entered;
		public bool isEntered {
			get { return entered; }
		}

		public void Enter() {
			entered = true;

			if (OnEnter != null)
				OnEnter();
		}

		public void Exit() {
			entered = false;

			if (OnExit != null) 
				OnExit();
		}

		public void Down() {
			if (OnDown != null)
				OnDown ();
		}
	}
}
