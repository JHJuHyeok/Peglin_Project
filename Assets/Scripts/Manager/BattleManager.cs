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

    private Vector2 aimDir;                     // 마우스 위치

    [Header("마우스 커서")]
    [SerializeField] private Vector2 cursorSpot;
    [SerializeField] private Texture2D cursorIdle;
    [SerializeField] private Texture2D cursorSelect;
    [SerializeField] private Texture2D cursorAim;
    [SerializeField] private Texture2D cursorCant;

    // 턴 확인
    public bool isMyTurn = true;
    public bool isOrbFall = false;
    private bool isProcessingTurn = false;


    private void Awake()
    {

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
    }

    private void Update()
    {
        if (!isProcessingTurn)
        {
            if (isMyTurn)   // 내 턴
            {
                StartCoroutine(MyTurn());

                // 마우스 기준으로 방향 계산
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0f;
                aimDir = (mouseWorld - shootPoint.position).normalized;

                // 궤적 표시
                predictor.ShowTrajectory(aimDir);

                if (Input.GetMouseButtonDown(0))
                {
                    isMyTurn = false;
                    //isProcessingTurn = false;
                    Fire();
                }
            }
            else            // 적 턴
            {
                StartCoroutine(CantControlTurn());
            }
        }
    }

    public IEnumerator MyTurn()
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

        yield return null;
        isProcessingTurn = false;
    }

    public IEnumerator CantControlTurn()
    {
        isProcessingTurn = true;

        Cursor.SetCursor(cursorCant, cursorSpot, CursorMode.Auto);

        if (isOrbFall)
        {
            orbList.Remove(orbList[0]);
            RemoveOrbPanelChild();
            UpdateOrbUI(orbList[0]);
            isOrbFall = false;
        }
        yield return null;
        isProcessingTurn = false;
    }

    // 오브 발사
    private void Fire()
    {
        orbRb.gravityScale = predictor.gravityScale;
        orbRb.AddForce(aimDir * power, ForceMode2D.Impulse);
        StopCoroutine(MyTurn());
        predictor.HideTrajectory();
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
        Destroy(targetTransform.GetChild(1).gameObject);
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


}
