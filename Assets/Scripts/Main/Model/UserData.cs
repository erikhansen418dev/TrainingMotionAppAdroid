using UnityEngine;
using System.Collections;

public static class UserData
{
	public static User user = new User();

	public static void SetUser(User _user)
	{
		user.firstName = _user.firstName;
		user.lastName = _user.lastName;
		user.email = _user.email;
	}
}

public class User
{
	public string firstName;
	public string lastName;
	public string email;

	public User(){}
	
	public User(string _firstName, string _lastName, string _email)
	{
		firstName = _firstName;
		lastName = _lastName;
		email = _email;
	}	

	public string ToString()
	{
		string strUser = string.Format ("FistName : {0}, LastName : {1}, Email : {2}", firstName, lastName, email);  
		return strUser;
	}
}

