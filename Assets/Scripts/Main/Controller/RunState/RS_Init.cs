using UnityEngine;
using System.Collections;
using com.erik.util;
using com.erik.training.model;

namespace com.erik.training.controller{

	public class RS_Init : RunState {
		
		void Start()
		{
			Debug.Log("Start RS_Init");
			ViewController.OnReady += HandleOnViewControllerIsReady;
			ViewController _vc = ViewController.Instance;
		}
		
		void HandleOnViewControllerIsReady ()
		{
			ViewController.OnReady -= HandleOnViewControllerIsReady;
			
			if (IsFirstTime())
				OnFirstTime ();
			else
				OnNoFirstTime ();
		}
		
		
		private bool IsFirstTime()
		{
			int flag = PlayerPrefs.GetInt (Constants.APP_USE_STATE_KEY);
			Debug.Log ("firstTimeValue : " + flag);
			
			bool isFirst = true;
			if (flag == 1)
				isFirst = false;
			
			return isFirst;
		}
		
		
		private void OnFirstTime()
		{
			nextState = typeof(RS_Register);
			GoNext();
		}
		
		
		private void OnNoFirstTime()
		{
			GetUserInfo ();

			nextState = typeof(RS_Home);
			GoNext ();
		}

		void GetUserInfo()
		{
			Debug.Log ("Getting UserInfo ...");
			
			User user = new User (); 	
			user.firstName 	= PlayerPrefs.GetString (Constants.USER_FIRST_NAME_KEY);
			user.lastName 	= PlayerPrefs.GetString (Constants.USER_LAST_NAME_KEY);
			user.email		= PlayerPrefs.GetString (Constants.USER_EMAIL_KEY);
			
			UserData.SetUser (user);
			
			Debug.Log("Finished Getting UserInfo...");
			Debug.Log ("user info : " + UserData.user.ToString());
		}
	}

}
