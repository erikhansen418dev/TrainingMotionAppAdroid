using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AppName : MonoBehaviour {

	Text myLabel;

	void Start()
	{
		myLabel = GetComponent<Text>();

		if(myLabel == null)
			return;

		myLabel.text = "Visual Skeleton Sample";
	}
}
