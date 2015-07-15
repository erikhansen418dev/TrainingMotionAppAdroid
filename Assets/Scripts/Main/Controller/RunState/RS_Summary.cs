using UnityEngine;
using System.Collections;
using com.erik.util;
using com.erik.training.model;
using com.erik.training.view;


namespace com.erik.training.controller
{
	public class RS_Summary : RunState {
		
		// Use this for initialization
		void Start () {
			
			ViewController.OnReady += HandleOnSummaryViewReady;
			ViewController.Instance.SetViewState (ViewState.VS_SUMMARY);
			
		}

		void HandleOnSummaryViewReady ()
		{
			ViewController.OnReady -= HandleOnSummaryViewReady;
			SummaryView.OnGOHome += HandleOnGoHome;
			AddFramePanelViewEvents ();

		}


/*		void HandleOnGOHome ()
		{
			SummaryView.OnGOHome -= HandleOnGOHome;
			RemoveFramePanelVeiwEvents ();
			nextState = typeof(RS_Home);
			GoNext ();
		}
*/

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
			SummaryView.OnGOHome -= HandleOnGoHome;
			RemoveFramePanelVeiwEvents ();

			Debug.Log("APP Quit");
			Application.Quit ();
		}
		
		void HandleOnGoUserInfo ()
		{
			SummaryView.OnGOHome -= HandleOnGoHome;
			RemoveFramePanelVeiwEvents ();
			
			nextState = typeof(RS_Register);
			GoNext ();
		}
		
		void HandleOnGoHome ()
		{
			SummaryView.OnGOHome -= HandleOnGoHome;
			RemoveFramePanelVeiwEvents ();
			
			nextState = typeof(RS_Home);
			GoNext ();
		}	
		//////////
		
	}
}

