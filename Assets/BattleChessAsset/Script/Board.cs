using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {	
	
	// chess engine management
	private ChessEngineManager ChessEngineMgr;	
	
	// chess piece prefab reference
	public Transform[] aWholePiece;		
	
	// board, 8 x 8, pile x rank
	private GameObject[,] aBoard;	
	
	
	
	// Use this for initialization
	void Start () {	
		
		// chess engine init/start
		ChessEngineMgr = new ChessEngineManager();		
		StartCoroutine( ChessEngineMgr.Start() );
		
		// init piece
		
		// init board
		aBoard = new GameObject[ChessBoardData.nNumPile,ChessBoardData.nNumRank];
		for( int i=0; i<ChessBoardData.nNumPile; i++ ){
			for( int j=0; j<ChessBoardData.nNumRank; j++ ){
				if( ChessBoardData.aStartPiecePos[i,j] == ChessBoardData.nNone_Piece )
					aBoard[i,j] = null;
				else
				{
					Vector3 currPos = new Vector3( 0.5f + j * 1.025f - 1.025f * 4.0f, 0.01f, 0.5f + i * 1.025f - 1.025f * 4.0f );					
					
					Transform currTransform = aWholePiece[ChessBoardData.aStartPiecePos[i,j]];
					Transform currPieceObject = Instantiate( currTransform, currPos, currTransform.rotation ) as Transform;
					aBoard[i,j] = currPieceObject.gameObject;
				}
			}		
		}	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDestroy () {   
		
		// chess engine end
		ChessEngineMgr.End();
		
	}
}
