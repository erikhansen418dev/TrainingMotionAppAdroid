using UnityEngine;
using System.Collections;
using com.erik.util;
using com.erik.training.view;

namespace com.erik.training.controller{

	public class TrainingAppRunState : RunState {

		public void AddFramePanelViewEvents()
		{
			FramePanelView.OnGoHome += HandleOnGoHome;
		}

		public virtual void HandleOnGoHome ()
		{
			RemoveFramePanelVeiwEvents ();	
			nextState = typeof(RS_Home);
			GoNext ();
			Debug.Log("FramePanelView Events Removed" );
		}

		public void RemoveFramePanelVeiwEvents()
		{
			FramePanelView.OnGoHome -= HandleOnGoHome;

		}
	}
}
