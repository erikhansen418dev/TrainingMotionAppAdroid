using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.erik.training.view{

	public class CircleTimer : MonoBehaviour {

		public delegate void CircleTimerEventDelegate();
		public event CircleTimerEventDelegate OnTimerEnd;

		public Image thinCircle;
		public Image ThickCircle;
		public Image ball;	
		public Text textTimer;

		private const float pi = 3.141592654f;
		private float ball_R = 43.0f;
		private float timeCount = 5f;
		private float timer = 0f;
		private bool isCounting = false;
		private float angleRad = 0f;
		
		// Use this for initialization
		void Start () {
			Init();
		}
		
		// Update is called once per frame
		void Update () {

			if (timer >= timeCount) {
				isCounting = false;
				timer = 0;

				if(OnTimerEnd != null)
					OnTimerEnd();
			}

			if (! isCounting)
				return;

			UpdateTimer ();

		}

		public void StartCountTime(float time)
		{
			isCounting = true;
			timeCount = time;
		}


		private void UpdateTimer()
		{
			timer += Time.deltaTime;

			angleRad = 2* pi * timer / timeCount;
			ball.rectTransform.localPosition = new Vector3(ball_R * Mathf.Sin(angleRad), ball_R * Mathf.Cos(angleRad));

			ThickCircle.fillAmount = timer / timeCount;

			float timeRemaining = timeCount - timer + 1;
			textTimer.text = ((int)timeRemaining).ToString("00");
		}	

		private void Init()
		{
			timer = 0f;
			UpdateTimer ();
		}


	}

}
