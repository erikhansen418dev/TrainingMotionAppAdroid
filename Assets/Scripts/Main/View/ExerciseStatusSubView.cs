using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.erik.training.controller;


namespace com.erik.training.view
{
	public class ExerciseStatusSubView : MonoBehaviour {
		
		public Image imageStartPos;
		public Image imageEndPos;
		public Text  textRepetitions;
		public Text  textDuration;
				
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void SetImage(Sprite sprite)
		{
			imageStartPos.sprite = sprite;
		}

		public void SetRepetition(int count)
		{
			textRepetitions.text = count.ToString();
		}

		public void SetDuration(float totalTime)
		{
			totalTime += Time.deltaTime;
			int seconds = ((int)totalTime) % 60;
			int minutes = ((int)totalTime) / 60;
			string time = minutes + ":" + seconds;
			
			textDuration.text = time;
		}
	}

}
