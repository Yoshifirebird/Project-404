using UnityEngine;

public class EventUser : MonoBehaviour
{

	public InteractibleEvent eventTarget;

   

	public void action ()	{

		eventTarget.m_actionEvent.Invoke(this);
	}
}
