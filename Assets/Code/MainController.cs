using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using SimpleJSON;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Kino;

public class MainController : MonoBehaviour {
	private TextAsset jsonText;
	[SerializeField]
	private TypeEffectScript mainText;
	[SerializeField]
	private float timeToContinueHistory = 3;

	private bool canInput = false; 
	[SerializeField]
	private float keyStrokeTime = 0.3f;

	[SerializeField]
	private float waitTime = 3f;

	private float timer = 0; 
	private TypeEffectScript[] questionTexts;
	private bool canContinueHistory = true;
	private bool delayedHistoryActive = false;
	private int indexQuestion = 0;
	private List<Question> questions;
	private Question actualQuestion;
	private DigitalGlitch cameraGlitch;
	private Text timerText;


	// Use this for initialization
	void Start () {
        jsonText = StaticConstants.jsonText;
		timerText = GameObject.Find ("Timer").GetComponent<Text> ();
		timerText.gameObject.SetActive (false);
		StaticConstants.numberOfBaddies = 0;
		StaticConstants.spawnMusic = GameObject.Find ("AudioShoot").GetComponent<AudioSource> ();
		StaticConstants.explosionMusic = GameObject.Find ("AudioExplosion").GetComponent<AudioSource> ();
		StaticConstants.particlePool = GameObject.Find ("Particles").GetComponentsInChildren<ParticleSystem> ();
		GameObject[] auxGarr = GameObject.FindGameObjectsWithTag ("spawn");
		StaticConstants.spawnPoints = new Transform[auxGarr.Length];
		for (int i = 0; i < auxGarr.Length; i++) {
			StaticConstants.spawnPoints [i] = auxGarr[i].transform;

		}
		string jsonString = jsonText.text;
		questions = new List<Question> ();
		cameraGlitch = Camera.main.GetComponent<DigitalGlitch> ();
		questionTexts = GameObject.Find ("Questions").transform.GetComponentsInChildren<TypeEffectScript> ();

		Question aux;
		for (int i = 0; i < questionTexts.Length; i++) {
			questionTexts [i] = GameObject.Find ("Questions").transform.FindChild (i.ToString ()).GetComponent<TypeEffectScript>();

		}


		JSONNode root = JSON.Parse (jsonString);

		if (root != null && root ["questions"] != null) {
			IEnumerable<JSONNode> questionsArray = root ["questions"].Childs;
			if (questionsArray != null) {
				Question auxQuestion;
				string[] options;
				foreach (JSONNode questionNode in questionsArray) {
					JSONArray arr = questionNode ["answers"].AsArray;
					options = new string[arr.Count];
					for (int i = 0; i < arr.Count; i++) {
						options [i] = arr [i];
					}
					arr = questionNode ["badWord"].AsArray;
					string[] badWordArr = new string[arr.Count];
					for (int i = 0; i < arr.Count; i++) {
						badWordArr [i] = arr [i];
					}
					auxQuestion = new Question (questionNode ["header"], options, int.Parse(questionNode["goodOption"]), 
						questionNode["goodAnswer"], questionNode["badAnswer"], 
						bool.Parse(questionNode["canContinue"]), badWordArr, int.Parse(questionNode["timeToPass"]), int.Parse(questionNode["speed"]));
					questions.Add (auxQuestion);
				}
			}
		}





	}


	private void EndGame(){
		mainText.CleanText ();
		mainText.SetNextString ("PROGRAM END. RESTARTING S00N", null, () => {SceneManager.LoadScene(1);});

	}
	
