using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSlider : MonoBehaviour
{
    [Header("Slide Settings")]
    public List<GameObject> slides; // Her bir slide (GameObject) burada tutulacak
    public Transform indicatorParent; // Altındaki yuvarlakların tutulacağı parent
    public GameObject indicatorPrefab; // Yuvarlak göstergeler için prefab
    public Color activeColor = Color.white; // Seçili yuvarlağın rengi
    public Color inactiveColor = Color.gray; // Seçili olmayan yuvarlağın rengi

    [Header("Swipe Settings")]
    public float swipeThreshold = 50f; // Kaç pikselden sonra kaydırma algılanacak

    private int currentIndex = 0;
    private List<Image> indicators = new List<Image>();
    private Vector2 touchStartPos;
    public ScrollRect scrollRect;
    private RectTransform rectTransform;

    void Start()
    {
        // Göstergeleri oluştur ve ilk slide'ı aktif yap
        SetupIndicators();
        UpdateSlides();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        HandleSwipeInput();
    }

    private void HandleSwipeInput()
    {
        if (Input.GetMouseButtonDown(0) && IsPointerOverThisUI()) // Mouse tıklaması sadece bu GameObject'in üzerine olduğunda işleme alınacak
        {
            touchStartPos = Input.mousePosition;
            DisableScroll(); // Kaydırma devre dışı bırakılır
        }
        else if (Input.GetMouseButtonUp(0)) // Dokunma bırakıldığında
        {
            Vector2 touchEndPos = Input.mousePosition;
            float swipeDistance = touchEndPos.x - touchStartPos.x;

            if (Mathf.Abs(swipeDistance) > swipeThreshold)
            {
                if (swipeDistance > 0) // Sağ kaydırma
                {
                    ShowPreviousSlide();
                }
                else // Sol kaydırma
                {
                    ShowNextSlide();
                }
            }
            EnableScroll(); // Kaydırma tekrar etkinleştirilir
        }
    }

    private void ShowNextSlide()
    {
        if (currentIndex < slides.Count - 1)
        {
            currentIndex++;
            UpdateSlides();
        }
    }

    private void ShowPreviousSlide()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateSlides();
        }
    }

    private void UpdateSlides()
    {
        // Tüm slide'ları kapat
        for (int i = 0; i < slides.Count; i++)
        {
            slides[i].SetActive(i == currentIndex);
        }

        // Göstergeleri güncelle
        for (int i = 0; i < indicators.Count; i++)
        {
            indicators[i].color = (i == currentIndex) ? activeColor : inactiveColor;
        }
    }

    private void SetupIndicators()
    {
        foreach (Transform child in indicatorParent)
        {
            Destroy(child.gameObject); // Eski göstergeleri temizle
        }
        indicators.Clear();

        for (int i = 0; i < slides.Count; i++)
        {
            GameObject indicator = Instantiate(indicatorPrefab, indicatorParent);
            Image indicatorImage = indicator.GetComponent<Image>();
            if (indicatorImage != null)
            {
                indicators.Add(indicatorImage);
            }
        }
    }

    private bool IsPointerOverThisUI()
    {
        // Mouse'un sadece CardSlider objesinin üzerinde olup olmadığını kontrol et
        // RectTransform ile kontrol yapıyoruz
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Mouse pozisyonu ve CardSlider rectTransform'u kullanarak kontrol edelim
            Vector2 localPointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localPointerPosition);
            return rectTransform.rect.Contains(localPointerPosition);
        }
        return false;
    }

    public void DisableScroll()
    {
        if (scrollRect != null)
        {
            scrollRect.enabled = false; // Kaydırma devre dışı
        }
    }

    // ScrollRect'i tekrar etkinleştirme
    public void EnableScroll()
    {
        if (scrollRect != null)
        {
            scrollRect.enabled = true; // Kaydırma etkin
        }
    }
}