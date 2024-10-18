using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour{

    #region Fields
    [SerializeField] private Transform m_blinkMe;
    [SerializeField] private float m_blinkTime = 0.5f;
    #endregion

    
    private void Start(){
        StartCoroutine(BlinkCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.GetComponent<Damage>()){
            GameMode.I.EndPlay();
        }
    }


    #region Helpers

    private IEnumerator BlinkCoroutine(){
        while (isActiveAndEnabled){
            yield return (new WaitForSeconds(UnityEngine.Random.Range(2, 5)));
            m_blinkMe.DOScaleY(0.0f, m_blinkTime * 0.5f).OnComplete(() => {
                m_blinkMe.DOScaleY(0.35f, m_blinkTime * 0.5f);
            });
        }
    }

    #endregion
}
