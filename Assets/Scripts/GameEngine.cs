using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEngine : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private TextMeshProUGUI tabToStart;
    [SerializeField]
    private TextMeshProUGUI recordText;
    [SerializeField]
    private TextMeshProUGUI resultText;

    private float textScaleSpeed = 5f;
    bool isVisibleTapToStart = true;

    public int Record { get; private set; } = 0;

    [SerializeField]
    private Button buttonSound;
    [SerializeField]
    private Button buttonMusic;
    [SerializeField]
    private Button buttonGenre;

    public bool isSound { get; private set; }
    private bool isMusic;
    public bool isGenre { get; private set; }

    public bool isGenreActivated { get; private set; } = false;
    private int countMusicClicks = 0;

    private Sprite spriteSoundOn;
    private Sprite spriteSoundOff;
    private Sprite spriteMusicOn;
    private Sprite spriteMusicOff;
    private Sprite spriteGenreOn;
    private Sprite spriteGenreOff;


    private void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        Time.timeScale = 1.0f;

        if (!File.Exists(Application.persistentDataPath + "/save.bin"))
        {
            SaveData data = new SaveData();
            data.record = 0;
            data.sound = true;
            data.music = true;
            data.genre = false;
            data.isGenreUnlocked = false;

            BinaryFormatter f = new BinaryFormatter();

            using (FileStream stream = File.Create(Application.persistentDataPath + "/save.bin"))
            {
                f.Serialize(stream, data);
            }
        }
        
        loadingScreen.SetActive(false);

        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = File.OpenRead(Application.persistentDataPath + "/save.bin"))
        {
            SaveData data = (SaveData)formatter.Deserialize(stream);
            recordText.text = "Top Score: " + data.record.ToString();
            isSound = data.sound;
            isMusic = data.music;
            isGenre = data.genre;
            Record = data.record;
            isGenreActivated = data.isGenreUnlocked;
            LoadSprites();
            CheckIcon();
        }

        StartCoroutine(ChangeTextSize());
    }

    private void LoadSprites()
    {
        spriteSoundOn = Resources.Load<Sprite>("UI/SoundOn");
        spriteSoundOff = Resources.Load<Sprite>("UI/SoundOff");
        spriteMusicOn = Resources.Load<Sprite>("UI/MusicOn");
        spriteMusicOff = Resources.Load<Sprite>("UI/MusicOff");
        spriteGenreOn = Resources.Load<Sprite>("UI/GenreOn");
        spriteGenreOff = Resources.Load<Sprite>("UI/GenreOff");
    }

    public void StartGame()
    {
        tabToStart.gameObject.SetActive(false);
        recordText.gameObject.SetActive(false);
        buttonMusic.gameObject.SetActive(false);
        buttonSound.gameObject.SetActive(false);
        buttonGenre.gameObject.SetActive(false);

        resultText.gameObject.SetActive(true);

        GameObject.Find("PlayerGroup").GetComponent<PlayerMovement>().isGameStarted = true;
    }

    private void CheckIcon()
    {
        if (isSound)
            buttonSound.image.sprite = spriteSoundOn;
        else
            buttonSound.image.sprite = spriteSoundOff;
        
        /*if (isMusic)
            buttonMusic.image.sprite = spriteMusicOn;
        else
            buttonMusic.image.sprite = spriteMusicOff;*/

        if (isGenreActivated)
        {
            buttonGenre.gameObject.SetActive(true);

            if (isGenre)
                buttonGenre.image.sprite = spriteGenreOn;
            else
                buttonGenre.image.sprite = spriteGenreOff;
        }
    }

    private IEnumerator ChangeTextSize()
    {
        float textSize = 35f;
        int koef = -1;

        while (isVisibleTapToStart) 
        {
            if(textSize >= 35) koef = -1;
            if(textSize <= 30) koef = 1;

            textSize += koef * textScaleSpeed * Time.deltaTime;

            tabToStart.fontSize = textSize;
            yield return new WaitForEndOfFrame();
        }

        tabToStart.enabled = false;
        recordText.enabled = false;
        resultText.enabled = true;
    }

    public void HideTapToStart()
    {
        isVisibleTapToStart = false;
    }

    public void ReloadLevel(int result)
    {
        loadingScreen.SetActive(true);

        SaveData data = new SaveData();

        if (Record < result)
            data.record = result;
        else
            data.record = Record;

        data.sound = isSound;
        data.music = isMusic;
        data.genre = isGenre;
        data.isGenreUnlocked = isGenreActivated;

        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = File.Create(Application.persistentDataPath + "/save.bin"))
        {
            formatter.Serialize(stream, data);
        }

        LevelGenerator.startZPosition = 0;
        Application.LoadLevel(Application.loadedLevel);

    }

    public void bMusic()
    {
        if (isMusic)
        {
            buttonMusic.image.sprite = spriteMusicOff;
        }
        else
        {
            buttonMusic.image.sprite = spriteMusicOn;
        }
        isMusic = !isMusic;
    }

    public void bSound()
    {
        if (isSound)
        {
            buttonSound.image.sprite = spriteSoundOff;
        }
        else
        {
            buttonSound.image.sprite = spriteSoundOn;
        }
        isSound = !isSound;

        if (++countMusicClicks == 10)
        {
            isGenreActivated = true;
            CheckIcon();
        }
    }

    public void bGenre() 
    {
        if (isGenre)
        {
            buttonGenre.image.sprite = spriteGenreOff;
        }
        else
        {
            buttonGenre.image.sprite = spriteGenreOn;
        }
        isGenre = !isGenre;
    }

    public void newBoxValue(int count)
    {
        resultText.text = count.ToString();
    }
}

[System.Serializable]
public class SaveData
{
    public int record;
    public bool genre;
    public bool sound;
    public bool music;
    public bool isGenreUnlocked;
}
