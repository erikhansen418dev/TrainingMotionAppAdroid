using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using com.erik.util;

namespace com.erik.util{

    interface IRunState
    {
        void UpdateState();
    }

    public class RunState : Singleton<RunState>, IRunState {

        public static bool isDone { get { return Instance.isInstanceDone; } }
        public static void Done() { Instance.isInstanceDone = true; }

        protected bool isInstanceDone = false;
        
        public static Type nextState; // the TYPE of the previous run state
        public static Type previous; // the TYPE of the next run state
        public static RunState current; // direct reference to the current run state
        
        bool Error = false;
        
		public virtual void Start()
        {
            
            name = this.GetType().ToString() ;
            
            RunStateChangeEvent+=ReportStateChange;
            
            Debug.Log("State successfully changed from: " + previous + "  to " + this.GetType() + ".");
            
			Init();

        }
        
        // Update is called once per frame
        void Update()
        {
            
            if (!isInstanceDone)
                UpdateState();
            else if (!Error)
                GoNext();
            
        }
        
        public void GoNext()
        {
            if (nextState != null)
            {
                try
                {
                    RunState _newState = (RunState)gameObject.AddComponent(nextState);
                    
                    Debug.Log("Changing state from " + this.GetType() + " to " + nextState.ToString());
                    
                    RunState.current = _newState;
                    
                    _newState.name = "Run State: " + nextState.ToString();
                    
                    RunState.previous = this.GetType();
                    
                    // fire event
                    if(RunStateChangeEvent!= null)
                        RunStateChangeEvent(_newState);
                    
					Dispose();
                    
                }
                catch
                {
                    Debug.LogError("ERROR: Cannot use state: " + nextState.ToString() + "\nAre you sure it inherits from RunState and all events are decoupled?");
                }
                
            }
            else
            {
                Error = true;
                Debug.Log("ERROR! The next state has not been defined!");
            }
            
        }
        
        public void GoPrevious() {
            
            if(previous!=null) {
                nextState = previous;
                GoNext();
            } else {
                Debug.Log("ERROR: No previous state defined!");
            }
            
        }
        
        private void ReportStateChange(RunState newState) {
            
            Debug.Log("Changed state from " + previous.ToString() + " to " + current.ToString());
            
        }
        
		public virtual void Dispose()
		{

			Destroy(this); // should destroy just the component, not the gameobject

		}

        public virtual void UpdateState()
        {
        }
        
        #region Events
        
        // runstate changes
        public delegate void RunStateChangeEventDelegate(RunState runState);
        public static event RunStateChangeEventDelegate RunStateChangeEvent;
        
        #endregion
        
    }

}