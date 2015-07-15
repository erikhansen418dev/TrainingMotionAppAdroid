using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.erik.training.model;
using com.erik.training.controller;

namespace com.erik.training.view{

	public class TutorialView : MonoBehaviour {
		public delegate void TutorialViewEventDelegate();
		public static event TutorialViewEventDelegate OnPresentationCompleted;

		public float timeCount = 10f;
		public string gifFolerPath;
		public CircleTimer circleTimer;
		public AnimatedGifDrawer gifDrawer;

		// Use this for initialization
		void Start () {	

			circleTimer.OnTimerEnd += HandleOnTimerEnd;
			circleTimer.StartCountTime (timeCount);

			string gifFilePath = System.IO.Path.Combine(Application.streamingAssetsPath, DataController.Instance.GetData ().tutorialGifName);
//			string gifFilePath = gifFolerPath + DataController.Instance.GetData ().tutorialGifName;

			//"Assets\\Graphics\\Tutorial_Animation\\Exercise1Gif.gif"
			gifDrawer.StartDraw (gifFilePath);
			
		}

		void HandleOnTimerEnd ()
		{
			circleTimer.OnTimerEnd -= HandleOnTimerEnd;

			gifDrawer.StopDraw ();

			if (OnPresentationCompleted != null)
				OnPresentationCompleted ();
		}	
		
		// Update is called once per frame
		void Update () {
			
		}
	}


}

