using UnityEngine;
using System.Collections;
using com.erik.util;
using com.erik.training.model;
using com.erik.training.view;

namespace com.erik.training.controller{

	public class RS_Calibration : RunState {
		
		// Use this for initialization
		void Start () {

			ViewController.OnReady += HandleOnCalibrationViewReady;
			ViewController.Instance.SetViewState (ViewState.VS_CALIBRATION);
		}		

		private void HandleOnCalibrationViewReady()
		{
			ViewController.OnReady -= HandleOnCalibrationViewReady;

//			CalibrationView.OnGoHome += HandleOnGoHome;
			CalibrationView.OnCalibrationSuccess += HandleOnCalibrationSuccess;		
			AddFramePanelViewEvents ();
		}

		void HandleOnCalibrationSuccess ()
		{
			CalibrationView.OnCalibrationSuccess -= HandleOnCalibrationSuccess;
//			CalibrationView.OnGoHome -= HandleOnGoHome;
			RemoveFramePanelVeiwEvents ();

			nextState = typeof(RS_Exercise);
			GoNext ();
		}

/*		private void HandleOnGoHome()
		{
			CalibrationView.OnCalibrationSuccess -= HandleOnCalibrationSuccess;
			CalibrationView.OnGoHome -= HandleOnGoHome;

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
			CalibrationView.OnCalibrationSuccess -= HandleOnCalibrationSuccess;
			RemoveFramePanelVeiwEvents ();

			Debug.Log("APP Quit");
			Application.Quit ();
		}
		
		void HandleOnGoUserInfo ()
		{
			CalibrationView.OnCalibrationSuccess -= HandleOnCalibrationSuccess;
			RemoveFramePanelVeiwEvents ();
			
			nextState = typeof(RS_Register);
			GoNext ();
		}
		
		void HandleOnGoHome ()
		{
			CalibrationView.OnCalibrationSuccess -= HandleOnCalibrationSuccess;
			RemoveFramePanelVeiwEvents ();
			
			nextState = typeof(RS_Home);
			GoNext ();
		}	
		//////////

	}

}
