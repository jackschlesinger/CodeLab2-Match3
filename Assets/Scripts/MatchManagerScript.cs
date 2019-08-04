using UnityEngine;

public class MatchManagerScript : MonoBehaviour {
	
	private GameManagerScript _gameManager;

	public virtual void Start () {
		_gameManager = GetComponent<GameManagerScript>();
	}

	public virtual bool GridHasMatch(){
		bool match = false;
		
		for(int x = 0; x < _gameManager.gridWidth; x++){
			for(int y = 0; y < _gameManager.gridHeight ; y++){
				if(x < _gameManager.gridWidth - 2){
					match = match || GridHasHorizontalMatch(x, y);
				}
			}
		}
		
		return match;
	}

	public bool GridHasHorizontalMatch(int x, int y){
		GameObject token1 = _gameManager.gridArray[x + 0, y];
		GameObject token2 = _gameManager.gridArray[x + 1, y];
		GameObject token3 = _gameManager.gridArray[x + 2, y];
		
		if(token1 != null && token2 != null && token3 != null){
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		} else {
			return false;
		}
	}

	private int _GetHorizontalMatchLength(int x, int y){
		int matchLength = 1;
		
		GameObject first = _gameManager.gridArray[x, y];

		if(first != null){
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
			
			for(int i = x + 1; i < _gameManager.gridWidth; i++){
				GameObject other = _gameManager.gridArray[i, y];

				if(other != null){
					SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

					if(sr1.sprite == sr2.sprite){
						matchLength++;
					} else {
						break;
					}
				} else {
					break;
				}
			}
		}
		
		return matchLength;
	}

	public virtual int RemoveMatches(){
		int numRemoved = 0;

		for(int x = 0; x < _gameManager.gridWidth; x++){
			for(int y = 0; y < _gameManager.gridHeight ; y++){
				if(x < _gameManager.gridWidth - 2){

					int horizonMatchLength = _GetHorizontalMatchLength(x, y);

					if(horizonMatchLength > 2){

						for(int i = x; i < x + horizonMatchLength; i++){
							GameObject token = _gameManager.gridArray[i, y]; 
							Destroy(token);

							_gameManager.gridArray[i, y] = null;
							numRemoved++;
						}
					}
				}
			}
		}
		
		return numRemoved;
	}
}
