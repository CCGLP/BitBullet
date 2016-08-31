using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;



public class TypeEffectScript : MonoBehaviour {
	public delegate void TypeCallback();

	private string nextString;
	private string[] badWord;
	[SerializeField]
	private GameObject prefabTextMesh;
	private int localBadWordIndex = 0; 
	private Text text; 
	private Tween tween; 
	public float timeBetweenLetters = 0.4f;
    private bool skipText = true;


	public void SetNextString(string sentence, string[] badWord = null, TypeCallback callBack = null){
		StaticConstants.badWordIndex = 0;
        skipText = false; 
		this.nextString = sentence;
		this.badWord = badWord;
		this.localBadWordIndex = 0;
		StartWritingLetters (callBack);

	}


	private void StartWritingLetters(TypeCallback callBack = null){


		tween = DOVirtual.DelayedCall (timeBetweenLetters, () => {
			WriteLetter (nextString.ToCharArray () [0], 0, callBack);
		}
			, false);

	}

	private void WriteLetter(char letter, int index, TypeCallback callBack = null){
		if (letter == 'º') {
			BaddieText baddie;
			baddie = ((GameObject)Instantiate (prefabTextMesh, StaticConstants.spawnPoints[Random.Range(0, StaticConstants.spawnPoints.Length)].position,
				prefabTextMesh.transform.rotation)).GetComponent<BaddieText> ();

			string realBadWord = "";
			int j = 0; 
			for (int i = 0; i < nextString.Length; i++) {
				if (nextString.ToCharArray () [i] != 'º') {
					realBadWord += ' ';
				} else {
					j++;
					if (j > localBadWordIndex)
						break;
				}
					
			}
			realBadWord = badWord[StaticConstants.badWordIndex];
			StaticConstants.badWordIndex++;
			localBadWordIndex++;

			Vector3 velocity = new Vector3 (Random.Range (0f, 1f), Random.Range (0f, 1f), 0).normalized * StaticConstants.actualQuestionSpeed;

			baddie.Initialize (realBadWord, velocity);
			for (int i = 0; i < badWord[StaticConstants.badWordIndex-1].Length; i++) {
				text.text += ' ';
			}


		} else {

			text.text += letter;
		}
        if ((index + 1) < nextString.Length)
        {
            if (!skipText)
                tween = DOVirtual.DelayedCall(timeBetweenLetters, () =>
                {
                    WriteLetter(nextString.ToCharArray()[index + 1], index + 1, callBack);
                });
            else
            {
                tween.Kill();
                WriteLetter(nextString.ToCharArray()[index + 1], index + 1, callBack);
            }
                
        }
        else
        {
            if (callBack != null)
                callBack();
            return;
        }
	}

	public void CleanText(){
		if (tween != null)
			tween.Kill ();
		this.text.text = "";
	}
	// Use this for initialization
	void Start () {
	
		this.text = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKeyDown)
        {
            skipText = true;
        }
	}
}
