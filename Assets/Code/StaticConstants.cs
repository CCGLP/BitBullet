using UnityEngine;
using System.Collections;

public  class StaticConstants : MonoBehaviour {
    public static TextAsset jsonText; 
	public static int numberOfBaddies = 0;
	public static int badWordIndex = 0;
	public static ParticleSystem[] particlePool;
	public static Transform[] spawnPoints;
	public static int actualQuestionSpeed; 
	public static AudioSource spawnMusic;
	public static AudioSource explosionMusic;
}
