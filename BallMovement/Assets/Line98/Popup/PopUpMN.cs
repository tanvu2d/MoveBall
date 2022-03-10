using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpMN : MonoBehaviour
{
   public void btnTryAgain ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
