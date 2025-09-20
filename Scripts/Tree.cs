using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject controller;

    private int xBoard = -1;
    private int yBoard = -1;

    public Sprite tree1_0, tree1_1, tree1_2, tree1_3, tree1_4, tree1_5, tree1_6, tree1_7, tree1_8, tree1_9, tree1_10, tree1_11, tree1_12, tree1_13, tree1_14;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case "tree1_0": this.GetComponent<SpriteRenderer>().sprite = tree1_0; break;
            case "tree1_1": this.GetComponent<SpriteRenderer>().sprite = tree1_1; break;
            case "tree1_2": this.GetComponent<SpriteRenderer>().sprite = tree1_2; break;
            case "tree1_3": this.GetComponent<SpriteRenderer>().sprite = tree1_3; break;
            case "tree1_4": this.GetComponent<SpriteRenderer>().sprite = tree1_4; break;
            case "tree1_5": this.GetComponent<SpriteRenderer>().sprite = tree1_5; break;
            case "tree1_6": this.GetComponent<SpriteRenderer>().sprite = tree1_6; break;
            case "tree1_7": this.GetComponent<SpriteRenderer>().sprite = tree1_7; break;
            case "tree1_8": this.GetComponent<SpriteRenderer>().sprite = tree1_8; break;
            case "tree1_9": this.GetComponent<SpriteRenderer>().sprite = tree1_9; break;
            case "tree1_10": this.GetComponent<SpriteRenderer>().sprite = tree1_10; break;
            case "tree1_11": this.GetComponent<SpriteRenderer>().sprite = tree1_11; break;
            case "tree1_12": this.GetComponent<SpriteRenderer>().sprite = tree1_12; break;
            case "tree1_13": this.GetComponent<SpriteRenderer>().sprite = tree1_13; break;
            case "tree1_14": this.GetComponent<SpriteRenderer>().sprite = tree1_14; break;
        }


    }
    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 1.12f;
        y *= 1.12f;

        x += -3.90f;
        y += -3.90f;

        this.transform.position = new Vector3(x, y, -1f);
    }

    public int GetXBoard() { return xBoard; }
    public int GetYBoard() { return yBoard; }

    public void SetXBoard(int x) { xBoard = x; }
    public void SetYBoard(int y) { yBoard = y; }
}
