using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikminCounterDebug : MonoBehaviour
{

	public PikminCounter counter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


			counter.allCounter = getReservesCount();

			foreach(PikminCounter.pikminRosters target in counter.m_pikminRosters)	{

				int i = 0;

				if(target.gotten)	{

					target.allCounter = getReservesSingleIndex(i);

				}

				i++;

			}


    }

	int getReservesCount ()	{

		int countUp = 0;

		foreach(Vector3Int target in GameManager.pikminReserves)	{

			countUp += target.x;
			countUp += target.y;
			countUp += target.z;

		}

		return countUp;

	}

	int getReservesSingleIndex (int pointer)	{

		int countUp = 0;


			countUp += GameManager.pikminReserves[pointer].x;
			countUp += GameManager.pikminReserves[pointer].y;
			countUp += GameManager.pikminReserves[pointer].z;


		return countUp;


	}


}
