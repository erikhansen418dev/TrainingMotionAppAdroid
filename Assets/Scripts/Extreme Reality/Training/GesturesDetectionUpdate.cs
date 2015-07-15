using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using com.erik.training.controller;
using Xtr3D.Net.ColorImage;
using Xtr3D.Net.ExtremeMotion;
using Xtr3D.Net.ExtremeMotion.Data;
using Xtr3D.Net.ExtremeMotion.Gesture;

public class GesturesDetectionUpdate : MonoBehaviour {

	public delegate void GestureDetectionEventDelegate();
	public static event GestureDetectionEventDelegate OnPerformedOnce;

	public Text textRepetition;
	public Text textDuration;
	private int count = 0;
	private float totalTime = 0;


	private bool strokeCompleted = true;
	private bool inStroke = false;


	//code sample of getting gestures from xtr3d engine. These gestures are defined inside SamplePoses.xml
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

	void OnOneStrokeCompeted()
	{
		count++;
		Debug.Log ("OnOneStrokeCompeted  : " + count);
	}

	void OnApplicationPause(bool paused) 
	{
		if (paused) {
			ExtremeMotionEventsManager.MyGesturesFrameReadyHandler -= MyGestureFrameReadyEventHandler;
		}
		else 
		{
			ExtremeMotionEventsManager.MyGesturesFrameReadyHandler += MyGestureFrameReadyEventHandler;
		}
	}

	void Update () 
	{
		textRepetition.text = count.ToString();

		totalTime += Time.deltaTime;
		int seconds = ((int)totalTime) % 60;
		int minutes = ((int)totalTime) / 60;
		string time = minutes + ":" + seconds;

		textDuration.text = time;

	}

	private void OnExerciseStop()
	{
//		DataController.Instance.SetData()
	}

	public void Start()
	{

		ExtremeMotionEventsManager.MyGesturesFrameReadyHandler += MyGestureFrameReadyEventHandler;
/*		if (Application.platform == RuntimePlatform.IPhonePlayer
		    || Application.platform == RuntimePlatform.Android)
		{
			OnApplicationPause(false);
		}
*/
	}

}