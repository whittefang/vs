using UnityEngine;
using System.Collections;

public class ColorPreviewScript : MonoBehaviour {
	public PaletteArray[] characterColorPalettes;
	public int spot = 0;
	public ColorPaletteSwap[] neutralPreview, selectedPreview;

	public TextMesh colorNumberText;

	[System.Serializable]
	public class PaletteArray{
		public Texture2D[] characterPalette;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void UpdateSelection(int character, int direction){
		int oldSpot = spot;
		spot += direction;
		if (spot >= characterColorPalettes [character].characterPalette.Length) {
			spot = characterColorPalettes [character].characterPalette.Length - 1;
		}else if (spot <= 0){
			spot = 0;
		}
		Debug.Log (character);
		colorNumberText.text = spot.ToString();

		if (oldSpot != spot && characterColorPalettes [character].characterPalette [spot] != null) {
			neutralPreview [character].LoadColors (characterColorPalettes [character].characterPalette [spot]);
			selectedPreview [character].LoadColors (characterColorPalettes [character].characterPalette [spot]);

			neutralPreview [character].gameObject.SetActive (!neutralPreview [character].gameObject.activeSelf);
			neutralPreview [character].gameObject.SetActive (!neutralPreview [character].gameObject.activeSelf);
			selectedPreview [character].gameObject.SetActive (!selectedPreview [character].gameObject.activeSelf);
			selectedPreview [character].gameObject.SetActive (!selectedPreview [character].gameObject.activeSelf);
		}
	}
}
