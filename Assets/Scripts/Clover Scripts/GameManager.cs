using UnityEngine;

public class GameManager : CommonBase {

	public static GameManager instance;

	public string lang = "ENG";


	public bool pause;
	public bool playerControlLocked;






	void Awake ()	{

		if (GameManager.instance != null)
			Destroy (gameObject);

		GameManager.instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

}
