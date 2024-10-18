using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameMode : MonoBehaviour{

    private const string BEST_SCORE_SAVE_KEY = "BEST SCORE";

    #region Fields
    public static GameMode I => m_instance;
    private static GameMode m_instance;

    [SerializeField] private Transform m_playerStartPos;
    [SerializeField] private GameObject m_playerPrefab;

    [SerializeField] private Transform m_leftBottom;
    [SerializeField] private Transform m_upperRight;

    [SerializeField] private MoveLine m_moveLinePrefab;

    [SerializeField] private float m_spawnEnemyInitialDelay = 5f;
    [SerializeField] private float m_enemMoveInitialDuration = 5f;
    [SerializeField] private float m_diffCurveCoef = 1f;
    
    [SerializeField] private float m_spawnEnemyDelay = 4f;
    [SerializeField] private float m_enemyMoveDuration = 5f;

    [SerializeField] private float m_elapsedTime = 0;
    [SerializeField] private TMP_Text m_elapsedTimeTest;
    
    [SerializeField] private TMP_Text m_currentScore;
    [SerializeField] private TMP_Text m_bestScore;
    
    private GameObject m_player;

    private bool m_running;
    
    #endregion

    
    #region Unity Event Functions
    private void Awake(){
        m_instance = this;
    }


    private void Start(){
        m_bestScore.text = $"BEST SCORE: {Mathf.RoundToInt(PlayerPrefs.GetFloat(BEST_SCORE_SAVE_KEY))}";
        m_currentScore.gameObject.SetActive(false);
        m_elapsedTimeTest.gameObject.SetActive(false);
    }

    private void Update(){
        m_elapsedTime += Time.deltaTime;
        m_elapsedTimeTest.text = $"{Mathf.RoundToInt(m_elapsedTime)}";
    }

    private void OnDestroy(){
        
    }

    #endregion


    #region Public

    public void BeginPlay(){
        m_currentScore.gameObject.SetActive(false);
        m_bestScore.gameObject.SetActive(false);
        
        m_elapsedTime = 0;
        m_player = GameObject.Instantiate(m_playerPrefab, m_playerStartPos.position, Quaternion.identity);
        m_running = true;
        StartCoroutine(SpawnMoveLinesCoroutine());
        
        m_elapsedTimeTest.gameObject.SetActive(true);

    }

    public void EndPlay(){
        m_elapsedTimeTest.gameObject.SetActive(false);

        float bestScore = PlayerPrefs.GetFloat(BEST_SCORE_SAVE_KEY);

        if (bestScore < m_elapsedTime){
            bestScore = m_elapsedTime;
            PlayerPrefs.SetFloat(BEST_SCORE_SAVE_KEY, m_elapsedTime);
        }

        m_currentScore.text = $"CURRENT SCORE: {Mathf.RoundToInt(m_elapsedTime)}";
        m_bestScore.text = $"BEST SCORE: {Mathf.RoundToInt(bestScore)}";

        m_currentScore.gameObject.SetActive(true);
        m_bestScore.gameObject.SetActive(true);
        
        StopAllCoroutines();
        m_running = false;
        Destroy(m_player);
        FindAnyObjectByType<UIPlayButton>(FindObjectsInactive.Include).gameObject.SetActive(true);
    }
    #endregion

    #region Helpers

    private IEnumerator SpawnMoveLinesCoroutine(){
        while (m_running){
            if (UnityEngine.Random.Range(0, 100) > 50){
                SpawnLineTroughThePlayer();
            }
            yield return (new WaitForSeconds(m_spawnEnemyDelay));
            SpawnMoveLine();
            UpdateGameTimeVariables();
        }
    }

    private Tuple<Vector2, Vector2> GetMoveLinePoints(){

        float leftY = UnityEngine.Random.Range(m_leftBottom.position.y, m_upperRight.position.y);
        Vector2 leftStart = new Vector2(m_leftBottom.position.x, leftY);
        float rightY = UnityEngine.Random.Range(m_leftBottom.position.y, m_upperRight.position.y);
        Vector2 rightEnd = new Vector2(m_upperRight.position.x, rightY);
        
        return (new Tuple<Vector2, Vector2>(leftStart, rightEnd));
    }

    private void SpawnMoveLine(){
        MoveLine mLine = Instantiate(m_moveLinePrefab);
        if (mLine){
            Tuple<Vector2, Vector2> movePoints = GetMoveLinePoints();
            mLine.BeginPlay(movePoints.Item1, movePoints.Item2, m_enemyMoveDuration);
        }
    }

    private void UpdateGameTimeVariables(){
        float difficultyCoef = Mathf.Pow(0.95f, Mathf.RoundToInt(m_elapsedTime / m_diffCurveCoef));
        m_spawnEnemyDelay = m_spawnEnemyInitialDelay * difficultyCoef;
        m_enemyMoveDuration = m_enemMoveInitialDuration * difficultyCoef;
    }
    
    private void SpawnLineTroughThePlayer(){

        float rightY = UnityEngine.Random.Range(m_leftBottom.position.y, m_upperRight.position.y);
        Vector2 rightStart = new Vector2(m_upperRight.position.x, rightY);

        Vector2 playerPos = m_player.transform.position;
        Vector2 direction = (playerPos - rightStart).normalized;

        Vector2 end = rightStart + direction * 15;
        
        MoveLine mLine = Instantiate(m_moveLinePrefab);
        if (mLine){
            Tuple<Vector2, Vector2> movePoints = GetMoveLinePoints();
            mLine.BeginPlay(rightStart, end, m_enemyMoveDuration);
        }
    }

    #endregion
    
    
    
}
