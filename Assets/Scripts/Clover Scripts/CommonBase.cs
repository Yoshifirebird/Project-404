using UnityEngine;

public class CommonBase : MonoBehaviour {

	public AudioSource m_audioSource;
	public AudioSource externalSource;
	public Animator m_animator;

	public void Start () {

		cbStart ();
	}

	public void cbStart ()	{

		//Get our components on this object
		if(m_audioSource == null) {m_audioSource = GetComponent<AudioSource> (); //print("Got the Audio");
		}

		if(m_animator == null) {m_animator = GetComponent<Animator> (); //print("Got the Animator");
		}
	}

	public void Update ()	{

		if (GameManager.instance.pause == false)
			UnpauseUpdate ();

	}


	public void UnpauseUpdate ()	{

		//print ("Unpause update from " + gameObject.name + ". Change me!");
	}




	public void playSound (AudioClip sound) 	{	//Simple. Just play a sound.

		m_audioSource.clip = sound;
		m_audioSource.Play ();

	}

	public void playSoundOneshot (AudioClip sound) 	{ 	//Play a one shot sound

		m_audioSource.PlayOneShot (sound);

	}

	public void playSoundExternal (AudioClip sound) 	{	//Create an external source to play the sound

		AudioSource external = Instantiate (externalSource);
		external.clip = sound;
		external.Play ();
	}

	public void delete (float delay)	{ 	//Simple object destruction. Is this even useful?

		Destroy (gameObject, delay);


	}

	public void setInactive ()	{

		gameObject.SetActive (false);

	}

	public void setToActive ()	{

		gameObject.SetActive (true);

	}

	public void createObject (Transform effect)	{

		Instantiate (effect,transform.position,transform.rotation);
	}
}
