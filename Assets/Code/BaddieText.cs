using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BaddieText : MonoBehaviour {
	private bool started = false;
	private BoxCollider boxColl;
	private Rigidbody rb;
	private TextMesh text;
	private Animator anim;

	void Start(){
		if (!started) {
			started = true; 
			StaticConstants.numberOfBaddies++;
			this.gameObject.tag = Random.Range (0, 100) < 50 ? "zeroText" : "oneText";
			boxColl = this.GetComponent<BoxCollider> ();
			rb = this.GetComponent<Rigidbody> ();
			text = this.GetComponent<TextMesh> ();
			anim = this.GetComponent<Animator> ();
		}
	}

	public void Initialize(string sentence, Vector3 velocity) {
		Start ();

		this.text.text = sentence;
		this.rb.velocity = velocity;

		//We're going to search all the white spaces between the first char and the first real char to adapt the box collider
		// with totally magic numbers.


		boxColl.size = Vector3.right * 9 * (sentence.Length)  + Vector3.up * 15 + Vector3.forward * 6;
		boxColl.center = (Vector3.right * boxColl.size.x/2) + Vector3.down * 9 - Vector3.forward * 9;


	}
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "bullet") {
			anim.SetTrigger ("buggy");
		}
	}
		
	// Update is called once per frame
	void Update () {
	
	}
}
