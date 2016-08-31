using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class LanguageSelection : MonoBehaviour {
    public TextAsset spanishJson;
    public TextAsset englishJson;
	
    public void OnSpanishClick()
    {
        StaticConstants.jsonText = spanishJson;
        SceneManager.LoadScene(1);

    }
    public void OnEnglishClick()
    {
        StaticConstants.jsonText = englishJson;
        SceneManager.LoadScene(1);
    }
}
