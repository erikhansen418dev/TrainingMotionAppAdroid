using UnityEngine;
using System.Collections;

public class CS_Loading : MonoBehaviour {

	#region PUBLIC VARIABLES

	public string strNameNextScene;
	public Texture2D textureLoadingImage;
	public Texture2D texureSplashImage;

	private Texture2D textureToDraw;

	#endregion  // PUBLIC VARIABLES


	#region PRIVATE VARIABLES

	private AsyncOperation asyncLoading;
	private bool isLoading;

	#endregion PRIVATE VARIABLES


	#region // MOMOBEHAVIOR FUNCTIONS


	// Use this for initialization
	void Start () {

		if (textureLoadingImage == null) {			
			Debug.Log("Loading Screen Error : Loading Image was not Loaded from Resources. Please check path of the Loading Image");
			return;
		}

		if (string.IsNullOrEmpty (strNameNextScene)) {
			Debug.Log("Loading Screen Error : Name Of the Scene to be loaded was not set. Please check NameNextScene");
			return;
		}

		StartCoroutine ("StartLoading");
	}
	
	// Update is called once per frame
	void Update () {

		if (asyncLoading == null)
			return;

		if (asyncLoading.isDone) {
			StartCoroutine(NewSceneAfterDelay(0f));
		}
		else {

		}
	}

	void OnGUI()
	{
		if (isLoading) {
			GUI.DrawTexture (new Rect (0f, 0f, Screen.width, Screen.height), textureToDraw, ScaleMode.StretchToFill); 
		}
	}

	IEnumerator NewSceneAfterDelay(float delaySec)
	{
		yield return new WaitForSeconds (delaySec);
		isLoading = false;
		Destroy (this.gameObject);
	}

	IEnumerator StartLoading()
	{
		isLoading = true;

		ShowSplash ();
		yield return new WaitForSeconds (2f);

		ShowLoadingImage ();
		yield break;
	}

	void ShowSplash()
	{
		textureToDraw = texureSplashImage;
	}

	void ShowLoadingImage()
	{
		textureToDraw = textureLoadingImage;

		DontDestroyOnLoad (this);
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		asyncLoading = Application.LoadLevelAsync (strNameNextScene);
	}

	#endregion // MONOBEHAVIOR FUNCTIONS


	#region MEMBER METHODS



	#endregion // MEMBER METHODS

}
