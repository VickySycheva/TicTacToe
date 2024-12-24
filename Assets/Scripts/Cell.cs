using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool CanPlace => CellObjectType == PlayerType.Null;
    public Vector2Int Index => index;

    public GameObject CellObject { get; private set; }
    public PlayerType CellObjectType { get; private set; }

    [SerializeField] private GameObject crossPrefab;
    [SerializeField] private GameObject naughtPrefab;
    [SerializeField] private Vector2Int index;

    public void Init()
    {
        CellObjectType = PlayerType.Null;
    }

    public void InstCellObj(PlayerType playerType)
    {
        CellObjectType = playerType;
        CellObject = Instantiate (CellObjectType == PlayerType.Cross ? crossPrefab : naughtPrefab);
        CellObject.transform.parent = transform;
        CellObject.transform.localPosition = Vector3.zero;
        CellObject.transform.localScale = new Vector3 (0.8f, 2, 0.8f);
    }

    public void ClearCell()
    {
        Destroy(CellObject);
        CellObjectType = PlayerType.Null;
    }

}
