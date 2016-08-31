using UnityEngine;
using System.Collections;

public class Question  {

	private string header;
	private string[] options;
	private int goodOption;
	private int speed;
	private string goodAnswer;
	private string badAnswer;
	private string[] badWord;
	private int timeToPass;
	private bool canContinue;


	public Question (string header, string[] options, int goodOption, string goodAnswer, string badAnswer, bool canContinue, string[] badWord, int timeToPass, int speed){
		this.header = header;
		this.options = options;
		this.goodOption = goodOption;
		this.goodAnswer = goodAnswer;
		this.badAnswer = badAnswer;
		this.canContinue = canContinue;
		this.badWord = badWord;
		this.timeToPass = timeToPass;
		this.speed = speed;
	}

	public string[] GetBadWord(){
		return this.badWord;
	}

	public int GetSpeed(){
		return this.speed;
	}

	public string GetOption(int index){
		if (index < options.Length) {
			return index.ToString() + "- " + options [index];
		} else
			return null;

	}

	public string GetHeader(){
		return this.header;

	}

	public int GetTimeToPass(){
		return this.timeToPass;
	}

	public string GetAnswer(int input){
		if (input == goodOption) {
			return goodAnswer;
		} else if (input >= options.Length)
			return null;
		else
			return badAnswer;

	}

	public bool CanContinue(int input){
		if (input == goodOption) {
			return true;
		} else {
			return canContinue;
		}
	}

	public int GetNumberOfOptions(){
		return options.Length;
	}
}
