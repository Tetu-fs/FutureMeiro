using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReloadScene : MonoBehaviour {

	// Use this for initialization

	
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
