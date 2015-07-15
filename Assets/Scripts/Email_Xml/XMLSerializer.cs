using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using com.erik.training.model;
using com.erik.util;

namespace com.erik.training.model{

	[XmlRoot("ExerciseResult")]
	public class ExerciseResult
	{
		public string FirstName;	
		public string FamilyName;	
		public string Email;
		
		public string DateTime;
		
		public string Exercise;	
		public int Repetitions;
		public string Duration; 



		public ExerciseResult(User user, ExerciseData exeData)
		{
			FirstName = user.firstName;
			FamilyName = user.lastName;
			Email = user.email;

			DateTime = System.DateTime.Now.ToString();

			Exercise = exeData.title;
			Repetitions = exeData.repetition;
			Duration = Extention.SecondToMMSS (exeData.duration);
		}

		public ExerciseResult()
		{

		}

		public void Save(string path)
		{
			var serializer = new XmlSerializer(typeof(ExerciseResult));
			using(var stream = new FileStream(path, FileMode.Create))
			{
				serializer.Serialize(stream, this);
			}
		}
	}

}
