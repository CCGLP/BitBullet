using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	private float timer = 0;
	[SerializeField]
	private float timeToShoot = 1f;

	[SerializeField]
	private GameObject zeroShoot;
	[SerializeField]
	private GameObject oneShoot;
	[SerializeField]
	private Transform originShootPoint;





	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timer += Time.fixedDeltaTime;
		if (timer > timeToShoot && Input.GetMouseButtonDown (0)) {
			Instantiate (zeroShoot, this.originShootPoint.position, zeroShoot.transform.rotation);
			timer = 0;
		} else if (timer > timeToShoot && Input.GetMouseButtonDown (1)) {
			Instantiate (oneShoot, this.originShootPoint.position, oneShoot.transform.rotation);
			timer = 0;
		}

	}
}
