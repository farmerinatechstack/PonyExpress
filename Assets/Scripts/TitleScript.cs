using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TitleScript : MonoBehaviour {
	[SerializeField] private float waitTime;
	[SerializeField] private float writeSpeed;
	[SerializeField] private float translationAmount;
	//[SerializeField] private RectTransform cursor;


	private Text titleText;
	private List<string> titles;

	private bool cycleTitle;
	[SerializeField] private string currTitle;

	// Use this for initialization
	void Start () {
		titleText = gameObject.GetComponent<Text> ();
		currTitle = "Home";
		cycleTitle = true;

		titles = new List<string> ();
		titles.Add ("Home");
		titles.Add ("Casa");
		titles.Add ("Bahay");
		titles.Add ("Domicile");
		titles.Add ("Ghar");
	}
	
	// Update is called once per frame
	void Update () {
		if (cycleTitle) {
			StartCoroutine (AnimateTitle ());
		}
	}

	IEnumerator AnimateTitle() {
		cycleTitle = false;
		yield return new WaitForSeconds (waitTime);

		for (int i = 0; i < currTitle.Length; i++) { // Delete characters
			titleText.text = currTitle.Substring(0, currTitle.Length-i-1);
			TranslateCursor (-translationAmount);
			yield return new WaitForSeconds (writeSpeed);
		}

		string prevTitle = currTitle;
		titles.Remove (prevTitle);
		currTitle = titles [Random.Range(0, titles.Count)];
		titles.Add (prevTitle);

		for (int i = 0; i < currTitle.Length; i++) { // Add characters
			titleText.text = currTitle.Substring(0,i+1);
			TranslateCursor (translationAmount);
			yield return new WaitForSeconds (writeSpeed);
		}
		cycleTitle = true;
	}

	void TranslateCursor(float translationAmount) {
		//Vector3 newPos = cursor.localPosition;
		//newPos.x += translationAmount;

		//cursor.localPosition = newPos;
	}
}
