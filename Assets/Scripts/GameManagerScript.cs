using UnityEngine;

public class GameManagerScript : MonoBehaviour {

	public int gridWidth = 8;
	public int gridHeight = 8;
	public float tokenSize = 1;

	private MatchManagerScript matchManager;
	private InputManagerScript inputManager;
	private RepopulateScript repopulateManager;
	private MoveTokensScript moveTokenManager;

	public GameObject grid;
	public  GameObject[,] gridArray;
	private Object[] tokenTypes;
	private GameObject _selected;

	public virtual void Start () {
		tokenTypes = Resources.LoadAll("Tokens/");
		gridArray = new GameObject[gridWidth, gridHeight];
		MakeGrid();
		
		matchManager = GetComponent<MatchManagerScript>();
		Debug.Assert(matchManager != null, "Attach a match manager to this object.");
		
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
	}

	public virtual void Update(){
		if(!GridHasEmpty()){
			if(matchManager.GridHasMatch()){
				matchManager.RemoveMatches();
			} else {
				inputManager.SelectToken();
			}
		} 
		else {
			if(!moveTokenManager.move){
				moveTokenManager.SetupTokenMove();
			}
			if(!moveTokenManager.MoveTokensToFillEmptySpaces()){
				repopulateManager.AddNewTokensToRepopulateGrid();
			}
		}
	}

	void MakeGrid() {
		grid = new GameObject("TokenGrid");
		
		for(int x = 0; x < gridWidth; x++)
		{
			for(int y = 0; y < gridHeight; y++)
			{
				AddTokenToPosInGrid(x, y, grid);
			}
		}
	}

	protected virtual bool GridHasEmpty(){
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight ; y++)
			{
				if (gridArray[x, y] == null)
				{
					return true;
				}
			}
		}

		return false;
	}

	public Vector2 GetPositionOfTokenInGrid(GameObject token){
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(gridArray[x, y] == token){
					return(new Vector2(x, y));
				}
			}
		}
		return new Vector2();
	}
		
	public Vector2 GetWorldPositionFromGridPosition(int x, int y){
		return new Vector2(
			(x - gridWidth/2) * tokenSize,
			(y - gridHeight/2) * tokenSize);
	}

	public void AddTokenToPosInGrid(int x, int y, GameObject parent){
		Vector3 position = GetWorldPositionFromGridPosition(x, y);
		GameObject token = 
			Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)], 
			            position, 
			            Quaternion.identity) as GameObject;
		token.transform.parent = parent.transform;
		gridArray[x, y] = token;
	}
}
