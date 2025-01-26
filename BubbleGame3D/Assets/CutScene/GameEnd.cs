using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    // Start is called before the first frame update
    public void MoveScene()
    {

        SceneUtil.GoLobbyScene();
    }
}
