using UnityEngine;
using System.Collections;
using com.erik.util;
using com.erik.training.model;
using com.erik.training.view;

namespace com.erik.training.controller{

	public class RS_Tutorial : RunState {
		
		// Use this for initialization
		void Start () {

			ViewController.OnReady += HandleOnTutorialViewReady;
			ViewController.Instance.SetViewState (ViewState.VS_TUTORIAL);				
		}

		void HandleOnTutorialViewReady ()
		{
			ViewController.OnReady -= HandleOnTutorialViewReady;
			TutorialView.OnPresentationCompleted += HandleOnTutorialPresentationCompleted;
			AddFramePanelViewEvents ();
			
		}

		void HandleOnTutorialPresentationCompleted ()
		{
			TutorialView.OnPresentationCompleted -= HandleOnTutorialPresentationCompleted;
			RemoveFramePanelVeiwEvents ();

			nextState = typeof(RS_Calibration);
			GoNext ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}


		/// <summary>
		/// 		/// </summary>
		public void AddFramePanelViewEvents()
		{
			FramePanelView.OnGoHome += HandleOnGoHome;
			FramePanelView.OnGoUserInfo += HandleOnGoUserInfo;
			FramePanelView.OnAppExit += HandleOnAppExit;
		}
		
		public void RemoveFramePanelVeiwEvents()
		{
			FramePanelView.OnGoHome -= HandleOnGoHome;
			FramePanelView.OnGoUserInfo -= HandleOnGoUserInfo;
			FramePanelView.OnAppExit -= HandleOnAppExit;
			
		}
		
		void HandleOnAppExit ()
		{
			TutorialView.OnPresentationCompleted -= HandleOnTutorialPresentationCompleted;
			RemoveFramePanelVeiwEvents ();

			Debug.Log("APP Quit");
			Application.Quit ();
		}
		
		void HandleOnGoUserInfo ()
		{
			TutorialView.OnPresentationCompleted -= HandleOnTutorialPresentationCompleted;
			RemoveFramePanelVeiwEvents ();
			
			nextState = typeof(RS_Register);
			GoNext ();
		}
		
		void HandleOnGoHome ()
		{
			TutorialView.OnPresentationCompleted -= HandleOnTutorialPresentationCompleted;
			RemoveFramePanelVeiwEvents ();
			
			nextState = typeof(RS_Home);
			GoNext ();
		}	
		//////////
	}

}

