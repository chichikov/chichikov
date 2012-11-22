using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {	
	
	// chess engine management
	private ChessEngineManager chessEngineMgr;	
	
	// chess piece prefab reference
	public Transform[] aWholePiece;		
	
	// board, 8 x 8, pile x rank
	private ChessBoardData.ChessPiece[,] aBoard;
	
	// board material
	private Material matBoard1;	
	private Material matBoard2;		
	
	// particle effect
	public ParticleSystem selectPiecePSystemPref;	
	private ParticleSystem selectPiecePSystem;
	private GameObject selectPiecePSObj;
	
	
	
	// Use this for initialization
	void Start () {				
		
		// init piece
		
		// init board
		aBoard = new ChessBoardData.ChessPiece[ChessBoardData.nNumPile,ChessBoardData.nNumRank];
		for( int i=0; i<ChessBoardData.nNumPile; i++ ){
			for( int j=0; j<ChessBoardData.nNumRank; j++ ){
				if( ChessBoardData.aStartPiecePos[i,j] == ChessBoardData.nNone_Piece ) {
					
					aBoard[i,j].SetPiece( null, ChessBoardData.PlayerSide.e_NoneSide );										
				}
				else
				{
					Vector3 currPos = new Vector3( j - 3.5f, 0.0f, i - 3.5f );					
					
					Transform currTransform = aWholePiece[ChessBoardData.aStartPiecePos[i,j]];
					Transform currPieceObject = Instantiate( currTransform, currPos, currTransform.rotation ) as Transform;
					
					
					if( i == 0 || i == 1 ) {
						
						aBoard[i,j].SetPiece( currPieceObject.gameObject, ChessBoardData.PlayerSide.e_White );												
					}
					else if( i == 6 || i == 7 ) {
						
						aBoard[i,j].SetPiece( currPieceObject.gameObject, ChessBoardData.PlayerSide.e_Black );						
					}
					
				}
			}		
		}
		
		
		// piece coloar
		SetWhiteSidePieceColor( Color.white );
		SetBlackSidePieceColor( Color.white );
		
		
		// board material
		if( renderer.materials.Length == 2 ) {
			
			matBoard1 = renderer.materials[0];
			matBoard2 = renderer.materials[1];			
			
			Color rgbaWhiteBoard, rgbaBlackBoard;
			rgbaWhiteBoard = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			rgbaBlackBoard = new Color( 0.039f, 0.34f, 0.22f, 1.0f );
			
			SetWhiteSideBoardColor( rgbaWhiteBoard );
			SetBlackSideBoardColor( rgbaBlackBoard );
		}	
		
		// particle effect
		selectPiecePSystem = Instantiate( selectPiecePSystemPref, Vector3.zero, Quaternion.identity ) as ParticleSystem;		
		selectPiecePSObj = selectPiecePSystem.gameObject;
		selectPiecePSystem.Stop();
		
		// chess engine init/start
		chessEngineMgr = new ChessEngineManager();	
		
		StartCoroutine( chessEngineMgr.Start() );		
	}
	
	// Update is called once per frame
	void Update () {		
		
		// process engine command respond
		ProcessEngineCommand();	
		
		// input
		// piece selection
		if( Input.GetMouseButton(0) ) {
			
			// collision check
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        
			if( Physics.Raycast( ray, out hitInfo, 1000 ) ) {
				
				if( hitInfo.collider.gameObject.tag != "Board" ) {
					//hitInfo.collider.gameObject.
					Vector3 vPos = hitInfo.collider.gameObject.transform.position;
					Quaternion rot = hitInfo.collider.gameObject.transform.rotation;
					selectPiecePSObj.transform.position = vPos;
					selectPiecePSObj.transform.rotation = rot;
					selectPiecePSystem.Play();				
				}
				else {
					selectPiecePSystem.Stop();
				}
			}
			else {
				selectPiecePSystem.Stop();
			}
		}	
	}
	
	void OnDestroy () {   
		
		// chess engine end
		chessEngineMgr.End();
		
	}	
	
	
	
	
	
	
	// SetBoardColor
	void SetWhiteSideBoardColor( Color rgbaWhite ) {
		
		if( matBoard1 != null ) {
			
			matBoard1.SetColor( "_Color", rgbaWhite );					
		}
	}
	
	void SetBlackSideBoardColor( Color rgbaBlack ) {
		
		if( matBoard2 != null ) {			
			
			matBoard2.SetColor( "_Color", rgbaBlack );			
		}
	}
	
	void SetWhiteSidePieceColor( Color rgbaWhite ) {
		
		for( int i=0; i<ChessBoardData.nNumPile; i++ ) {
			for( int j=0; j<ChessBoardData.nNumRank; j++ ) {
				
				if( aBoard[i,j].gameObject != null ) {
					
					if( aBoard[i,j].playerSide == ChessBoardData.PlayerSide.e_White )
						aBoard[i,j].gameObject.renderer.material.SetColor( "_Color", rgbaWhite );											
				}
			}
		}		
	}
	
	void SetBlackSidePieceColor( Color rgbaBlack ) {
		
		for( int i=0; i<ChessBoardData.nNumPile; i++ ) {
			for( int j=0; j<ChessBoardData.nNumRank; j++ ) {
				
				if( aBoard[i,j].gameObject != null ) {
					
					if( aBoard[i,j].playerSide == ChessBoardData.PlayerSide.e_Black )
						aBoard[i,j].gameObject.renderer.material.SetColor( "_Color", rgbaBlack );	
				}
			}
		}	
	}

	

	
	
	
	
	// process engine command respond
	void ProcessEngineCommand() {
		// read one line
		string strCurCommandLine = chessEngineMgr.PopReceivedQueue();
		while( strCurCommandLine != null ) {
			
			// process one engine respond
			EngineToGuiCommand command = chessEngineMgr.ParseCommand( strCurCommandLine );
			if( command != null ) {				
				
				//command.PrintCommand();
				chessEngineMgr.SetConfigCommand( command.commandData );
				
				ExcuteEngineCommand( command );								
			}
			
			strCurCommandLine = chessEngineMgr.PopReceivedQueue();
		}
		
	}
	
	
	
	// 
	bool ExcuteIdCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ExcuteUciOkCommand( CommandBase.CommandData cmdData ) {
		
		// send setoption command!!!
		
		// send isready command	
		chessEngineMgr.Send( "isready" );		
		
		return false;
	}
	
	bool ExcuteReadyOkCommand( CommandBase.CommandData cmdData ) {
		
		// send isready command	
		chessEngineMgr.Send( "ucinewgame" );
		
		// test move
		chessEngineMgr.Send( "position startpos moves e2e4" );
		
		/*
		if( aBoard[1,4] != null ) {
			Vector3 vNewPos = new Vector3( aBoard[1,4].transform.position.x, 
				aBoard[1,4].transform.position.y, 0.5f );
			
			aBoard[1,4].transform.position = vNewPos;
		}
		*/
		
		chessEngineMgr.Send( "go" );	
		
		return false;
	}
	
	bool ExcuteCopyProtectionCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ExcuteRegistrationCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ExcuteOptionCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ExcuteInfoCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ExcuteBestMoveCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}	
	
	// delegate function for engine command
	public bool ExcuteEngineCommand( EngineToGuiCommand cmd ) {		
		
		if( cmd == null )
			return false;
		
		bool bValidCmd = !cmd.commandData.bInvalidCmd;		
		if( bValidCmd ) {
			
			string strCmd = cmd.commandData.strCmd;
			
			switch( strCmd ) {
					
				case "id":		
					return ExcuteIdCommand( cmd.commandData );						
						
				case "uciok":	
					return ExcuteUciOkCommand( cmd.commandData );				
					
				case "readyok":		
					return ExcuteReadyOkCommand( cmd.commandData );							
				
				case "copyprotection":
					return ExcuteCopyProtectionCommand( cmd.commandData );						
				
				case "registration":
					return ExcuteRegistrationCommand( cmd.commandData );							
					
				case "option":
					return ExcuteOptionCommand( cmd.commandData );							
				
				case "info":
					return ExcuteInfoCommand( cmd.commandData );											
				
				case "bestmove":
					return ExcuteBestMoveCommand( cmd.commandData );							
					
				default:								
					return false;					
			} // switch	
			
			//return true;
		}	
		
		return false;
	}	
	
}
