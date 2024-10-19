using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject playerHealthUI;

    [SerializeField]
    private GameObject time;
    private TMP_Text timeText;
    private float timeCount;


    void Start()
    {
        timeCount = 0f;
        timeText = time.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        timeText.text = "Time: " + (Mathf.Round(timeCount * 100f) / 100f).ToString() + "s";
    }

    public void setHealth(int health)
    {
        int i = 0;
        foreach (Transform heart in playerHealthUI.transform)
        {
            if (i < health)
            {
                heart.gameObject.SetActive(true);
            }
            else
            {
                heart.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
