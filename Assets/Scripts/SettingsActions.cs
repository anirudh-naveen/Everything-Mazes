using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Swap to main game screen
        if (Input.GetButtonDown("Cancel"))
            {
                SceneManager.LoadScene("CreationScene");
            }  
    }
}
