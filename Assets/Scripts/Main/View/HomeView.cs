using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.erik.training.model;

namespace com.erik.training.view{

	public class HomeView : MonoBehaviour {

		public delegate void HomeViewEventDelegate (ExerciseData exData);
		public static event HomeViewEventDelegate OnEnterExercise;

		public List<ExerciseEntry> listExerciseEntries;
		
		// Use this for initialization
		void Start () {

			int index = 0;
			foreach (ExerciseEntry e_entry in listExerciseEntries) {
			
				e_entry.OnEntrySelected += HandleOnEntrySelected;
				e_entry.SetIndex(index);
				index ++;
			}
			
		}

		void HandleOnEntrySelected (int index)
		{	

			foreach (ExerciseEntry e_entry in listExerciseEntries) {
				
				e_entry.OnEntrySelected -= HandleOnEntrySelected;
			}

			ExerciseData exData = listExerciseEntries [index].exerciseData;
			Debug.Log ("HandleOnEntrySelected.. : " + exData.ToString());

			if (OnEnterExercise != null)
				OnEnterExercise (exData);
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}

} 


