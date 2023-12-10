using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Range(0, 20f)]
    public float speed;
    [Range(0, 20f)]
    public float rotateSpeed;

    [Range(0, 20f)]
    public float PointSampleDivisor = 1f;

    public List<Transform> Menu2Board;
    public List<Transform> Board2Bottle;
    public List<Transform> Bottle2Page;
    public List<List<Transform>> route = new List<List<Transform>>();

    private int MoveProcess = 0; //移动进度
    private Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        RouteConstruct();
        animator = GetComponent<Animator>();

    }

    #region -- 动画实现镜头移动 ---

    /// <summary>
    ///gameManager 控制摄像机移动+旋转
    /// </summary>
    public void AnimatorMove()
    {
        animator.SetInteger("step", MoveProcess);
        MoveProcess = MoveProcess + 1;
    }

    #endregion

    #region -- 贝塞尔曲线实现镜头移动 ---

    /// <summary>
    ///gameManager 控制摄像机移动+旋转
    /// </summary>
    public void BezierMove()
    {
        StartCoroutine(BezierMoveCamera(route[MoveProcess][0], route[MoveProcess][1], route[MoveProcess][2]));
        MoveProcess = MoveProcess + 1;
    }

    /// <summary>
    ///摄像机移动路径构建
    /// </summary>
    private void RouteConstruct()
    {
        route.Add(Menu2Board);
        route.Add(Board2Bottle);
        route.Add(Bottle2Page);
    }

    /// <summary>
    /// 摄像机移动
    /// </summary>
    /// <param name="start">起始位置</param>
    /// <param name="midPoint">中间点</param>
    /// <param name="target">目标点</param>
    IEnumerator BezierMoveCamera(Transform start, Transform midPoint, Transform target)
    {
        for (float i = 0; i<=1; i += Time.deltaTime * PointSampleDivisor + 0.001f)
        {
            //插值旋转
            Quaternion r1 = Quaternion.Slerp(start.rotation, midPoint.rotation, i);
            Quaternion r2 = Quaternion.Slerp(start.rotation, target.rotation, i);
            Quaternion r = Quaternion.Slerp(r1, r2, i);
            //Bezier曲线移动
            Vector3 p1 = Vector3.Lerp(start.position, midPoint.position, i);
            Vector3 p2 = Vector3.Lerp(midPoint.position, target.position, i);
            Vector3 p = Vector3.Lerp(p1, p2, i);
            yield return StartCoroutine(MoveToPoint(p, r));
        }
        yield return StartCoroutine(MoveToObj(target));
    }

    /// <summary>
    /// 移动到插值点
    /// </summary>
    IEnumerator MoveToPoint(Vector3 p, Quaternion r)
    {
        while ((Vector3.Distance(transform.position, p) >0.1f) ||
           ((r*Quaternion.Inverse(transform.rotation)).eulerAngles.x > 1f) || ((r * Quaternion.Inverse(transform.rotation)).eulerAngles.y > 0.1f) || ((r * Quaternion.Inverse(transform.rotation)).eulerAngles.z > 0.1f))
        { 
            if (Vector3.Distance(transform.position, p) > 0.1f)
                transform.position = Vector3.MoveTowards(transform.position, p, Time.deltaTime * speed);
            if (((r * Quaternion.Inverse(transform.rotation)).eulerAngles.x > 1f) || ((r * Quaternion.Inverse(transform.rotation)).eulerAngles.y > 0.1f) || ((r * Quaternion.Inverse(transform.rotation)).eulerAngles.z > 0.1f))
                transform.rotation = Quaternion.RotateTowards(transform.rotation, r, Time.deltaTime * rotateSpeed);
        }
        yield return null;
    }

    /// <summary>
    /// 末端导航到准确位置
    /// </summary>
    IEnumerator MoveToObj(Transform target)
    {
       Vector3 p = target.position;
        Quaternion r = target.rotation;
        while ((Vector3.Distance(transform.position, p) > 0.1f) || ((r * Quaternion.Inverse(transform.rotation)).eulerAngles.x > 0.1f) || ((r * Quaternion.Inverse(transform.rotation)).eulerAngles.y > 0.1f) || ((r * Quaternion.Inverse(transform.rotation)).eulerAngles.z > 0.1f))
        {
            if (Vector3.Distance(transform.position, p) > 0.1f)
                transform.position = Vector3.MoveTowards(transform.position, p, Time.deltaTime * speed);
            if (((r * Quaternion.Inverse(transform.rotation)).eulerAngles.x > 0.1f) || ((r * Quaternion.Inverse(transform.rotation)).eulerAngles.y > 0.1f) || ((r * Quaternion.Inverse(transform.rotation)).eulerAngles.z > 0.1f))
                transform.rotation = Quaternion.RotateTowards(transform.rotation, r, Time.deltaTime * rotateSpeed);
        }
        yield return null;
    }

    #endregion


}
