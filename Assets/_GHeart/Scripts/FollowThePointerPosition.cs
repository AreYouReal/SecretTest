using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowThePointerPosition : MonoBehaviour {


    #region Fields
    [SerializeField] private float m_speed = 1;
    
    private Vector3 m_aimPos;
    #endregion
    
    #region Unity Event Functions

    private void Start(){
        m_aimPos = transform.position;
    }

    private void Update(){
        if (Input.GetMouseButton(0)){
            m_aimPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_aimPos.z = 0;
        }
        transform.position = Vector3.Lerp(transform.position, m_aimPos, Time.deltaTime * m_speed);
    }
    #endregion
    
}
