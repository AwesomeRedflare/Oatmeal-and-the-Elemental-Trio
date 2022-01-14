using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOne : MonoBehaviour
{
    public Text objectiveText;

    public int enemyCount;

    private void Update()
    {
        objectiveText.text = "Kill All Enemies:" + (20 - enemyCount) + "/20";
    }
}
