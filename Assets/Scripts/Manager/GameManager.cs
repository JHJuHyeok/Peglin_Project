using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 1. 보유 유물 목록
    /// 2. 보유 구슬 목록
    /// 3. HP 수치
    /// </summary>
    [Header("전투 수치")]
    [SerializeField] private PlayerManager player;
    public int normalAtk;
    public int critAtk;
    public int bombAtk;
    public int getCoin;
    private List<OrbData> orbList;

    [Header("조작 턴")]
    private GameObject currentBall;
    private Rigidbody2D orbRb;                  // 오브 리지드바디
    private Transform respawnPos;               // 오브 배치되는 위치
    public TrajectoryPredictor predictor;       // 선 그리기
    public Transform shootPoint;                // 오브 발사 위치
    public float power = 5f;                    // 발사 위력

    private Vector2 aimDir;                     // 마우스 위치

    [Header("마우스 커서")]
    [SerializeField] private Texture2D cursorIdle;
    [SerializeField] private Texture2D cursorSelect;
    [SerializeField] private Texture2D cursorAim;
    [SerializeField] private Texture2D cursorCant;

    // 턴 확인
    public bool isMyTurn = true;
    private bool isProcessingTurn = false;

    // 싱글톤 디자인
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 보장
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if(respawnPos == null)
            respawnPos = GameObject.FindWithTag("RespawnPos").transform;
        GetNewOrbs();


    }

    private void Update()
    {
        // 내 턴일 때
        if(isMyTurn)
        {
            Cursor.SetCursor(cursorAim, Vector2.zero, CursorMode.Auto);

            if (orbList == null)
                GetNewOrbs();
            else
                currentBall = orbList[0].prefab;

            // 지니고 있는 오브 리스트 0번 SO의 프리펩을 배치
            CreateBall();

            // 마우스 기준으로 방향 계산
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            aimDir = (mouseWorld - shootPoint.position).normalized;

            // 궤적 표시
            predictor.ShowTrajectory(aimDir);

            // 발사
            if (Input.GetMouseButtonDown(0))
            {
                isMyTurn = false;
                Fire();
            }
        }
        else   // 적 턴일 때
        {
            Cursor.SetCursor(cursorCant, Vector2.zero, CursorMode.Auto);


        }
    }

    void Fire()
    {
        orbRb.gravityScale = predictor.gravityScale;
        orbRb.AddForce(aimDir * power, ForceMode2D.Impulse);
        predictor.HideTrajectory();
    }

    void CreateBall()
    {
        if(orbRb == null)
            orbRb = Instantiate(currentBall.GetComponent<Rigidbody2D>(), 
                shootPoint.position, Quaternion.identity);
    }

    public void ClearBall()
    {
        orbRb = null;
    }

    private void GetNewOrbs()
    {
        orbList = player.myOrbList.ToList();
        ShuffleList(orbList);
    }

    private List<T> ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }

    public virtual void GetOrbData(OrbData data)
    {
        if (data.OrbLevel == 1)
        {
            normalAtk = data.atk_One;
            critAtk = data.crit_One;
        }
        else if (data.OrbLevel == 2)
        {
            normalAtk = data.atk_Two;
            critAtk = data.crit_Two;
        }
        else if (data.OrbLevel == 3)
        {
            normalAtk = data.atk_Three;
            critAtk = data.crit_Three;
        }
    }
}
