using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Game : MonoBehaviour
{
    [SerializeField] GameObject chessPiece;
    [SerializeField] GameObject tree;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] private UnityEngine.SpriteRenderer boardSprite;
    [SerializeField] private float waveInterval = 10f;

    private float waveTimer = 0f;
    //пока не используется - логика рандомного спавна в методе SpawnEnemyWave
    [SerializeField] private int[] spawnRows = { 0, 2, 4, 6, 7 };
    //[SerializeField] private float enemyMoveInterval = 0.25f;
    //private float enemyMoveTimer = 0f;

    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerWhite = new GameObject[16];
    private GameObject[] trees = new GameObject[14];

    private string[] treesName = new string[14];

    //private string currentPlayer = "white";

    private bool gameOver = false;
    void Start()
    {
       
        playerWhite = new GameObject[]
        {
            Create("white_rook", 0,0), Create("white_knight", 0,1),
            Create("white_bishop", 0,2), Create("white_queen", 0,3),
            Create("white_king", 0,4), Create("white_bishop", 0,5),
            Create("white_knight", 0,6), Create("white_rook", 0,7),
            Create("white_pawn", 1, 0), Create("white_pawn", 1, 1),
            Create("white_pawn", 1, 2), Create("white_pawn", 1, 3),
            Create("white_pawn", 1, 4), Create("white_pawn", 1, 5),
            Create("white_pawn", 1, 6), Create("white_pawn", 1, 7)
        };

        string RandomIntToStr = Mathf.RoundToInt(UnityEngine.Random.RandomRange(1000000, 9999999)).ToString() + Mathf.RoundToInt(UnityEngine.Random.RandomRange(1000000, 9999999)).ToString(); ;

        for (int i = 0; i < treesName.Length; i++)
        {
            Debug.Log(i.ToString() + " " + RandomIntToStr);
            treesName[i] = "tree1_" + RandomIntToStr[i];
            Debug.Log(i.ToString() + " " + RandomIntToStr);
        }

        trees = new GameObject[]
        {
            CreateTree(treesName[0], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 1,9),
            CreateTree(treesName[1], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 2,8),
            CreateTree(treesName[2], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 3,7),
            CreateTree(treesName[3], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 3,7),
            CreateTree(treesName[4], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 4,6),
            CreateTree(treesName[5], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 5,5),
            CreateTree(treesName[6], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 6,4),
            CreateTree(treesName[7], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 7,3),
            CreateTree(treesName[8], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 8,2),
            CreateTree(treesName[9], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 8,2),
            CreateTree(treesName[10], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 9,1),
            CreateTree(treesName[11], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 9,1),
            CreateTree(treesName[12], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 0,10),
            CreateTree(treesName[13], Mathf.RoundToInt(UnityEngine.Random.Range(9,13)), 0,10),
        };

        for (int i = 0; i < playerWhite.Length; i++)
        {
            SetPosition(playerWhite[i]);
        }
        //Instantiate(chessPiece, new Vector3(0,0,-1), Quaternion.identity);

        GameObject enemy = CreateEnemy("enemy_1", 7, 6);
        SetPosition(enemy);
    }

    public GameObject CreateEnemy(string name, int x, int y)
    {
        var obj = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
        var enemy = obj.GetComponent<Enemy>();
        enemy.SetXBoard(x);
        enemy.SetYBoard(y);
        enemy.SetBoardSprite(boardSprite); // ключевая строка
        enemy.Activate();
        SetPosition(obj);
        return obj;
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chessPiece, new Vector3(0, 0, -1), Quaternion.identity);
        ChessMan cm = obj.GetComponent<ChessMan>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        return obj;
    }

    public GameObject CreateTree(string name, int x, int y, int layer)
    {
        GameObject obj = Instantiate(tree, new Vector3(0, 0, -1), Quaternion.identity);
        Tree tr = obj.GetComponent<Tree>();
        Transform scaleTree = obj.GetComponent<Transform>();
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = layer;
        float scaleF = UnityEngine.Random.Range(4, 6);
        scaleTree.localScale = new Vector3(scaleF, scaleF, 1);
        tr.name = name;
        tr.SetXBoard(x);
        tr.SetYBoard(y);
        tr.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        if (obj == null) return;

        int x = -1;
        int y = -1;

        if (obj.TryGetComponent<ChessMan>(out ChessMan cm))
        {
            x = cm.GetXBoard();
            y = cm.GetYBoard();
        }
        else if (obj.TryGetComponent<Enemy>(out Enemy en))
        {
            x = en.GetXBoard();
            y = en.GetYBoard();
        }
        else if (obj.TryGetComponent<Tree>(out Tree tr))
        {
            x = tr.GetXBoard();
            y = tr.GetYBoard();
        }
        else
        {
            Debug.LogWarning("SetPosition called with GameObject without recognized board coordinate component");
            return;
        }

        if (x >= 0 && y >= 0 && x < positions.GetLength(0) && y < positions.GetLength(1))
        {
            positions[x, y] = obj;
        }
    }


    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(float x, float y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1))
        {
            return false;
        }
        return true;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void Update()
    {
        if (gameOver)
        {
            // Проверяем нажатие левой кнопки мыши после окончания игры
            if (Input.GetMouseButtonDown(0))
            {
                // Перезагружаем текущую сцену
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        waveTimer += Time.deltaTime;
        if (waveTimer >= waveInterval)
        {
            SpawnEnemyWave();
            waveTimer = 0f;
        }

        // enemyMoveTimer += Time.deltaTime;
        // if (enemyMoveTimer >= enemyMoveInterval)
        // {
        //     MoveAllEnemiesForward();
        //     enemyMoveTimer = 0f;
        // }
    }

    public void Win()
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("Result").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.FindGameObjectWithTag("Result").GetComponent<TextMeshProUGUI>().text = "You've win";

        GameObject.FindGameObjectWithTag("Result").GetComponent<TextMeshProUGUI>().enabled = true;
    }

    public void Lose()
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("Result").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.FindGameObjectWithTag("Result").GetComponent<TextMeshProUGUI>().text = "You've lost";

        GameObject.FindGameObjectWithTag("Result").GetComponent<TextMeshProUGUI>().enabled = true;
    }

    void SpawnEnemyWave()
    {
        int xSpawn = positions.GetLength(0) - 1; // правая колонка
        int enemyCount = 3;
        int attemptsPerEnemy = 8;

        for (int i = 0; i < enemyCount; i++)
        {
            bool spawned = false;
            for (int a = 0; a < attemptsPerEnemy; a++)
            {
                int randomRow = Random.Range(0, positions.GetLength(1));
                if (GetPosition(xSpawn, randomRow) == null)
                {
                    GameObject enemy = CreateEnemy("enemy", xSpawn, randomRow);
                    SetPosition(enemy);
                    spawned = true;
                    break;
                }
            }
            // если не получилось — пропускаем этого врага волны
        }
    }

}
