using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PikminCounter : MonoBehaviour {

  public Text squadCounterText;
  public int squadCounter;

  public Text fieldCounterText;
  public int fieldCounter;

  public Text allCounterText;
  public int allCounter;

  [System.Serializable]
  public class pikminRosters {

    public bool gotten;
    public GameObject target;
    public Text squad;
    public int squadCounter;
    public Text field;
    public int fieldCounter;
    public Text reserve;
    public int allCounter;
  }

  public pikminRosters[] m_pikminRosters;

  // Start is called before the first frame update
  void Start () {

  }

  // Update is called once per frame
  void Update () {

    if (squadCounterText.text != squadCounter.ToString ()) {
      squadCounterText.text = squadCounter.ToString ();
      squadCounterText.GetComponent<Animation> ().Play ();

    }

    if (fieldCounterText.text != fieldCounter.ToString ()) {
      fieldCounterText.text = fieldCounter.ToString ();
      fieldCounterText.GetComponent<Animation> ().Play ();

    }

    if (allCounterText.text != allCounter.ToString ()) {
      allCounterText.text = allCounter.ToString ();
      allCounterText.GetComponent<Animation> ().Play ();

    }

    foreach (pikminRosters entry in m_pikminRosters) {

      if (entry.gotten) {

        if (entry.squad.text != entry.squadCounter.ToString ()) {
          entry.squad.text = entry.squadCounter.ToString ();
          entry.squad.GetComponent<Animation> ().Play ();

        }

        if (entry.field.text != entry.fieldCounter.ToString ()) {
          entry.field.text = entry.fieldCounter.ToString ();
          entry.field.GetComponent<Animation> ().Play ();
        }

        if (entry.reserve.text != entry.allCounter.ToString ()) {
          entry.reserve.text = entry.allCounter.ToString ();
          entry.reserve.GetComponent<Animation> ().Play ();
        }
      }

    }
  }
}
