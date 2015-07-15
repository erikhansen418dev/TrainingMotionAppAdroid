using UnityEngine;
using System.Collections;
using com.erik.util;
using com.erik.training.model;
using com.erik.training.view;

namespace com.erik.training.controller{

	public class RS_Home : RunState {
		
		// Use this for initialization
		void Start () {
			
			Debug.Log("RS_Home start");
			
			ViewController.OnReady += HandleOnHomeViewReady;
			ViewController.Instance.SetViewState (ViewState.VS_HOME);
		}
		
		void HandleOnHomeViewReady ()
		{
			ViewController.OnReady -= HandleOnHomeViewReady;
			HomeView.OnEnterExercise += HandleOnEnterExercise;
			AddFramePanelViewEvents ();
		}

		void HandleOnEnterExercise (ExerciseData exData)
		{
			HomeView.OnEnterExercise -= HandleOnEnterExercise;
			RemoveFramePanelVeiwEvents ();

			Debug.Log (exData.ToString ());

			DataController.OnUpdated += HandleOnDataUpdated;

			DataController.Instance.SetData (exData);
		}

		void HandleOnDataUpdated ()
		{
			DataController.OnUpdated -= HandleOnDataUpdated;

			nextState = typeof(RS_Tutorial);
			GoNext ();
		}

		/// <summary>
		/// 		/// </summary>
		public void AddFramePanelViewEvents()
		{
			FramePanelView.OnGoUserInfo += HandleOnGoUserInfo;
			FramePanelView.OnAppExit += HandleOnAppExit;
		}
		
		public void RemoveFramePanelVeiwEvents()
		{
			FramePanelView.OnGoUserInfo -= HandleOnGoUserInfo;
			FramePanelView.OnAppExit -= HandleOnAppExit;
			
		}
		
		void HandleOnAppExit ()
		{
			HomeView.OnEnterExercise -= HandleOnEnterExercise;
			RemoveFramePanelVeiwEvents ();

			Debug.Log("APP Quit");
			Application.Quit ();
		}
		
		void HandleOnGoUserInfo ()
		{
			HomeView.OnEnterExercise -= HandleOnEnterExercise;
			RemoveFramePanelVeiwEvents ();
			
			nextState = typeof(RS_Register);
			GoNext ();
		}		

		//////////
		
		
	}

}

