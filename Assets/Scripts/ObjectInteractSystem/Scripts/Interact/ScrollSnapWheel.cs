using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ScrollSnapWheel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] List<RectTransform> spritesToCycle;
    // [SerializeField] Image displayImage; // UI obj that shows the current sprite
    [SerializeField] float scrollSpeed = 200f;
    [SerializeField] float snapSpeed = 10f; // How fast it snaps once dragging stops
    [SerializeField] RectTransform verticalContent;
    [SerializeField] int correctNumberCode;
    private int currentIndex = 0; // current index in spritesToCycle that will show is displayImage
    private float scrollOffset = 0;
    private bool isSnapping = false; // true at the end of drag
    private float spriteHeight;
    private int currentSelectedNumberIndex;
    private int currentSelectedNumber;

    private bool isSelectedNumberCorrect;
    void Start()
    {
        // if (displayImage == null) displayImage = GetComponent<Image>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            spritesToCycle.Add(transform.GetChild(i).GetComponent<RectTransform>());
        }

        // NOTE: Bottom Of midpoint must be less than top of midpoint

        SetCurrentSelectedNumber();

        spriteHeight = spritesToCycle[0].rect.height;
        

        //UpdateSprite();
    }

    void Update()
    {
        if (isSnapping)
        {
            // Position to snap to
            // Example like if pos = 245 and height = 100 -> 2.45 round to 2 * 100 = 200 -> target snap pos
            Vector2 target = new Vector2(0, Mathf.Round(verticalContent.anchoredPosition.y / spriteHeight) * spriteHeight);
            verticalContent.anchoredPosition = Vector2.Lerp(verticalContent.anchoredPosition, target, Time.deltaTime * snapSpeed);
            
            if (Mathf.Abs(target.y - verticalContent.anchoredPosition.y) < 0.1f)
            {
                SetCurrentSelectedNumber();
                verticalContent.anchoredPosition = target;
                isSelectedNumberCorrect = currentSelectedNumber == correctNumberCode ? true : false;
                if (isSelectedNumberCorrect) Debug.Log("Digit is correct!");
                BoxPuzzleManager.instance.CheckCode();
                isSnapping = false;
            }
        }
    }
    private int GetCurrentSelectedNumber() // DOES NOT SET THE PRIVATE FIELD 
    {
        currentSelectedNumberIndex = transform.childCount % 2 == 0 ?
            Mathf.RoundToInt(transform.childCount / 2) - 1 : Mathf.CeilToInt(transform.childCount / 2) - 1;
        string textOfCurrentSelectedNumber =
            transform.GetChild(currentSelectedNumberIndex).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        int currentSelectedNumber = int.Parse(textOfCurrentSelectedNumber);
        Debug.Log($"Current Lock Number: {currentSelectedNumber}");

        return currentSelectedNumber;
    }

    private void SetCurrentSelectedNumber()
    {
        this.currentSelectedNumber = GetCurrentSelectedNumber();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isSnapping = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        scrollOffset = eventData.delta.y * scrollSpeed;
        verticalContent.anchoredPosition += new Vector2(0, scrollOffset);

        if (verticalContent.anchoredPosition.y >= spriteHeight/2) // drag up
        {
            verticalContent.GetChild(0).SetAsLastSibling();
            verticalContent.anchoredPosition -= new Vector2(0, spriteHeight);
        }

        if (verticalContent.anchoredPosition.y <= -spriteHeight/2)
        {
            verticalContent.GetChild(verticalContent.childCount - 1).SetAsFirstSibling();
            verticalContent.anchoredPosition += new Vector2(0, spriteHeight);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isSnapping = true;
    }

    public int GetCorrectNumberCode()
    {
        return correctNumberCode;
    }

    public bool GetIsSelectedNumberCorrect()
    {
        return isSelectedNumberCorrect;
    }


}
