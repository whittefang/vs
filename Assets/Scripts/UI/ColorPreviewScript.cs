using UnityEngine;
using System.Collections;

public class ColorPreviewScript : MonoBehaviour {
	public PaletteArray[] characterColorPalettes;
	int[] spot;
	public ColorPaletteSwap[] neutralPreview, selectedPreview;

	public TextMesh colorNumberText;

	[System.Serializable]
	public class PaletteArray{
		public Texture2D[] characterPalette;
	}
	// Use this for initialization
	void Awake () {
		spot = new int[7];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void UpdateSelection(int character, int direction){
		int oldSpot = spot[character];
		spot[character] += direction;
		if (spot[character] >= characterColorPalettes [character].characterPalette.Length) {
			spot[character] = characterColorPalettes [character].characterPalette.Length - 1;
		}else if (spot[character] <= 0){
			spot[character] = 0;
		}
		colorNumberText.text = spot[character].ToString();

		if (oldSpot != spot[character] && characterColorPalettes [character].characterPalette [spot[character]] != null ) {
			neutralPreview [character].LoadColors (characterColorPalettes [character].characterPalette [spot[character]]);
			selectedPreview [character].LoadColors (characterColorPalettes [character].characterPalette [spot[character]]);

			neutralPreview [character].gameObject.SetActive (!neutralPreview [character].gameObject.activeSelf);
			neutralPreview [character].gameObject.SetActive (!neutralPreview [character].gameObject.activeSelf);
			selectedPreview [character].gameObject.SetActive (!selectedPreview [character].gameObject.activeSelf);
			selectedPreview [character].gameObject.SetActive (!selectedPreview [character].gameObject.activeSelf);
		}
	}
	public int GetColorNumber(int character){
		return spot[character];
	}
}
