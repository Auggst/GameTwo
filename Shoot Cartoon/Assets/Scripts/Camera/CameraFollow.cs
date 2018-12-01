using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float smoothing = 5f;
    public bool _________________;
    Vector3 offset;

    private void Start()
    {
        //相机与玩家的初始偏移量
        offset = transform.position - target.position;
    }
    //用LateUpdate()来更新相机位置
    private void LateUpdate()
    {
        //相机目标位置
        Vector3 targetCamPos = target.position + offset;
        //移动相机Lerp()线性插值方法
        transform.position = Vector3.Lerp(transform.position,
                                          targetCamPos,
                                          smoothing*Time.deltaTime);
    }
}
