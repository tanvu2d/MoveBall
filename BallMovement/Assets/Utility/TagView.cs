using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagView : MonoBehaviour
{
    [SerializeField] GameObject[] Views;

    int currView = 0;

    public void ShowTag(int index)
    {
        Views[currView].SetActive(false);

        Views[index].SetActive(true);

        currView = index;
    }

    
}
