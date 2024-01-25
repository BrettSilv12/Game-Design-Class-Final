using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyManager : MonoBehaviour
{

    public Transform[] m_SpawnPoints;
    public GameObject enemyai;

    // Start is called before the first frame update
    void Start()
    {
    }


    public void SpawnNewEnemy() {
        Instantiate(enemyai, m_SpawnPoints[Random.Range(0, m_SpawnPoints.Length-1)].transform.position, Quaternion.identity);
    }

}
