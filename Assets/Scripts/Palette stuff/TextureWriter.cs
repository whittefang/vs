using UnityEngine;
using System.Collections;
using System.IO;

public class TextureWriter : MonoBehaviour {
	public Texture2D newPalette;
	public Texture2D originalColors;
	public SpriteRenderer targetSprite;
	public MeshRenderer newPalettePreview;
	public string fileName;
	Color lastColor;
	int lastSpot = 0;
	// Use this for initialization
	void Start () {
		//SR = GetComponent<SpriteRenderer> ();
		MakeNewTexture();

		//SaveTexture ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F1)) {
			SaveTexture ();
		}
		if (Input.GetKeyDown (KeyCode.F2)) {
			for (int i = 0; i < 64; i++) {

				SetPixel (i, new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f), 1));
			}
		}
	}
	public void MakeNewTexture(){
		newPalette = new Texture2D (64, 1, TextureFormat.ARGB32, false, false);
		newPalette.filterMode = FilterMode.Point;
		newPalette.SetPixels (originalColors.GetPixels ());
		newPalette.Apply();
		targetSprite.material.SetTexture("_ColorPaletteMorph", newPalette);
		newPalettePreview.material.mainTexture = newPalette;
	}
	public void SetPixel(int x, Color newCol){
		newPalette.SetPixel (x, 0, newCol);
		lastColor = newCol;
		newPalette.Apply();

	}
	public void SaveTexture(){
		// Encode texture into PNG
		byte[] bytes = newPalette.EncodeToPNG();

		File.WriteAllBytes(Application.dataPath + "/shaderPalettes/" + fileName + ".png", bytes);
	}
	public void ResetColor (int x){
		newPalette.SetPixel (x, 0, originalColors.GetPixel (x, 0));
		newPalette.Apply();
	}
	public void ShowPixels(int x){
		newPalette.SetPixel (lastSpot, 0, lastColor);
		lastColor = newPalette.GetPixel (x, 0);
		lastSpot = x;
		newPalette.SetPixel (x, 0, new Color(1,.1f,1,1));
		newPalette.Apply();
	}
}
