using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;
    private GameObject enemy = null;

    GameObject reference = null;

    int matrixX;
    int matrixY;

    public bool attack = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy = collision.gameObject;
        attack = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy = null;
        attack = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void OnMouseDown()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        if (attack)
        {
            Destroy(enemy);
        }

        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<ChessMan>().GetXBoard(), reference.GetComponent<ChessMan>().GetYBoard());

        reference.GetComponent<ChessMan>().SetXBoard(matrixX);
        reference.GetComponent<ChessMan>().SetYBoard(matrixY);
        reference.GetComponent<ChessMan>().SetCoords();

        controller.GetComponent<Game>().SetPosition(reference);

        reference.GetComponent<ChessMan>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference() { return reference; }
}