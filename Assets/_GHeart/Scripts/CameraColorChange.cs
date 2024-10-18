using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraColorChange : MonoBehaviour {

    #region Fields

    [SerializeField] private List<Color> m_possibleColors;
    [SerializeField] private float m_switchColorCooldown = 5f;
    [SerializeField] private float m_colorInterpSpeed = 1f;
    
    private Camera m_cam;

    private Color m_aimColor;
    

    #endregion
    
    #region Unity Event Functinos

    private void Start(){
        m_cam =GetComponent<Camera>();
        m_aimColor = Color.black;
        StartCoroutine(SwitchColorCoroutine());
    }


    private void Update(){
        m_cam.backgroundColor = Color.Lerp(m_cam.backgroundColor, m_aimColor, Time.deltaTime * m_colorInterpSpeed);
    }

    private void OnDestroy(){
        StopAllCoroutines();
    }

    #endregion

    #region Helpers

    private IEnumerator SwitchColorCoroutine(){
        
        while (isActiveAndEnabled){
            yield return (new WaitForSeconds(m_switchColorCooldown));
            m_aimColor = GetColor();
        }
        
    }

    private Color GetColor(){
        if (m_possibleColors != null && m_possibleColors.Count > 0){
            int rndIndex = UnityEngine.Random.Range(0, m_possibleColors.Count);
            return(m_possibleColors[rndIndex]);
        }
        return (Color.black);
    }


    #endregion
    
}
