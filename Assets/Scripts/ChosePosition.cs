using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ChosePosition : MonoBehaviour
{
    public GameObject currencyPosition;
    public GameObject location;
    public Text gameName;
    public Text coin;
    public Text mana;
    public Text something;
    private GameObject currencyLocation;
    [SerializeField] private float coinLastValue;
    [SerializeField] private float manaLastValue;
    [SerializeField] private float somethingLastValue;
    [SerializeField] private List<GameObject> stars;
    private bool isChange;

    [SerializeField] [ItemCanBeNull] private List<GameObject> locations;
    [SerializeField] private List<Slider> sliders; // 3 sliders: something, mana, coin
    [SerializeField] private List<Info> locationsInfo;
    [SerializeField] private int totalCoin;
    [SerializeField] private int totalMana;
    [SerializeField] private int totalSomething;
    private int playerLocation;

    private class Info
    {
        public string name;
        public float mana;
        public float coin;
        public float something;
        public int stars;
    }

    private void Update()
    {
        coin.text = totalCoin.ToString();
        mana.text = totalMana.ToString();
        something.text = totalSomething.ToString();

        if (playerLocation > -1)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                if (locationsInfo[playerLocation].stars < 1)
                {
                    stars[playerLocation * 3].SetActive(true); 
                }
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                if (locationsInfo[playerLocation].stars < 2)
                {
                    stars[playerLocation * 3].SetActive(true); 
                    stars[playerLocation * 3 + 1].SetActive(true); 
                }
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                if (locationsInfo[playerLocation].stars < 3)
                {
                    stars[playerLocation * 3].SetActive(true); 
                    stars[playerLocation * 3 + 1].SetActive(true); 
                    stars[playerLocation * 3 + 2].SetActive(true); 
                }
            }
        }
    }

    private void Start()
    {
        currencyPosition.SetActive(false);
        location.SetActive(false);

        playerLocation = -1;

        locationsInfo = new List<Info>();
        Random random = new Random();

        for (var i = 0; i < locations.Count; i++)
        {
            locationsInfo.Add(new Info()
            {
                name = "location " + i,
                mana = (float)random.NextDouble(),
                coin = (float)random.NextDouble(),
                something = (float)random.NextDouble()
            });
        }

        for (var i = 0; i < stars.Count; i++)
        {
            stars[i].SetActive(false);
        }
    }

    public void ManaChanged()
    {
        if (!isChange)
        {
            return;
        }
        
        var value = sliders[1].value - manaLastValue;
        
        if (value > totalMana)
        {
            sliders[1].value = (float) manaLastValue / 100;
            return;
        }

        totalMana -= (int) (value * 1000);
        manaLastValue = sliders[1].value;
    }

    public void CoinChanged()
    {
        if (!isChange)
        {
            return;
        }
        
        var value = sliders[2].value - coinLastValue;
        
        if (value > (float) totalCoin)
        {
            sliders[2].value = (float) coinLastValue / 100;
            return;
        }

        totalCoin -= (int) (value * 1000);
        coinLastValue = sliders[2].value;
    }

    public void SomethingChanged()
    {
        if (!isChange)
        {
            return;
        }
        
        var value = sliders[0].value - somethingLastValue;
        
        if (value > totalSomething)
        {
            sliders[0].value = (float) somethingLastValue / 100;
            return;
        }

        totalSomething -= (int) (value * 1000);
        somethingLastValue = sliders[0].value;
    }

    public void CloseLocation()
    {
        location.SetActive(false);
        currencyPosition.SetActive(false);

        currencyLocation.transform.localScale = new Vector3(1f, 1f, 1f);

        playerLocation = -1;
    }

    public void GoToLocation(GameObject location)
    {
        isChange = false;
        location.transform.localScale = new Vector3(1.1f, 1.1f, 1f);

        currencyLocation = location;
        currencyPosition.gameObject.transform.position = currencyLocation.transform.position;
        currencyPosition.transform.position += new Vector3(10f, 20f, 0);
        
        currencyPosition.SetActive(true);
        this.location.SetActive(true);

        var numberOfLocation = 0;
        
        for (var i = 0; i < locations.Count; i++)
        {
            if (locations[i] == location)
            {
                numberOfLocation = i;
                break;
            }
        }

        sliders[0].value = locationsInfo[numberOfLocation].something;
        somethingLastValue = locationsInfo[numberOfLocation].something;
        sliders[1].value = locationsInfo[numberOfLocation].mana;
        manaLastValue = locationsInfo[numberOfLocation].mana;
        sliders[2].value = locationsInfo[numberOfLocation].coin;
        coinLastValue = locationsInfo[numberOfLocation].coin;
        gameName.text = locationsInfo[numberOfLocation].name;

        playerLocation = numberOfLocation;
        
        isChange = true;
    }
}
