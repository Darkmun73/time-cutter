using System.Collections;
using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class TutorialController : MonoBehaviour // TODO: Переделать на StatePattern?
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private InputReader inputReader;

    [SerializeField] private TutorialPage empty;
    [SerializeField] private TutorialPage page1;
    [SerializeField] private TutorialPage page2p1;
    [SerializeField] private TutorialPage page2p2;
    [SerializeField] private TutorialPage page3p1;
    [SerializeField] private TutorialPage page3p2;

    [SerializeField] private float tutorialStartTime;

    private TutorialPage currentPage = null;

    void Awake()
    {
        videoPlayer.clip = null;
    }

    void Start()
    {
        StartTutorial();
    }

    void OnEnable()
    {
        inputReader.Continuing += TurnPage;
    }

    void OnDisable()
    {
        inputReader.Continuing -= TurnPage;
    }

    private void OpenPage(TutorialPage page)
    {
        page.gameObject.SetActive(true);
        page.Activate(videoPlayer);

        if (page != empty)
            inputReader.ToggleTutorial();

        currentPage = page;
    }

    private void StartTutorial()
    {
        StartCoroutine(StartTutorialRoutine());
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Tutorial");
    }

    private IEnumerator StartTutorialRoutine()
    {
        yield return new WaitForSeconds(tutorialStartTime);
        OpenPage(page1);
    }

    private void CloseCurrentPage()
    {
        currentPage.gameObject.SetActive(false);
        currentPage.Deactivate(videoPlayer);

        if (currentPage != empty)
            inputReader.ToggleTutorial();

        currentPage = null;
    }

    private void TurnPage()
    {
        TutorialPage newPage = null;
        if (currentPage == page1)
            newPage = page2p1;
        else if (currentPage == page2p1)
            newPage = page2p2;
        else if (currentPage == page2p2)
            newPage = empty;
        else if (currentPage == empty)
            newPage = page3p1;
        else if (currentPage == page3p1)
            newPage = page3p2;
        
        CloseCurrentPage();
        if (newPage != null)
            OpenPage(newPage);
        else
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Tutorial");
            SceneLoader.LoadLevel1(); // TODO: выглядит захардкожено. Делал через ивенты, но статик не применить
        }
            
    }
}
