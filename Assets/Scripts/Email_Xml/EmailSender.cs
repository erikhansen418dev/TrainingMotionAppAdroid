using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class EmailSender : MonoBehaviour 
{
	public InputField inputEmailAddressTo;
	public Button btnSend;
	public Text textOfBtnSend;


	private int count = 1;
	private string status = "ok"; 
	private string emailAddrTo = "";
	private string screenshotFileName = "";

	
	public void Start()
	{

	}
	
	IEnumerator ScreenshotEncode()
	{

		// We should only read the screen after all rendering is complete
		yield return new WaitForEndOfFrame();

		screenshotFileName = Application.dataPath + "/../RangeOfMotionAppChart-" + DateTime.Now.ToString("yyyy_dd_M-HH_mm_ss") + ".png";
		Debug.LogError (screenshotFileName);
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		
		// save our test image (could also upload to WWW)
		File.WriteAllBytes(screenshotFileName, bytes);
		// count++;
		
		DestroyObject(tex);
		
//		Debug.Log(Application.dataPath + "/../test-" + timeNow + ".png");

		MailMessage mail = new MailMessage();
		
		mail.From = new MailAddress("rangeofmotion.xtr3d@gmail.com");
		mail.To.Add(emailAddrTo);
		mail.Subject = "Range of Motion app - Do not reply ";
		mail.Body = "";
		
		mail.Attachments.Add(new Attachment(screenshotFileName, @"image/png"));
		
		SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
		smtpServer.Port = 587;
		smtpServer.Credentials = new System.Net.NetworkCredential("rangeofmotion.xtr3d@gmail.com", "rangeofmotionxtr3d") as ICredentialsByHost;
		smtpServer.EnableSsl = true;
		ServicePointManager.ServerCertificateValidationCallback = 
			delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
		{
			return true; 
		};
		smtpServer.Send(mail);
		Debug.Log("success");
		status = "ok";

		Debug.LogError("Sent..");

	}


	public void OnSendEmail()
	{
		emailAddrTo = inputEmailAddressTo.text;
		if (emailAddrTo == "") {
			Debug.LogError("Email Address is Empty! Please enter the Email Address ");
			return;
		}

		StartCoroutine(ScreenshotEncode());
	}
}