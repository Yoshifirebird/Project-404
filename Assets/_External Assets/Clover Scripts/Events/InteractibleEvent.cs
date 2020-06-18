/*
 * InteractibleEvent.cs
 * Created by: Newgame+ LD
 * Created on: ??/??/???? (dd/mm/yy)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleEvent : CommonBase {

  [Tooltip ("This script is for when there's a small event for a player to interact with using the action button")]
  public float radius;
  //public bool playerDetected;
  public InteractorKey actorTarget;
  //public PlayerActorExtension playerTarget;

  public Vector3 offset;
  [System.Serializable]
  public class enterEvent : UnityEvent<InteractorKey> { }
  public enterEvent m_enterEvent;
  [System.Serializable]
  public class stayEvent : UnityEvent<InteractorKey> { }
  public stayEvent m_stayEvent;
  [System.Serializable]
  public class exitEvent : UnityEvent<InteractorKey> { }
  public exitEvent m_exitEvent;
  [System.Serializable]
  public class actionEvent : UnityEvent<InteractorKey> { }
  public actionEvent m_actionEvent;

  public int priority;
  public bool playerOnly;
  public bool holdDebugDraw;
  public bool logDebugInfo;
  public bool noAction = false;

  //public CustomInputModule control;

  public new void Update () {

    base.Update();
  }

  public new void UnpauseUpdate () {

    //base.UnpauseUpdate ();

    //print ("UU Scan");
    scanforactor ();

  }

  //InteractorKey targetActor = null;

  void scanforactor () {

    //print ("scan");

    foreach (Collider checker in (Physics.OverlapSphere (transform.position + offset, radius))) {
      if (logDebugInfo)
        print (checker);
      //if (checker.gameObject.tag == "Player") {
      if (checker.GetComponent<InteractorKey> ()) {

        if (!playerOnly || (playerOnly && checker.tag == "Player")) {

          if (actorTarget == null) {

            actorTarget = checker.GetComponent<InteractorKey> ();

            if (actorTarget.eventTarget != null && actorTarget.eventTarget.priority > priority) {

              actorTarget = null;
              return;
            }

            if (!noAction) {
              actorTarget.eventTarget = this;
            }
            m_enterEvent.Invoke (actorTarget);

          }

          m_stayEvent.Invoke (actorTarget);
          if (logDebugInfo) print ("Stay Event");

          return;
        }
      }
    }

    if (actorTarget != null) {

      m_exitEvent.Invoke (actorTarget);

      actorTarget.eventTarget = null;

      actorTarget = null;
    }

  }

  void OnDrawGizmosSelected () {

    if (!holdDebugDraw) {
      Gizmos.color = Color.white;
      Gizmos.DrawWireSphere (transform.position + offset, radius);
    }
  }

  void OnDrawGizmos () {
    if (holdDebugDraw) {
      Gizmos.color = Color.white;
      Gizmos.DrawWireSphere (transform.position + offset, radius);
    }
  }

}
