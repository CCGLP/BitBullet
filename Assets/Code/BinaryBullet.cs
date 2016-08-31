using UnityEngine;
using System.Collections;

public class BinaryBullet : MonoBehaviour {

	public enum ShootType{
		zero,
		one

	}

	[SerializeField]
	private float bulletSpeed = 1; 

	[SerializeField]
	private ShootType type;
	private Rigidbody rb;



	// Use this for initialization
	void Start () {
		StaticConstants.spawnMusic.Stop ();
		StaticConstants.spawnMusic.Play ();
		rb = this.GetComponent<Rigidbody> ();
		RaycastHit hit;
		Ray ray= Camera.main.ScreenPointToRay (Input.mousePosition);
		Physics.Raycast (this.transform.position, ray.direction, out hit);
		rb.velocity =  (hit.point - this.transform.position).normalized * bulletSpeed;
		this.transform.parent.LookAt (hit.point);

	}


	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "zeroText" && type == ShootType.zero) {
			StaticConstants.explosionMusic.Stop ();
			StaticConstants.explosionMusic.Play ();
			StaticConstants.numberOfBaddies--;
			StaticConstants.particlePool [0].transform.position = collision.contacts [0].point;
			StaticConstants.particlePool [0].Stop ();
			StaticConstants.particlePool [0].Play ();
			Destroy (collision.gameObject);


		} else if (collision.gameObject.tag == "oneText" && type == ShootType.one) {
			StaticConstants.explosionMusic.Stop ();
			StaticConstants.explosionMusic.Play ();
			StaticConstants.numberOfBaddies--;
			StaticConstants.particlePool [0].transform.position = collision.contacts [0].point;
			StaticConstants.particlePool [0].Stop ();
			StaticConstants.particlePool [0].Play ();
			Destroy (collision.gameObject);

		}
		Destroy (this.gameObject.transform.parent.gameObject);


	}

	// Update is called once per frame
	void Update () {
			




	}
}
