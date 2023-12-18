using UnityEngine;
using UnityEngine.UI;

public class ClueUI : MonoBehaviour
{
    public Image lockImage;
    public Image contentImage;

    public void lockClue()
    {
        lockImage.gameObject.SetActive(true);
        contentImage.gameObject.SetActive(false);
    }

    public void unlockClue()
    {
        lockImage.gameObject.SetActive(false);
        contentImage.gameObject.SetActive(true);
    }
}