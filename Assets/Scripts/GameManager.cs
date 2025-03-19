using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI CardScore;

    public int initCardNumber;
    public int maxCardInOurHand;
    public int maxCardInTable;

    public int cardSpacing;

    public bool isLerping;

    [SerializeField]
    private int _addcartnumber;
    public int addCartNumber = 2;
    public int addBoostedCartNumber = 3;

    public int cardSum;

    public bool JokerIsInHand;

    public List<GameObject> cards = new List<GameObject>();
    public List<GameObject> cardNumbersTable = new List<GameObject>();
    public List<GameObject> cardsInHand = new List<GameObject>();

    public List<Sprite> cardImages;


    //Unity Component
    public GameObject cardPrefab;

    public GameObject cardTableParent;
    public GameObject cardHandParent;

    public Transform startPosition;
    public List<GameObject> buildings = new List<GameObject>();
    public float buildingHeight = 16.0f / 20;
    public float buildingWidth = 4.5f / 20;

    public GameObject backGraundImage;
    //percentage

    public float jokerPercent;
    public float ascenderPercent;
    public float descenderPercent;
    public float cardGeneratorPercent;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        CardScore.text = "0 / 7";
        InitGame();
        CheckGameStatus();
        Time.timeScale = 1.0f;
    }
    public void InitGame()
    {
        scoreText.text = "Score: " + score;
        _addcartnumber = addCartNumber;
        //bize ilkin olaraq 6 eded kart verecek. Kartlar random olmalidir.
        for (int i = 0; i < initCardNumber; i++)
        {
            int a = Random.Range(1, 7);
            GameObject go = Instantiate(cardPrefab, cardHandParent.transform);
            go.GetComponent<Card>().SetCard(a);
            cards.Add(go);
        }
    }

    public int CheckInitCarts(List<int> cards)
    {

        //if()

        return 1;
    }

    public bool CheckCardsSum()
    {
        int sum = 0;
        foreach (var item in cardsInHand)
        {
            sum += item.GetComponent<Card>().cardNumber;
            if (item.GetComponent<Card>().cardNumber == 7)
            {
                Debug.Log("Joker is in hand");
                return true;
            }
        }

        if (sum == cardSum)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool checkJokerBool()
    {
        foreach (var item in cards)
        {
            if (item.GetComponent<Card>().cardType == cardType.Joker) return true;
        }
        return false;
    }
    public int checkJoker()
    {
        int sum = 0;

        foreach (var item in cardsInHand)
        {
            if (item.GetComponent<Card>().cardType != cardType.Joker)
                sum += item.GetComponent<Card>().cardNumber;
        }

        Debug.Log("Joker olanda: " + (cardSum - sum));
        return cardSum - sum;
    }
    public void BuildConstruction()
    {
        Debug.Log(checkJoker() + " lalalala");
        if (checkJoker() < 0 || checkJoker() == 7) return;

        if (!CheckCardsSum())
        {
            Debug.Log("nOT EqUAl TO 7!");
            return;
        }

        Vector3 init = startPosition.position + Vector3.up * (buildingHeight / 2) + Vector3.left * (buildingWidth / 2 * 7);
        Debug.DrawLine(startPosition.position, init, Color.black);

        float sum = 0;
        bool jokerActivated = false;
        string test = "";
        foreach (var item in cardsInHand)
        {
            if (item.GetComponent<Card>().cardType == cardType.Joker && !jokerActivated)
            {
                test += " - ";
                jokerActivated = true;

                if (cardsInHand.Count == 1)
                {
                    Debug.Log("Basqa kart gel");
                    return;
                }

                int temp = (checkJoker() < 7) ? temp = checkJoker() : temp = 6;

                Instantiate(buildings[temp - 1], init + Vector3.right * (buildingWidth * temp / 2 + sum), Quaternion.identity);

                sum += buildingWidth * temp;
            }
            else
            {
                if (item.GetComponent<Card>().cardType != cardType.Joker)
                {
                    test += " + ";
                    Instantiate(buildings[item.GetComponent<Card>().cardNumber - 1], init + Vector3.right * (buildingWidth * item.GetComponent<Card>().cardNumber / 2 + sum), Quaternion.identity);
                    sum += buildingWidth * item.GetComponent<Card>().cardNumber;
                }
            }

        }
        Debug.Log(test);
        SpecCardInHand();

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            cardNumbersTable.Remove(cardsInHand[i].gameObject);
            cards.Remove(cardsInHand[i].gameObject);
            Destroy(cardsInHand[i].gameObject);
        }

        //------Qezenfer ----------------------------------------------------------------- Axici sekilde hereket etsin

        startPosition.position += buildingHeight * Vector3.up;
        isLerping = true;

        cardsInHand.Clear();

        NextTour();
    }

    IEnumerator LerpFromTo(Vector3 pos1, Vector3 pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            Camera.main.transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            backGraundImage.transform.position -= Vector3.up * Time.deltaTime * 0.05f;
            yield return 0;
        }
        isLerping = false;
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("GameOver");
        AudioManager.instance.PlaySFX("Fail", 0);
        ButtonManager.instance.GameOver.SetActive(true);
        ButtonManager.instance.GameOverScoreText.text = score.ToString();
    }
    private void Update()
    {
        if (isLerping)
        {
            StartCoroutine(LerpFromTo(Camera.main.transform.position, Camera.main.transform.position + Vector3.up * buildingHeight / 2, 0.5f));
        }
    }

    public void ViewScore()
    {
        int cardScoreNow = 0;

        foreach (GameObject go in cardsInHand)
        {
            cardScoreNow += go.GetComponent<Card>().cardNumber;
        }
        
        CardScore.text = cardScoreNow.ToString() + " / 7";
    }

    public void SpecCardInHand()
    {
        foreach (var item in cards)
        {
            if (item.GetComponent<Card>().cardType == cardType.CardGenerator)
            {
                _addcartnumber = addBoostedCartNumber;
            }
        }
    }

    public void AddNewCards()
    {
        if (Random.Range(1, 100) <= 50)
        {
            AddCardToList(Random.Range(1, 7));
        }
        else
        {
            int val = Random.Range(1, 100);
            Debug.Log("Percent: " + val);

            if (val > 0 && val <= jokerPercent)
            {
                AddCardToList(7);
            }
            if (val > jokerPercent && val <= jokerPercent + ascenderPercent)
            {
                AddCardToList(8);
            }
            if (val > jokerPercent + ascenderPercent && val <= jokerPercent + ascenderPercent + descenderPercent)
            {
                AddCardToList(9);
            }
            if (val > jokerPercent + ascenderPercent + descenderPercent && val <= 100)
            {
                AddCardToList(10);
            }
        }
    }

    //1-6 normal kart, 7 - joker, 8 - asc, 9 - desc
    public void AddCardToList(int number)
    {

        GameObject go = Instantiate(cardPrefab, cardHandParent.transform);
        go.GetComponent<Card>().SetCard(number);
        cards.Add(go);
    }

    public void NextTour()
    {
        score++;
        scoreText.text = "Score: " + score;

        SpecCards();
        if (cards.Count < maxCardInOurHand) for (int i = 0; i < _addcartnumber; i++)
            {
                if (cards.Count >= maxCardInOurHand) break;
                AddNewCards();
            }
        else Debug.Log("Ayeee");

        Refresh();

        _addcartnumber = addCartNumber;

        if (!CheckGameStatus())
        {
            StartCoroutine(GameOver());
        }
    }

    public void SpecCards()
    {
        foreach (var item in cards)
        {
            if (item.GetComponent<Card>().cardType == cardType.Ascender)
            {
                if (item.GetComponent<Card>().cardNumber < 6)
                {
                    item.GetComponent<Card>().cardNumber++;
                    item.GetComponent<Card>().Refresh();
                }
                else
                {
                    item.GetComponent<Card>().cardNumber = 1;
                    item.GetComponent<Card>().Refresh();
                }
            }
            if (item.GetComponent<Card>().cardType == cardType.Descender)
            {
                if (item.GetComponent<Card>().cardNumber > 1)
                {
                    item.GetComponent<Card>().cardNumber--;
                    item.GetComponent<Card>().Refresh();
                }
                else
                {
                    item.GetComponent<Card>().cardNumber = 6;
                    item.GetComponent<Card>().Refresh();
                }
            }
        }
    }

    public void Refresh()
    {
        cardHandParent.GetComponent<GridLayoutGroup>().spacing = (cardSpacing - (cards.Count > initCardNumber ? 20 * (cards.Count - initCardNumber) : 0)) * Vector3.right;
        // Debug.Log("Refreshhh " + (cards.Count > initCardNumber ? 10 * (cards.Count - initCardNumber) : 0) + "  ---  " + (cardSpacing - (cards.Count > initCardNumber ? 10 * (cards.Count - initCardNumber) : 0)) * Vector3.right);
    }

    static bool HasCombinationWithSum(List<int> numbers, int targetSum, int k)
    {
        return FindCombination(numbers, targetSum, k, 0, new List<int>());
    }

    static bool FindCombination(List<int> numbers, int targetSum, int k, int startIndex, List<int> currentCombination)
    {
        if (currentCombination.Count == k)
        {
            if (currentCombination.Sum() == targetSum)
            {
                return true;
            }
            return false;
        }

        for (int i = startIndex; i < numbers.Count; i++)
        {
            currentCombination.Add(numbers[i]);

            if (FindCombination(numbers, targetSum, k, i + 1, currentCombination))
            {
                return true;
            }

            currentCombination.RemoveAt(currentCombination.Count - 1);
        }

        return false;
    }
    public bool CheckGameStatus()
    {

        if (checkJokerBool()) return true;

        List<int> list = new List<int>(sort(cards));


        for (int i = 1; i <= (list.Count >= maxCardInTable ? maxCardInTable : list.Count); i++)
        {
            if (HasCombinationWithSum(list, cardSum, i))
            {
                Debug.Log("Continue");
                return true;
            }
        }
        Debug.Log("End");
        StartCoroutine(GameOver());
        return false;


        /*
        for (int i = 0; i < list.Count; i++)
        {
            int sum = 0;
            sum += list[i];

            for (int j = 0; j < (list.Count >= maxCardInTable ? maxCardInTable : list.Count); j++)
            {
                for (int k = 0; k < (list.Count >= maxCardInTable ? maxCardInTable : list.Count); k++)
                {
                    sum += list[k];
                    for (int l = k; l < list.Count - 1; l++) 
                    {
                        for(int m = l + 1; m < list.Count; m++)
                        {
                            if(sum + list[m] < cardSum) break;
                            if(sum + list[m] == 7) return true;
                        }
                        sum -= list[l];
                    }
                }
            }
        }
        */
    }

    public List<int> sort(List<GameObject> olist)
    {
        List<int> list = new List<int>();

        foreach (GameObject o in olist)
        {
            list.Add(o.GetComponent<Card>().cardNumber);
        }

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                if (list[i] > list[j])
                {
                    int temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
        }

        //Debug.Log("count " + list.Count);
        string a = "";
        foreach (int o in list)
        {
            a += o.ToString() + "  ";
        }
        Debug.Log(a);


        return list;
    }
}
