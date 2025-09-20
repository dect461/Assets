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

    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerWhite = new GameObject[16];
    private GameObject[] trees = new GameObject[14];
    private string[] treesName = new string[14];

    private string currentPlayer = "white";

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

        string RandomIntToStr = Mathf.RoundToInt(UnityEngine.Random.RandomRange(1000000, 9999999)).ToString() + Mathf.RoundToInt(UnityEngine.Random.RandomRange(1000000, 9999999)).ToString();;

        for (int  i = 0; i < treesName.Length; i++)
        {
            Debug.Log(i.ToString() + " " + RandomIntToStr);
            treesName[i] = "tree1_" + RandomIntToStr[i];
            Debug.Log(i.ToString() + " " + RandomIntToStr);
        }
        

        //Подумать как улучшить систему со слоями, пока обойдемся так

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
        
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chessPiece, new Vector3(0,0,-1), Quaternion.identity);
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
        ChessMan cm = obj.GetComponent<ChessMan>();

        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x,y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x,y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >=positions.GetLength(0) || y >= positions.GetLength(1) )
        {
            return false;
        }
        return true;
    }
    // Можно будет убрать потом
    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        } 
        else
        {
            currentPlayer = "white";
        }
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        { 
            gameOver = false;

            SceneManager.LoadScene("Game");
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("Winner").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.FindGameObjectWithTag("Winner").GetComponent<TextMeshProUGUI>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("Restart").GetComponent<TextMeshProUGUI>().enabled = true;
    }
}
