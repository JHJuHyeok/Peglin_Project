using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
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

    [Header("애니메이션 관리")]
    [SerializeField] private GameObject peglin;
    private Animator peglinAnim;
    private static readonly int MoveHash = Animator.StringToHash("IsMoving");
    private static readonly int AttackHash = Animator.StringToHash("IsAttack");
    private static readonly int BombHash = Animator.StringToHash("IsBomb");
    private static readonly int DeadHash = Animator.StringToHash("IsDead");

    [Header("UI 제어")]
    [SerializeField] Text ballName;
    [SerializeField] Text atkText;
    [SerializeField] Text critText;
    [SerializeField] Text description;
    [SerializeField] Text HPText;
    [SerializeField] Text GarbageCount;
    public Text CoinText;

    [Header("오브 패널")]
    [SerializeField] GameObject orbsPanel;
    [SerializeField] Image orbRing;
    [SerializeField] Image orbImage;
    [SerializeField] Sprite OrbRing_One;
    [SerializeField] Sprite OrbRing_Two;
    [SerializeField] Sprite OrbRing_Three;

    [SerializeField] Sprite orbBG_One;
    [SerializeField] Sprite orbBG_Two;
    [SerializeField] Sprite orbBG_Three;

    [Header("조작 턴")]
    private GameObject currentBall;
    private Rigidbody2D orbRb;                  // 오브 리지드바디
    private Transform respawnPos;               // 오브 배치되는 위치
    public TrajectoryPredictor predictor;       // 선 그리기
    public Transform shootPoint;                // 오브 발사 위치
    public float power = 5f;                    // 발사 위력

    private int garbageCount;
    private int garbageMax = 1;

    [Header("정산 턴")]
    public bool isCritical = false;
    public int damageCount;
    public int bombCount;

    [Header("몬스터 관련")]
    [SerializeField] private Transform[] waypoints;
    private MonsterBase[] monsters;

    private Vector2 aimDir;                     // 마우스 위치

    [Header("마우스 커서")]
    [SerializeField] private Vector2 cursorSpot;
    [SerializeField] private Texture2D cursorIdle;
    [SerializeField] private Texture2D cursorSelect;
    [SerializeField] private Texture2D cursorAim;
    [SerializeField] private Texture2D cursorCant;

    [Header("랜덤 생성 맵 종류")]
    [SerializeField] private GameObject pegAlign_One;
    [SerializeField] private GameObject pegAlign_Two;
    [SerializeField] private GameObject pegAlign_Three;
    [SerializeField] private GameObject peg_Dull;
    [SerializeField] private GameObject peg_Coin;
    [SerializeField] private GameObject peg_Crit;
    [SerializeField] private GameObject peg_Ref;
    [SerializeField] private GameObject peg_Bomb;
    public GameObject pegAlign;

    // 턴 확인
    public bool isMyTurn = true;
    public bool isFire = false;
    public bool isOrbFall = false;
    private bool isProcessingTurn = false;


    private void Awake()
    {
        peglinAnim = peglin.GetComponent<Animator>();
    }

    private void Start()
    {
        if(respawnPos == null)
            respawnPos = GameObject.FindWithTag("RespawnPos").transform;

        GetNewOrbs();

        cursorSpot = new Vector2(-3f, 0f);
        CoinText.text = $"{player.Coin}";
        HPText.text = $"{player.currentHP}/{player.MaxHP}";
        GarbageCount.text = $"{garbageCount}/{garbageMax}";

        monsters = FindObjectsOfType<MonsterBase>();

        CreateAlign(2, 2, 8);
    }

    private void Update()
    {
        if (!isProcessingTurn)
        {
            if (isMyTurn)   // 내 턴
            {
                StartCoroutine(MyTurn());

                if (!isFire)
                {
                    // 마우스 기준으로 방향 계산
                    Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorld.z = 0f;
                    aimDir = (mouseWorld - shootPoint.position).normalized;

                    // 궤적 표시
                    predictor.ShowTrajectory(aimDir);
                }
                if (Input.GetMouseButtonUp(0) && !isFire)
                {
                    isMyTurn = false;
                    //isProcessingTurn = false;
                    Fire();
                }
            }
            else if (!isMyTurn)   // 적 턴
            {
                StartCoroutine(OrbPanelUpdate());
                //StartCoroutine(MyAttack(monsters[0].gameObject, 0.1f));
                //MoveToPeglin(peglin, waypoints, 0.2f);

            }
        }
    }

    private IEnumerator MyTurn()
    {
        isProcessingTurn = true;

        Cursor.SetCursor(cursorAim, cursorSpot, CursorMode.Auto);

        if (orbList == null)
            GetNewOrbs();
        else
            currentBall = orbList[0].prefab;

        UpdateOrbUI(orbList[0]);

        // 지니고 있는 오브 리스트 0번 SO의 프리펩을 배치
        CreateBall(orbList[0]);

        isProcessingTurn = false;
        yield return null;
    }
    // 오브 패널 UI 업데이트
    private IEnumerator OrbPanelUpdate()
    {
        //yield return null;
        isProcessingTurn = true;

        Cursor.SetCursor(cursorCant, cursorSpot, CursorMode.Auto);

        if (isOrbFall)
        {
            orbList.Remove(orbList[0]);
            RemoveOrbPanelChild();
            UpdateOrbUI(orbList[0]);
            isOrbFall = false;
        }
        isMyTurn = true;
        isProcessingTurn = false;
        Debug.Log("오브 업데이트");
        yield return null;
    }
    // 데미지 정산 애니메이션
    private IEnumerator MyAttack(GameObject target, float duration)
    {
        //yield return StartCoroutine(OrbPanelUpdate());

        if (bombCount != 0)
        {
            // 폭탄 던지기
            peglinAnim.SetBool(BombHash, true);
            yield return new WaitForSeconds(1.0f);
            // 여기서 날아가는 폭탄 표시
            peglinAnim.SetBool(BombHash, false);
            bombCount = 0;
        }
        // 여기서 공격
        peglinAnim.SetBool(AttackHash, true);
        GameObject throwOrb = Instantiate(currentBall, peglin.transform, false);

        throwOrb.transform.position = Vector2.MoveTowards(throwOrb.transform.position, target.transform.position, duration);
        // 0.1초 후에 삭제
        yield return new WaitForSeconds(duration);
        Destroy(throwOrb);
        // 몬스터 체력 감소
        if (target.TryGetComponent<MonsterBase>(out var monster))
        {
            monster.Damaged(damageCount);
            damageCount = 0;
        }

        peglinAnim.SetBool(AttackHash, false);
        isProcessingTurn = false;
        Debug.Log("공격 진행");
        yield return new WaitForSeconds(0.2f);
        isMyTurn = true;
    }


    private IEnumerator MoveToPeglin(GameObject peglin, Transform[] waypoint, float seconds)
    {
        //yield return StartCoroutine(MyAttack(monsters[0].gameObject, 0.1f));

        if (peglin.TryGetComponent<MonsterBase>(out var data))
        {
            //waypoint[data.currentPoint].position;
            peglin.transform.position = Vector2.MoveTowards(
                    peglin.transform.position, 
                    waypoint[data.currentPoint].position, seconds);
            data.currentPoint++;

            yield return new WaitForSeconds(seconds + 0.1f);
            isProcessingTurn = false;
            isMyTurn = true;
            yield return null;
        }
    }

    // 오브 발사
    private void Fire()
    {
        orbRb.gravityScale = predictor.gravityScale;
        orbRb.AddForce(aimDir * power, ForceMode2D.Impulse);
        //StopCoroutine(MyTurn());
        isFire = true;
        predictor.HideTrajectory();
    }

    // 랜덤 배치 생성
    public void CreateAlign(int critPeg, int refPeg, int bombPeg)
    {
        // 랜덤으로 페그 배치 생성
        pegAlign = new GameObject();
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                pegAlign = Instantiate(pegAlign_One, pegAlign_One.transform.position, Quaternion.identity);
                break;
            case 1:
                pegAlign = Instantiate(pegAlign_Two, pegAlign_Two.transform.position, Quaternion.identity);
                break;
            case 2:
                pegAlign = Instantiate(pegAlign_Three, pegAlign_Three.transform.position, Quaternion.identity);
                break;
        }

        CreatePegs(pegAlign, critPeg, refPeg, bombPeg);
    }

    public void CreatePegs(GameObject align, int critPeg, int refPeg, int bombPeg)
    {
        if (align.transform.childCount == 0) return;

        List<int> executeNum = new List<int>();     // 전체 제외 인덱스
        List<int> critPegNum = new List<int>();     // 크리티컬 페그 인덱스
        List<int> refPegNum = new List<int>();      // 새로고침 페그 인덱스
        List<int> bombPegNum = new List<int>();     // 폭탄 페그 인덱스

        for (int i = 0; i < critPeg; i++)
        {
            int r = Random.Range(0, align.transform.childCount);

            while (executeNum.Contains(r))
            {
                r = Random.Range(0, align.transform.childCount);
            }
            executeNum.Add(r);
            critPegNum.Add(r);
        }
        for (int i =0; i< refPeg; i++)
        {
            int r = Random.Range(0, align.transform.childCount);

            while (executeNum.Contains(r))
            {
                r = Random.Range(0, align.transform.childCount);
            }
            executeNum.Add(r);
            refPegNum.Add(r);
        }
        for (int i = 0; i < bombPeg; i++)
        {
            int r = Random.Range(0, align.transform.childCount);

            while (executeNum.Contains(r))
            {
                r = Random.Range(0, align.transform.childCount);
            }
            executeNum.Add(r);
            bombPegNum.Add(r);
        }

        for (int i = 0; i < align.transform.childCount; i++)
        {
            if (critPegNum.Contains(i))
            {
                GameObject aPeg = Instantiate(peg_Crit);
                aPeg.transform.SetParent(align.transform.GetChild(i), false);
            }
            else if(refPegNum.Contains(i))
            {
                GameObject aPeg = Instantiate(peg_Ref);
                aPeg.transform.SetParent(align.transform.GetChild(i), false);
            }
            else if (bombPegNum.Contains(i))
            {
                GameObject aPeg = Instantiate(peg_Bomb);
                aPeg.transform.SetParent(align.transform.GetChild(i), false);
            }
            else
            {
                GameObject aPeg = Instantiate(peg_Coin);
                aPeg.transform.SetParent(align.transform.GetChild(i), false);
            }
        }
    }

    public void RemovePegs(GameObject align)
    {
        foreach(Transform child in align.transform)
        {
            foreach(Transform peg in child)
                Destroy(peg.gameObject);
        }
    }

    private void UpdateOrbUI(OrbData orb)
    {
        ChangeOrbStatUI(orb);
        ChangeOrbsPanelUI(orb);
    }

    // 오브 스탯의 텍스트 변경
    private void ChangeOrbStatUI(OrbData orb)
    {
        GetOrbData(orb);

        ballName.text = $"{orb.OrbName} (레벨{orb.OrbLevel})";
        atkText.text = $"{normalAtk}";
        critText.text = $"{critAtk}";
        description.text = $"{orb.description}";
    }
    // 오브 패널의 이미지를 변경
    private void ChangeOrbsPanelUI(OrbData orb)
    {
        orbImage.sprite = orb.prefab.GetComponent<SpriteRenderer>().sprite;
        GetPanelOrbs();
    }
    // 오브 패널 비우기
    private void RemoveOrbPanelChild()
    {
        Transform targetTransform = orbsPanel.GetComponent<Transform>();
        if (targetTransform.childCount == 0)
            GetNewOrbs();
        else
            Destroy(targetTransform.GetChild(0).gameObject);
    }
    // 오브 스폰 위치에 오브 배치
    private void CreateBall(OrbData orb)
    {
        if (orbRb != null) return;

        orbRb = Instantiate(currentBall.GetComponent<Rigidbody2D>(),
            shootPoint.position, Quaternion.identity);
    }
    // 생성될 오브의 리지드바디를 비운다
    public void ClearBall()
    {
        orbRb = null;
    }
    // 새 오브 리스트를 가져온다
    private void GetNewOrbs()
    {
        orbList = player.myOrbList.ToList();
        ShuffleList(orbList);
        currentBall = orbList[0].prefab;
    }
    // 오브 패널에 이미지 생성
    private void GetPanelOrbs()
    {
        if (orbsPanel.transform.childCount >= orbList.Count-1) return;

        for (int i = 1; i < orbList.Count; i++)
        {
            // 오브 백그라운드
            GameObject orbImage = new GameObject($"OrbBG_{i}", typeof(RectTransform), typeof(Image));
            RectTransform bgRect = orbImage.GetComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(60, 60);     // 사이즈 설정
            orbImage.transform.SetParent(orbsPanel.transform, false);
            Image backImg = orbImage.GetComponent<Image>();

            switch (orbList[i].OrbLevel)
            {
                case 1:
                    backImg.sprite = orbBG_One;
                    break;
                case 2:
                    backImg.sprite = orbBG_Two;
                    break;
                case 3:
                    backImg.sprite = orbBG_Three;
                    break;
            }

            // 오브 리스트 이미지
            GameObject panelOrb = new GameObject($"MyOrb_{i}", typeof(RectTransform), typeof(Image));
            RectTransform orbRect = panelOrb.GetComponent<RectTransform>();
            // 앵커 포인트 스트레치 설정
            orbRect.anchorMin = new Vector2(0, 0);
            orbRect.anchorMax = new Vector2(1, 1);
            orbRect.offsetMin = new Vector2(12, 12);
            orbRect.offsetMax = new Vector2(-12, -12);
            panelOrb.transform.SetParent(orbImage.transform, false);
            Image img = panelOrb.GetComponent<Image>();
            img.sprite = orbList[i].prefab.GetComponent<SpriteRenderer>().sprite;
        }
    }
    // 리스트 랜덤 섞기
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
    // 오브 데이터 확보
    public virtual void GetOrbData(OrbData data)
    {
        if (data.OrbLevel == 1)
        {
            normalAtk = data.atk_One + player.Buff_Str;
            critAtk = data.crit_One + player.Buff_Crit;
            orbRing.sprite = OrbRing_One;
        }
        else if (data.OrbLevel == 2)
        {
            normalAtk = data.atk_Two + player.Buff_Str;
            critAtk = data.crit_Two + player.Buff_Crit;
            orbRing.sprite = OrbRing_Two;
        }
        else if (data.OrbLevel == 3)
        {
            normalAtk = data.atk_Three + player.Buff_Str;
            critAtk = data.crit_Three + player.Buff_Crit;
            orbRing.sprite = OrbRing_Three;
        }
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null) return; //없으면 하지말고

        Gizmos.color = Color.yellow;//노란색
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}
