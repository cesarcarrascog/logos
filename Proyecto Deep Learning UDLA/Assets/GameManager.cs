using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public StandObject[] stands;
    [SerializeField]
    private int frameRate;
    void Start()
    {
        instance = this;
        Application.targetFrameRate = frameRate;
    }
}
