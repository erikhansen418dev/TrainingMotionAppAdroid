using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using UnityEngine;

namespace com.erik.training.view{

	public class AnimatedGifDrawer : MonoBehaviour
	{

		public float speed = 0.2f;
		public Vector2 drawPosition;
		public UnityEngine.UI.RawImage image;

		List<Texture2D> gifFrames = new List<Texture2D>();

		private string loadingGifPath;
		private bool isDrawing = false;
		
		void Awake()
		{

		}

		void Update()
		{
			if(isDrawing)
			  	image.texture = gifFrames[(int)(Time.frameCount * speed) % gifFrames.Count];
		}

		private bool LoadingGifImage(string path)
		{
			Debug.Log("Loading Gif Image");
			if (string.IsNullOrEmpty (path)) {
			
				Debug.Log("Gif Image Path is not Set");
				return false;
			}

			var gifImage = Image.FromFile(path);

			if (gifImage == null) {
			
				Debug.Log("Loading Gif Image has failed");
				return false;
			}

			var dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
			int frameCount = gifImage.GetFrameCount(dimension);
			for (int i = 0; i < frameCount; i++)
			{
				gifImage.SelectActiveFrame(dimension, i);
				var frame = new Bitmap(gifImage.Width, gifImage.Height);
				System.Drawing.Graphics.FromImage(frame).DrawImage(gifImage, Point.Empty);
				var frameTexture = new Texture2D(frame.Width, frame.Height);
				for (int x = 0; x < frame.Width; x++)
					for (int y = 0; y < frame.Height; y++)
				{
					System.Drawing.Color sourceColor = frame.GetPixel(x, y);
					frameTexture.SetPixel(frame.Width - 1 - x, y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A)); // for some reason, x is flipped
				}
				frameTexture.Apply();
				gifFrames.Add(frameTexture);
			}

			return true;
		}

		public void StartDraw(string path)
		{
			if (LoadingGifImage (path)) {
				Debug.Log ("Start Draw");
				isDrawing = true;
			}
		}

		public void StopDraw()
		{
			isDrawing = false;
		}
	}
}

