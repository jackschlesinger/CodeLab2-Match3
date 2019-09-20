using UnityEngine;

public class MatchManagerScript : MonoBehaviour {
	
	private GameManagerScript _gameManager;

	void Start () {
		_gameManager = GetComponent<GameManagerScript>();
	}

	public bool GridHasMatch(){
		for(var x = 0; x < _gameManager.gridWidth; x++){
			for(var y = 0; y < _gameManager.gridHeight; y++){
				if (x < _gameManager.gridWidth - 2 && GridHasHorizontalMatch(x, y)) return true;
				if (y < _gameManager.gridHeight - 2 && GridHasVerticalMatch(x, y)) return true;
			}
		}
		
		return false;
	}

	private bool GridHasHorizontalMatch(int x, int y){
		var token1 = _gameManager.gridArray[x + 0, y];
		var token2 = _gameManager.gridArray[x + 1, y];
		var token3 = _gameManager.gridArray[x + 2, y];

		if (ReferenceEquals(token1, null) || ReferenceEquals(token2, null) || ReferenceEquals(token3, null)) return false;
		
		return (token1.name == token2.name && token2.name == token3.name);
	}

	private bool GridHasVerticalMatch(int x, int y)
	{
		var token1 = _gameManager.gridArray[x, y];
		var token2 = _gameManager.gridArray[x, y + 1];
		var token3 = _gameManager.gridArray[x, y + 2];

		if (ReferenceEquals(token1, null) || ReferenceEquals(token2, null) || ReferenceEquals(token3, null)) return false;
		
		return (token1.name == token2.name && token2.name == token3.name);
	}

	private int _GetHorizontalMatchLength(int x, int y){
		GameObject first = _gameManager.gridArray[x, y];
		if (ReferenceEquals(first, null)) return 0;

		var matchLength = 1;

			
		for(var i = x + 1; i < _gameManager.gridWidth; i++){
			var other = _gameManager.gridArray[i, y];

			if (ReferenceEquals(other, null)) break;
			if(first.name == other.name){
				matchLength++;
			} 
			else {
				break;
			}
		}

		return matchLength;
	}
	
	private int _GetVerticalMatchLength(int x, int y){
		var first = _gameManager.gridArray[x, y];
		
		if (ReferenceEquals(first, null)) return 0;
		
		var matchLength = 1;

		for(var currentY = y + 1; currentY < _gameManager.gridHeight; currentY++){
			var other = _gameManager.gridArray[x, currentY];

			if (ReferenceEquals(other, null)) break;

			if(first.name == other.name) {
				matchLength++;
			} 
			else {
				break;
			}
		}

		return matchLength;
	}

	public virtual int RemoveMatches(){
		var numRemoved = 0;

		for(var x = 0; x < _gameManager.gridWidth; x++)
		{
			for(var y = 0; y < _gameManager.gridHeight ; y++)
			{
				var horizonMatchLength = 0;
				var vertMatchLength = 0;

				if (x < _gameManager.gridWidth - 2) horizonMatchLength = _GetHorizontalMatchLength(x, y);
				if (y < _gameManager.gridHeight - 2) vertMatchLength = _GetVerticalMatchLength(x, y);


				if(horizonMatchLength > 2){

					for(var currentX = x; currentX < x + horizonMatchLength; currentX++)
					{
						var token = _gameManager.gridArray[currentX, y]; 
						if (!ReferenceEquals(token, null)) Destroy(token);

						_gameManager.gridArray[currentX, y] = null;
						numRemoved++;
					}
				}
				

				if (vertMatchLength > 2)
				{
					for(var currentY = y; currentY < y + vertMatchLength; currentY++){
						var token = _gameManager.gridArray[x, currentY]; 
						if (!ReferenceEquals(token, null)) Destroy(token);

						_gameManager.gridArray[x, currentY] = null;
						numRemoved++;
					}
				}
			}
		}
		
		return numRemoved;
	}
}
