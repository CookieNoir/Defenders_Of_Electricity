using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int PlayerMoney { set; get; } = 500;
    public int healthPoints = 100;
    private int currentHealthPoints;
    public Transform healthPointsBar;
    public int energyPoints = 100;
    private int currentEnergyPoints;
    public int energyIncomePerSecond;
    private float energyAdditionCooldown;
    public Transform energyPointsBar;
    public Text incomeText;

    public Transform reactor;
    public Transform[] spawnPoints;
    public GameObject[] enemies;

    private float spawnCooldown;
    public float spawnRate;
    public float additionPerSecond1;
    public float additionPerSecond2;

    public Text moneyText;
    public List<Enemy> enemiesList;

    public AreaControl[] areas;
    public AudioListener listener;
    public bool audioTurnedOn;
    public Image audioTurnOnImage;
    public Sprite audioTurnedOnSprite;
    public Sprite audioTurnedOffSprite;

    public float reactorMaxIntensity;
    public float reactorMinIntensity;
    public Light reactorLight;
    public AudioSource backgroundSound;

    public GameObject RestartMenu;
    public GameObject playerInterface;

    public void ChangeAudio()
    {
        audioTurnedOn = !audioTurnedOn;
        backgroundSound.enabled = audioTurnedOn;
        for (int i = 0; i < areas.Length; ++i)
        {
            for (int j = 0; j < areas[i].nodes.Length; ++j)
            {
                if (areas[i].nodes[j].weapon) areas[i].nodes[j].weapon.shot.enabled = audioTurnedOn;
            }
        }
        listener.enabled = audioTurnedOn;
        if (audioTurnedOn) audioTurnOnImage.sprite = audioTurnedOnSprite;
        else audioTurnOnImage.sprite = audioTurnedOffSprite;
    }

    public void UpdateMoney(int value)
    {
        PlayerMoney += value;
        moneyText.text = PlayerMoney.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateHealthPoints(int value)
    {
        currentHealthPoints += value;
        if (currentHealthPoints <= 0)
        {
            Time.timeScale = 0f;
            playerInterface.SetActive(false);
            RestartMenu.SetActive(true);
        }
        else
        {
            float scale = (currentHealthPoints * 1f) / healthPoints;
            healthPointsBar.localScale = new Vector3(scale, 1f, 1f);
        }
    }

    private void UpdateIncomeText()
    {
        int value = energyIncomePerSecond;
        for (int i = 0; i < areas.Length; ++i)
        {
            if (areas[i].isActive) value--;
        }
        if (value > 0)
        {
            incomeText.text = '+' + value.ToString();
        }
        else incomeText.text = value.ToString();
    }

    public void UpdateEnergyPoints()
    {
        int value = energyIncomePerSecond;
        for (int i = 0; i < areas.Length; ++i)
        {
            if (areas[i].isActive) value--;
        }
        currentEnergyPoints += value;
        if (currentEnergyPoints <= 0)
        {
            currentEnergyPoints = 0;
            for (int i = 0; i < areas.Length; ++i)
            {
                areas[i].TurnOff();
            }
        }
        else if (currentEnergyPoints > energyPoints)
        {
            currentEnergyPoints = energyPoints;
        }
        float scale = (currentEnergyPoints * 1f) / energyPoints;
        energyPointsBar.localScale = new Vector3(scale, 1f, 1f);
        reactorLight.intensity = Mathf.SmoothStep(reactorMinIntensity, reactorMaxIntensity, scale);
    }

    public void UpdateEnergyPoints(int value)
    {
        currentEnergyPoints = value;
        float scale = (currentEnergyPoints * 1f) / energyPoints;
        energyPointsBar.localScale = new Vector3(scale, 1f, 1f);
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            currentHealthPoints = 0;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SpawnEnemy()
    {
        int spawnPoint = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemies.Length);
        Enemy newEnemy = Instantiate(enemies[enemyIndex], spawnPoints[spawnPoint].position, spawnPoints[spawnPoint].rotation).GetComponent<Enemy>();
        newEnemy.SetDestination(reactor);
        enemiesList.Add(newEnemy);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        UpdateMoney(0);
        UpdateHealthPoints(healthPoints);
        UpdateEnergyPoints(energyPoints / 2);
        ChangeAudio();
    }

    private void Update()
    {
        if (spawnRate < 1f) spawnRate += Time.deltaTime * additionPerSecond1;
        else spawnRate += Time.deltaTime * additionPerSecond2;
        if (spawnCooldown <= 0f)
        {
            SpawnEnemy();
            spawnCooldown = 1f / spawnRate;
        }
        else
        {
            spawnCooldown -= Time.deltaTime;
        }

        if (energyAdditionCooldown <= 0f)
        {
            UpdateEnergyPoints();
            energyAdditionCooldown = 1f;
        }
        else
        {
            energyAdditionCooldown -= Time.deltaTime;
        }
        UpdateIncomeText();
    }
}
