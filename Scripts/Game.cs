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
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];
    private GameObject[] trees = new GameObject[10];
    private string[] treesName = new string[10];

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

        playerBlack = new GameObject[]
        {
            Create("black_rook", 7,0), Create("black_knight", 7,1),
            Create("black_bishop", 7,2), Create("black_queen", 7,3),
            Create("black_king", 7,4), Create("black_bishop", 7,5),
            Create("black_knight", 7,6), Create("black_rook", 7,7),
            Create("black_pawn", 6, 0), Create("black_pawn", 6, 1),
            Create("black_pawn", 6, 2), Create("black_pawn", 6, 3),
            Create("black_pawn", 6, 4), Create("black_pawn", 6, 5),
            Create("black_pawn", 6, 6), Create("black_pawn", 6, 7)
        };

        for (int  i = 0; i < treesName.Length; i++)
        {
            treesName[i] = "tree1_" + Mathf.RoundToInt(UnityEngine.Random.Range(0, 11)).ToString(); 
        }
        
        trees = new GameObject[]
        {
            CreateTree(treesName[0], Mathf.RoundToInt(UnityEngine.Random.Range(8,12)), 1),
            CreateTree(treesName[1], Mathf.RoundToInt(UnityEngine.Random.Range(8,12)), 2),
            CreateTree(treesName[2], Mathf.RoundToInt(UnityEngine.Random.Range(8,12)), 3),
            CreateTree(treesName[3], Mathf.RoundToInt(UnityEngine.Random.Range(8,12)), 3),
            CreateTree(treesName[4], Mathf.RoundToInt(UnityEngine.Random.Range(8,12)), 4),
            CreateTree(treesName[5], Mathf.RoundToInt(UnityEngine.Random.Range(8,12)), 5),
            CreateTree(treesName[6], Mathf.RoundToInt(UnityEngine.Random.Range(8,12)), 6),
            CreateTree(treesName[7], Mathf.RoundToInt(UnityEngine.Random.Range(8,12)), 7),
            CreateTree(treesName[8], Mathf.RoundToInt(UnityEngine.Random.Range(8,12)), 8),
            CreateTree(treesName[9], Mathf.RoundToInt(UnityEngine.Random.Range(8,12)), 8),
        };

        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
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

    public GameObject CreateTree(string name, int x, int y)
    {
        GameObject obj = Instantiate(tree, new Vector3(0, 0, -1), Quaternion.identity);
        Tree tr = obj.GetComponent<Tree>();
        Transform scaleTree = obj.GetComponent<Transform>();
        float scaleF = UnityEngine.Random.Range(3, 6);
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
