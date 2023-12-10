using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isCamAnim = false;
    public PageShift pageShift;
    public static GameManager instance;
    private int gameProcess;
    private void Awake()
    {
        instance = this;
        if (!isCamAnim) Camera.main.GetComponent<Animator>().enabled = false;
    }


    public void OnClick()
    {
        Debug.Log("gameProcess:"+gameProcess);
        gameProcess = gameProcess + 1;
        ChangeGameState();
    }

    private void ChangeGameState()
    {
        if (gameProcess == 3)
        {
          if (!isCamAnim)
          {
                //贝塞尔曲线移动
                Camera.main.GetComponent<CameraControl>().BezierMove();
                Camera.main.GetComponent<CameraControl>().PointSampleDivisor = 3;//加速
           }
          else
          {
                //动画移动
                Camera.main.GetComponent<CameraControl>().AnimatorMove();
            }
           pageShift.ShowPage1();
        }
        else if (gameProcess == 4)
        {
            pageShift.ShowPage2();
        }
        else
        {
            if (!isCamAnim)
                //贝塞尔曲线移动
                Camera.main.GetComponent<CameraControl>().BezierMove();
            else
            {
                //动画移动
                Camera.main.GetComponent<CameraControl>().AnimatorMove();
            }
        }
    }


}