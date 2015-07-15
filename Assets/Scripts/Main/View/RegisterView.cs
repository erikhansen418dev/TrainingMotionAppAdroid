using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.erik.training.model;

namespace com.erik.training.view{

	public class RegisterView : MonoBehaviour {
		
		public delegate void RegisterViewEventDelegate(User userInfo);
		public static event RegisterViewEventDelegate OnRegisterUserSuccess;
		
		public InputField inputFirstName;
		public InputField inputLastName;
		public InputField inputEmail;
		public Button buttonNext;
		public Text textMsg;
		
		private string message = string.Empty;
		
		// Use this for initialization
		void Start () {
			buttonNext.onClick.AddListener (OnNext);
			SetDefaultValue ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
		void OnNext()
		{
			string firstName = inputFirstName.text;
			if (string.IsNullOrEmpty (firstName)) {
				message = "First Name Input Error ";
				textMsg.text = message;
				return;
			}
			
			string lastName = inputLastName.text;
			if (string.IsNullOrEmpty (lastName)) {
				message = "Last Name Input Error ";
				textMsg.text = message;
				return;
			}
			
			string email = inputEmail.text;
			if (string.IsNullOrEmpty (email)) {
				message = "Email Input Error ";
				textMsg.text = message;
				return;
			}
			
			User user = new User (firstName, lastName, email);
			
			if (OnRegisterUserSuccess != null)
				OnRegisterUserSuccess (user);
			
		}

		void SetDefaultValue()
		{
			if(!string.IsNullOrEmpty(UserData.user.firstName))
				inputFirstName.text = UserData.user.firstName;

			if(!string.IsNullOrEmpty(UserData.user.lastName))
				inputLastName.text = UserData.user.lastName;

			if(!string.IsNullOrEmpty(UserData.user.email))
				inputEmail.text = UserData.user.email;
		}

		// for administrator
		
		private bool IsAdministrator(string firstName, string lastName)
		{
			return true;
		}
	}




}

