using UnityEngine;
using System.Collections;
using com.erik.util;
using com.erik.training.model;
using com.erik.training.view;

namespace com.erik.training.controller{

	public class RS_Register : RunState {
		
		// Use this for initialization
		void Start () {
			
			ViewController.OnReady += HandleOnRegisterViewReady;
			ViewController.Instance.SetViewState (ViewState.VS_REGISTER);	
		}
		
		void HandleOnRegisterViewReady ()
		{
			ViewController.OnReady 	-= HandleOnRegisterViewReady;
			RegisterView.OnRegisterUserSuccess 	+= HandleOnRegisterUserSuccess;
			AddFramePanelViewEvents ();
		}
		
		void HandleOnRegisterUserSuccess (User userInfo)
		{
			RegisterUser (userInfo);

			RegisterView.OnRegisterUserSuccess 	-= HandleOnRegisterUserSuccess;
			RemoveFramePanelVeiwEvents ();

			nextState = typeof(RS_Home);
			GoNext ();
		}
		
		private void RegisterUser(User _user)
		{
			Debug.Log ("Registering User Info ...");
			
			PlayerPrefs.SetString (Constants.USER_FIRST_NAME_KEY, _user.firstName);
			PlayerPrefs.SetString (Constants.USER_LAST_NAME_KEY, _user.lastName);
			PlayerPrefs.SetString (Constants.USER_EMAIL_KEY, _user.email);
			
			PlayerPrefs.SetInt (Constants.APP_USE_STATE_KEY, 1);

			User user = new User (); 	
			user.firstName 	= PlayerPrefs.GetString (Constants.USER_FIRST_NAME_KEY);
			user.lastName 	= PlayerPrefs.GetString (Constants.USER_LAST_NAME_KEY);
			user.email		= PlayerPrefs.GetString (Constants.USER_EMAIL_KEY);
			
			UserData.SetUser (user);
			
			Debug.Log ("User Registered ...");
		}


		/// <summary>
		/// 		/// </summary>
		public void AddFramePanelViewEvents()
		{
			FramePanelView.OnAppExit += HandleOnAppExit;
		}
		
		public void RemoveFramePanelVeiwEvents()		{

			FramePanelView.OnAppExit -= HandleOnAppExit;			
		}
		
		void HandleOnAppExit ()
		{
			RegisterView.OnRegisterUserSuccess 	-= HandleOnRegisterUserSuccess;		
			RemoveFramePanelVeiwEvents ();

			Debug.Log("APP Quit");
			Application.Quit ();
		}		

		//////////
		
	}
}

