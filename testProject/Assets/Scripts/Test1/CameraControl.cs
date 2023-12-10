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

    private int MoveProcess = 0; //�ƶ�����
    private Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        RouteConstruct();
        animator = GetComponent<Animator>();

    }

    #region -- ����ʵ�־�ͷ�ƶ� ---

    /// <summary>
    ///gameManager ����������ƶ�+��ת
    /// </summary>
    public void AnimatorMove()
    {
        animator.SetInteger("step", MoveProcess);
        MoveProcess = MoveProcess + 1;
    }

    #endregion

    #region -- ����������ʵ�־�ͷ�ƶ� ---

    /// <summary>
    ///gameManager ����������ƶ�+��ת
    /// </summary>
    public void BezierMove()
    {
        StartCoroutine(BezierMoveCamera(route[MoveProcess][0], route[MoveProcess][1], route[MoveProcess][2]));
        MoveProcess = MoveProcess + 1;
    }

    /// <summary>
    ///������ƶ�·������
    /// </summary>
    private void RouteConstruct()
    {
        route.Add(Menu2Board);
        route.Add(Board2Bottle);
        route.Add(Bottle2Page);
    }

    /// <summary>
    /// ������ƶ�
    /// </summary>
    /// <param name="start">��ʼλ��</param>
    /// <param name="midPoint">�м��</param>
    /// <param name="target">Ŀ���</param>
    IEnumerator BezierMoveCamera(Transform start, Transform midPoint, Transform target)
    {
        for (float i = 0; i<=1; i += Time.deltaTime * PointSampleDivisor + 0.001f)
        {
            //��ֵ��ת
            Quaternion r1 = Quaternion.Slerp(start.rotation, midPoint.rotation, i);
            Quaternion r2 = Quaternion.Slerp(start.rotation, target.rotation, i);
            Quaternion r = Quaternion.Slerp(r1, r2, i);
            //Bezier�����ƶ�
            Vector3 p1 = Vector3.Lerp(start.position, midPoint.position, i);
            Vector3 p2 = Vector3.Lerp(midPoint.position, target.position, i);
            Vector3 p = Vector3.Lerp(p1, p2, i);
            yield return StartCoroutine(MoveToPoint(p, r));
        }
        yield return StartCoroutine(MoveToObj(target));
    }

    /// <summary>
    /// �ƶ�����ֵ��
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
    /// ĩ�˵�����׼ȷλ��
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
