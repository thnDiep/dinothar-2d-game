using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClueCollection : MonoBehaviour
{
    [SerializeField] Image clueIcon;
    [SerializeField] Image clueCollection;
    [SerializeField] ClueUI[] cluesUI;
    [SerializeField] GameObject[] pages;
    [SerializeField] TextMeshProUGUI pageNumber;
    private int currentIndex;

    private Color originalColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Color shineColor = new Color(1f, 1f, 1f, 1f);
    //private int collectedClueNumber = 0;

    private float shineTime;
    private float shineTimer = 7f;

    private bool collected;
    float elapsedTime = 0f;

    private void Start()
    {
        GameManager.ClueChangedEvent += updateClueCollection;
        for (int i = 0; i < pages.Length; i++)
        {
            if (i == currentIndex)
                pages[i].gameObject.SetActive(true);
            else
                pages[i].gameObject.SetActive(false);
        }
        updateClueCollection(GameManager.Instance.getClueCollection());

        clueCollection.gameObject.SetActive(false);
        shineTime = shineTimer;
        collected = false;
        clueIcon.color = originalColor;
        currentIndex = 0;
        pageNumber.text = (currentIndex + 1).ToString() + "/" + pages.Length.ToString();
    }

    private void OnDestroy()
    {
        GameManager.ClueChangedEvent -= updateClueCollection;
    }

    private void Update()
    {
        if (collected)
        {
            shineTime -= Time.deltaTime;
            elapsedTime += Time.deltaTime;

            clueIcon.color = Color.Lerp(originalColor, shineColor, Mathf.PingPong(elapsedTime, 1f));

            if (shineTime < 0)
            {
                shineTime = shineTimer;
                collected = false;
                clueIcon.color = originalColor;
            }
        }
    }

    public void updateClueCollection(int[] clues)
    {
        collected = true;
        clueIcon.color = shineColor;

        for(int i = 0; i < clues.Length; i++)
        {
            if (clues[i] == 0)
                cluesUI[i].lockClue();
            else
                cluesUI[i].unlockClue();
        }
        //collectedcluenumber++;
        //if (collectedcluenumber == 1)
        //{
        //    clue1.gameobject.setactive(true);
        //}
        //else if (collectedcluenumber == 2)
        //{
        //    clue2.gameobject.setactive(true);
        //}
        //else
        //{
        //    clue3.gameobject.setactive(true);
        //}
    }

    public void openClueCollection()
    {
        clueCollection.gameObject.SetActive(true);
        // mở lên coi rồi thì không sáng nữa
        shineTime = shineTimer;
        collected = false;
        clueIcon.color = originalColor;
    }

    public void closeClueCollection()
    {
        clueCollection.gameObject.SetActive(false);
    }   

    public void nextPage()
    {
        pages[currentIndex].gameObject.SetActive(false);
        currentIndex = (currentIndex + 1) % pages.Length;
        pages[currentIndex].gameObject.SetActive(true);
        pageNumber.text = (currentIndex + 1).ToString() + "/" + pages.Length.ToString();
    }

    public void prevPage()
    {
        pages[currentIndex].gameObject.SetActive(false);
        currentIndex = (currentIndex - 1 + pages.Length) % pages.Length;
        pages[currentIndex].gameObject.SetActive(true);
        pageNumber.text = (currentIndex + 1).ToString() + "/" + pages.Length.ToString();
    }
}