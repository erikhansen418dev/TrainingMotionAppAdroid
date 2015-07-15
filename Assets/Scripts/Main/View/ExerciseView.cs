using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.erik.training.model;
using com.erik.training.controller;

using Xtr3D.Net.ColorImage;
using Xtr3D.Net.ExtremeMotion;
using Xtr3D.Net.ExtremeMotion.Data;
using Xtr3D.Net.ExtremeMotion.Gesture;

namespace com.erik.training.view
{
	public class ExerciseView : MonoBehaviour {
		
		public delegate void ExerciseEventDelegate(int count, float duration);
		public static event ExerciseEventDelegate OnFinish;	
		public static event ExerciseEventDelegate OnGoCalibration;

		public ExercisePopupView popupView;

		public ExerciseStatusSubView statusSubview;
		public Button buttonFinish;

		public Text textRepetition;
		public Text textDuration;
		
		private ExerciseData exeData;

		private int count = 0;
		private float totalTime = 0;
		
		
		private bool strokeCompleted = true;
		private bool inStroke = false;
		private bool isTraining = false;

		private TrackingState lastEngineState = TrackingState.Tracked;
		private TrackingState currentEngineState = TrackingState.Tracked;
		
		// Use this for initialization
		void Start () {

			ExtremeMotionEventsManager.MyGesturesFrameReadyHandler += MyGestureFrameReadyEventHandler;
			ExtremeMotionEventsManager.MyDataFrameReadyHandler += MyDataFrameReadyEventHandler;

			buttonFinish.onClick.AddListener (OnButtonFinish);
			Init ();
		}

		private void MyGestureFrameReadyEventHandler(object sender, GesturesFrameReadyEventArgs e)
		{
			
			// Opening the received frame
			using (var gesturesFrame = e.OpenFrame() as GesturesFrame)
			{
				
				if (gesturesFrame != null) 
				{
					BaseGesture[] gestures = gesturesFrame.FirstSkeletonGestures();
					foreach (BaseGesture gesture in gestures)
					{
						Debug.Log("GestureID : " + gesture.ID);
						
						switch(gesture.ID)
						{
						case 1:
							inStroke = true;
							break;
						case 2:
							if(inStroke)
							{
								inStroke = false;
								OnOneStrokeCompeted();
							}
							break;
						default:
							break;
							
						}
					}
				}   
				
			}
		}	

		
		// Update is called once per frame
		void Update () {

			if (! isTraining) {
				
				return;
			}

			if(lastEngineState != currentEngineState)
			{
				currentEngineState = lastEngineState;
				
				switch (currentEngineState) {
					
				case  TrackingState.Tracked:
					break;
				case  TrackingState.Calibrating:
					break;
				case TrackingState.NotTracked:
					OnLostTracking();
					break;
				default:
					break;
				}
			}

			textRepetition.text = count.ToString();
			
			totalTime += Time.deltaTime;
			int seconds = ((int)totalTime) % 60;
			int minutes = ((int)totalTime) / 60;
			string time = minutes + ":" + seconds;
			
			textDuration.text = time;
		}


		private void MyDataFrameReadyEventHandler(object sender, DataFrameReadyEventArgs e)
		{
			using (DataFrame dataFrame = e.OpenFrame() as DataFrame)
			{
				if (dataFrame != null)
				{
					lastEngineState = dataFrame.Skeletons[0].TrackingState;
				}
			}
		}

		void OnLostTracking()
		{
			Debug.Log("Tracking Lost");

			popupView.OnCloseWith += HandleOnPopupCloseWith;
			popupView.gameObject.SetActive (true);

			StopTraining();
		}

		void HandleOnPopupCloseWith (bool bForCalibrate)
		{
			popupView.OnCloseWith -= HandleOnPopupCloseWith;
			popupView.gameObject.SetActive(false);
		
			if (bForCalibrate) {			
				Debug.Log("User is going to Calibration");
				if(OnGoCalibration != null)
					OnGoCalibration(count,totalTime);

			} else {			
				Debug.Log("User is going to Finish Exercise");
				if (OnFinish != null)
					OnFinish (count, totalTime);
			}
			
		}

		void OnOneStrokeCompeted()
		{
			count++;
			Debug.Log ("OnOneStrokeCompeted  : " + count);
		}


		void OnApplicationPause(bool paused) 
		{
			if (paused) {
				ExtremeMotionEventsManager.MyGesturesFrameReadyHandler -= MyGestureFrameReadyEventHandler;
				ExtremeMotionEventsManager.MyDataFrameReadyHandler -= MyDataFrameReadyEventHandler;
			}
			else 
			{
				ExtremeMotionEventsManager.MyGesturesFrameReadyHandler += MyGestureFrameReadyEventHandler;
				ExtremeMotionEventsManager.MyDataFrameReadyHandler += MyDataFrameReadyEventHandler;
			}
		}
		
		private void Init ()
		{
			exeData = DataController.Instance.GetData ();
			Debug.Log (exeData.image.name);
			statusSubview.SetImage (exeData.image);

			count = exeData.repetition;
			totalTime = exeData.duration;

			popupView.gameObject.SetActive (false);

			StartTraining ();
		}

		private void StopTraining()
		{
			isTraining = false;
		}

		private void StartTraining()
		{
			isTraining = true;
		}
		
		public void OnButtonFinish()
		{
			if (OnFinish != null)
				OnFinish (count, totalTime);
		}
		
		public void Show()
		{
			gameObject.SetActive (true);
		}
		
		public void Hide()
		{
			gameObject.SetActive (false);			
		}
	}	
}