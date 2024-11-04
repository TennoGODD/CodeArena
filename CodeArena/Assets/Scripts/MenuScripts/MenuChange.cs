using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuChange : MonoBehaviour
{
    [SerializeField] private GameObject _lvlChanger;
    public void SetUnactive()
    {
        _lvlChanger.SetActive(false);
    }

    public void SetActive()
    {
        _lvlChanger.SetActive(true);
    }

}
