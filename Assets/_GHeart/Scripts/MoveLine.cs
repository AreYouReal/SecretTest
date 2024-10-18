using DG.Tweening;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class MoveLine : MonoBehaviour {
    
    #region Fields
    [SerializeField] private LineRenderer m_lineRenderer;
    [SerializeField] private GameObject m_enemyPrefab;

    private GameObject m_enemy;
    #endregion
    
    
    
    #region Public
    public void BeginPlay(Vector2 a_startPoint, Vector2 a_endPoint, float a_moveDuration){
        m_lineRenderer.SetPosition(0, a_startPoint);
        m_lineRenderer.SetPosition(1, a_endPoint);
        Vector2 moveDirection = (a_endPoint - a_startPoint).normalized;
        m_enemy = Instantiate(m_enemyPrefab, a_startPoint, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, moveDirection)));
        if (m_enemy){
            m_enemy.transform.DOMove(a_endPoint, a_moveDuration).SetDelay(1.0f).OnComplete(EndPlay);
        }
    }

    public void EndPlay(){
        Destroy(m_enemy);
        Destroy(gameObject);
    }
    #endregion


}
