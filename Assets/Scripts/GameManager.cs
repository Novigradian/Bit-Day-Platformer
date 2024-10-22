using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private AudioSource music;
    
    // Start is called before the first frame update
    void Start()
    {
        music = gameObject.GetComponent<AudioSource>();
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void Success1()
    {
        SceneManager.LoadScene(2);
    }
    public void Success2()
    {
        SceneManager.LoadScene(3);
    }
    public void Success3()
    {
        SceneManager.LoadScene(4);
    }

    public void Success6()
    {
        SceneManager.LoadScene(5);
    }
}