	// Update is called once per frame
	//Here goes the gameLoop!
	void Update () {
		if(canContinueHistory){
			timerText.gameObject.SetActive (false);
				canContinueHistory = false;
				delayedHistoryActive = false;
				actualQuestion = questions [indexQuestion];
			StaticConstants.actualQuestionSpeed = actualQuestion.GetSpeed ();
			if (indexQuestion >= 5 && indexQuestion <= 14) {
				cameraGlitch.intensity += 0.011f;
			} else
				cameraGlitch.intensity = 0;
			
				indexQuestion++;
				mainText.CleanText ();
			mainText.SetNextString (actualQuestion.GetHeader (), actualQuestion.GetBadWord (), () => {canInput = true; timer = 0; });

				int index = 0;

				string nextQuestion = actualQuestion.GetOption (index);

				while (nextQuestion != null) {
					index++;
					questionTexts [index].SetNextString (nextQuestion, actualQuestion.GetBadWord ());

					nextQuestion = actualQuestion.GetOption (index);
				}

		}

		timer += Time.deltaTime;

		if ((actualQuestion.GetTimeToPass() - timer) < 20) {
			if (!timerText.gameObject.activeSelf)
				timerText.gameObject.SetActive (true);
			timerText.text = "TIME TO RESTART: " + ((int)(actualQuestion.GetTimeToPass() - timer)).ToString ();
		}
		if (canInput && timer > actualQuestion.GetTimeToPass()) {
			EndGame ();
			timer = 0;
		}
		if (!canContinueHistory && !delayedHistoryActive && (canInput && StaticConstants.numberOfBaddies == 0)) {
			if (timer >keyStrokeTime) {
				if ((Input.GetKeyDown (KeyCode.Keypad0) || Input.GetKeyDown (KeyCode.Alpha0)) && actualQuestion.GetAnswer(0) != null) {
					timer = 0;
					canInput = false;

					if (actualQuestion.GetNumberOfOptions () >= 1) {
						if (actualQuestion.CanContinue (0)) {
							delayedHistoryActive = true;

						}
						else {
							DOVirtual.DelayedCall (waitTime * 3, EndGame);
						}
						mainText.CleanText ();
						mainText.SetNextString (actualQuestion.GetAnswer (0), null, () => {timer = 0; DOVirtual.DelayedCall(waitTime 	,()=>{ canContinueHistory = delayedHistoryActive;});});
						for (int i = 0; i < questionTexts.Length; i++) {
							questionTexts [i].CleanText ();
						}
						
					}
				} else if ((Input.GetKeyDown (KeyCode.Keypad1) || Input.GetKeyDown (KeyCode.Alpha1)) && actualQuestion.GetAnswer(1) != null) {
					timer = 0;
					canInput = false;

					if (actualQuestion.GetNumberOfOptions () >= 2) {
						if (actualQuestion.CanContinue (1)) {
							delayedHistoryActive = true;

						} else {
							DOVirtual.DelayedCall (waitTime * 3, EndGame);
						}

						mainText.CleanText ();
						mainText.SetNextString (actualQuestion.GetAnswer (1), null, () => {timer = 0; DOVirtual.DelayedCall(waitTime,()=>{ canContinueHistory = delayedHistoryActive;});});
						for (int i = 0; i < questionTexts.Length; i++) {
							questionTexts [i].CleanText ();
						}

					}
				} else if ((Input.GetKeyDown (KeyCode.Keypad2) || Input.GetKeyDown (KeyCode.Alpha2)) && actualQuestion.GetAnswer(2) != null) {
					timer = 0;
					canInput = false;

					if (actualQuestion.GetNumberOfOptions () >= 3) {
						if (actualQuestion.CanContinue (2)) {
							delayedHistoryActive = true;

						}
						else {
							DOVirtual.DelayedCall (waitTime * 3, EndGame);
						}
						mainText.CleanText ();
						mainText.SetNextString (actualQuestion.GetAnswer (2), null, () => {timer = 0; DOVirtual.DelayedCall(waitTime,()=>{ canContinueHistory = delayedHistoryActive;});});

						for (int i = 0; i < questionTexts.Length; i++) {
							questionTexts [i].CleanText ();
						}

					}
				} else if ((Input.GetKeyDown (KeyCode.Keypad3) || Input.GetKeyDown (KeyCode.Alpha3)) && actualQuestion.GetAnswer(3) != null) {
					timer = 0;
					canInput = false;

					if (actualQuestion.GetNumberOfOptions () >= 4) {
						if (actualQuestion.CanContinue (3)) {
							delayedHistoryActive = true;


						}
						else {
							DOVirtual.DelayedCall (waitTime * 3, EndGame);
						}
						mainText.CleanText ();
						mainText.SetNextString (actualQuestion.GetAnswer (3), null, () => {timer = 0; DOVirtual.DelayedCall(waitTime,()=>{ canContinueHistory = delayedHistoryActive;});});

						for (int i = 0; i < questionTexts.Length; i++) {
							questionTexts [i].CleanText ();
						}

					}
				}
			}


		}

	}


	/*void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawRay (Camera.main.ScreenPointToRay (Input.mousePosition));

	}
	*/
}
