using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using JetSystems;

[RequireComponent(typeof(Button))]
public class UITheme : MonoBehaviour
{
    [SerializeField] private Image _themeImage;
    [SerializeField] private Text _themeText;
    [SerializeField] private Text _themePriceText;

    private readonly UnityEvent<UITheme, ThemeData> _themeAvailableClicked = new UnityEvent<UITheme, ThemeData>();

    private ThemeData _themeData;
    private bool _isAvaialble;


    public UnityEvent<UITheme, ThemeData> ThemeAvailableClicked => _themeAvailableClicked;
    public ThemeData ThemeData => _themeData;


    private Button _cachedButton;
    public Button CachedButton
    {
        get
        {
            if (_cachedButton == null)
                _cachedButton = GetComponent<Button>();
            return _cachedButton;
        }
    }


    private void Awake()
    {
        CachedButton.onClick.AddListener(OnThemeButtonClick);
    }

    private void OnDestroy()
    {
        CachedButton.onClick.RemoveListener(OnThemeButtonClick);
        ThemeAvailableClicked.RemoveAllListeners();
    }


    private void OnThemeButtonClick()
    {
        if (_isAvaialble)
        {
            ThemeAvailableClicked.Invoke(this, _themeData);
            return;
        }

        if (UIManager.COINS < _themeData.Price)
        {
            Debug.Log("But failed!");
        }
        else
        {
            UIManager.RemoveCoins(_themeData.Price);
            SetAvailable(true);
            PlayerPrefsManager.AddUnlockedTheme(_themeData.Id);
        }
    }

    public void SetData(ThemeData themeData, bool isAvailable)
    {
        if(themeData.Sprite != null)
        {
            _themeText.gameObject.SetActive(false);
            _themeImage.sprite = themeData.Sprite;
            _themeImage.gameObject.SetActive(true);
        }
        else
        {
            _themeImage.gameObject.SetActive(false);
            _themeText.text = themeData.Name;
            _themeText.gameObject.SetActive(true);
        }

        _themePriceText.text = themeData.Price.ToString();

        if (PlayerPrefsManager.HasUnlokedTheme(themeData.Id))
            isAvailable = true;

        SetAvailable(isAvailable);

        _themeData = themeData;
    }

    public void SetAvailable(bool available)
    {
        _themePriceText.transform.parent.gameObject.SetActive(!available);
        _isAvaialble = available;
    }
}


