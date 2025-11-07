using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    private PlayerManager player;
    private BattleManager battle;

    [Header("패널 제어")]
    [SerializeField] private GameObject blackPanel;
    [SerializeField] private GameObject backpackPanel;
    [SerializeField] private GameObject simpleMap;
    [SerializeField] private GameObject OptionPanel;
    [SerializeField] private GameObject rewardPanel;

    [Header("가방 요소")]
    private List<OrbData> orbList;
    [SerializeField] private GameObject backpackContent;
    [SerializeField] private Sprite orbBack_One;
    [SerializeField] private Sprite orbBack_Two;
    [SerializeField] private Sprite orbBack_Three;

    private bool isBackpackOn = false;

    public static PopupManager Instance { get; private set; }

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
        player = FindObjectOfType<PlayerManager>();
        orbList = player.myOrbList;

        for (int i = 0; i < orbList.Count; i++)
            AddOrbUI(orbList[i], backpackContent, orbBack_One, orbBack_Two, orbBack_Three);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isBackpackOn)
            OnBackpackBtn();
        else if (Input.GetKeyDown(KeyCode.I) && isBackpackOn)
            OffBackpackBtn();
        if (Input.GetKeyDown(KeyCode.Escape))
            OnOptionBtn();
        // 현재 씬이 게임씬일 때
        if (Input.GetKeyDown(KeyCode.M))
            OnMapBtn();
        // 승리할 시 리워드 패널 활성화

        if(Input.GetKeyUp(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

    }

    public void AddOrbUI(OrbData orb, GameObject parent, Sprite one, Sprite two, Sprite three)
    {
        //for (int i = 0; i < list.Count; i++)
        {
            // 오브 백그라운드
            GameObject orbImage = new GameObject($"OrbBG", typeof(RectTransform), typeof(Image));
            RectTransform bgRect = orbImage.GetComponent<RectTransform>();
            orbImage.transform.SetParent(parent.transform, false);
            Image backImg = orbImage.GetComponent<Image>();

            // 오브 레벨당 백 이미지
            switch (orb.OrbLevel)
            {
                case 1:
                    backImg.sprite = one;
                    break;
                case 2:
                    backImg.sprite = two;
                    break;
                case 3:
                    backImg.sprite = three;
                    break;
            }

            // 오브 리스트 이미지
            GameObject panelOrb = new GameObject($"MyOrb", typeof(RectTransform), typeof(Image));
            RectTransform orbRect = panelOrb.GetComponent<RectTransform>();
            orbRect.sizeDelta = new Vector2(60, 60);
            panelOrb.transform.SetParent(orbImage.transform, false);
            Image img = panelOrb.GetComponent<Image>();
            img.sprite = orb.prefab.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void OnBackpackBtn()
    {
        Time.timeScale = 0;
        blackPanel.SetActive(true);
        backpackPanel.SetActive(true);
        isBackpackOn = true;
    }

    public void OffBackpackBtn()
    {
        Time.timeScale = 1;
        backpackPanel.SetActive(false);
        blackPanel.SetActive(false);
        isBackpackOn = false;
    }

    public void OnMapBtn()
    {
        Time.timeScale = 0;
    }

    public void OffMapBtn()
    {
        Time.timeScale = 1;
    }

    public void OnOptionBtn()
    {
        Time.timeScale = 0;
    }

    public void OffOptionBtn()
    {
        Time.timeScale = 1;
    }
}
