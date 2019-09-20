using System;
using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour {
	private GameManagerScript _gameManager;
	private MoveTokensScript _moveManager;
	private GameObject _selected = null;
	public Camera camera;

	public virtual void Start () {
		_moveManager = GetComponent<MoveTokensScript>();
		_gameManager = GetComponent<GameManagerScript>();
	}

	public virtual void SelectToken(){
		if (!Input.GetMouseButtonDown(0)) return;
		
		var mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
			
		var overlapPoint = Physics2D.OverlapPoint(mousePos);

		if (ReferenceEquals(overlapPoint, null)) return;
		
		if(ReferenceEquals(_selected, null))
		{
			_selected = overlapPoint.gameObject;
		} 
		else 
		{
			var pos1 = _gameManager.GetPositionOfTokenInGrid(_selected);
			var pos2 = _gameManager.GetPositionOfTokenInGrid(overlapPoint.gameObject);

			if(Math.Abs(Mathf.Abs((pos1.x - pos2.x) + (pos1.y - pos2.y)) - 1) < 0.01f){
				_moveManager.SetupTokenExchange(_selected, pos1, overlapPoint.gameObject, pos2, true);
			}

			_selected = null;
		}
	}
}
