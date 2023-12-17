using UnityEngine;
using UnityEngine.UI;


public class StarsOfLevel : MonoBehaviour
{
    // Start is called before the first frame update
    // public Sprite unCollectedStar;
    public Sprite collectedStar;

    [SerializeField] Image star1;
    [SerializeField] Image star2;
    [SerializeField] Image star3;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setActiveStarsOfLevel(bool isActive, int starNumber)
    {
        gameObject.SetActive(isActive);
        if (starNumber >= 1)
        {
            star1.sprite = collectedStar;
        }
        if (starNumber >= 2)
        {
            star2.sprite = collectedStar;
        }
        if (starNumber >= 3)
        {
            star3.sprite = collectedStar;
        }

    }
}
