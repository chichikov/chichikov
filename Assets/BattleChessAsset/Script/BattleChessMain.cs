using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class BattleChessMain : MonoBehaviour {		
	
	// chess piece prefab reference
	public Transform[] aWholePiece;				
	
	// particle effect
	public ParticleSystem selectPiecePSystemRef;		
	// movable particle effect
	public ParticleSystem movablePiecePSystemRef;
	
	// chess board
	ChessBoard board;
	
	// chess engine management
	ChessEngineManager chessEngineMgr;	
	
	
	
	// Use this for initialization
	void Start() {				
		
		board = new ChessBoard();
		board.Init( this, aWholePiece, selectPiecePSystemRef, movablePiecePSystemRef );		
		
		// chess engine init/start
		chessEngineMgr = new ChessEngineManager();	
		
		StartCoroutine( chessEngineMgr.Start() );		
	}
	
	// Update is called once per frame
	void Update() {		
		
		// process engine command respond
		ProcessEngineCommand();	
		
		// input
		// piece selection
		if( Input.GetMouseButtonDown(0) ) {
			
			// collision check
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        
			if( Physics.Raycast( ray, out hitInfo, 1000 ) ) {
				
				// collision to piece
				if( hitInfo.collider.gameObject.tag != "Board" ) {				
					
					Vector3 vPos = hitInfo.collider.gameObject.transform.position;
					Quaternion rot = hitInfo.collider.gameObject.transform.rotation;
					
					board.SelectPiece( hitInfo.collider.gameObject, vPos, rot );													
				}
				// collision to board
				else {					
					
					// move to blank position			
					Vector3 vPos = hitInfo.point;					
					if( board.MoveTo( vPos ) )
					{							
						string strMoveCmd = board.GetCurrMoveCommand();
						
						UnityEngine.Debug.Log( strMoveCmd );
						
						chessEngineMgr.Send( strMoveCmd );							
					}
					
					board.SelectPiece( null, Vector3.zero, Quaternion.identity );										
				}
			}
			// extracollision
			else {				
				
				board.SelectPiece( null, Vector3.zero, Quaternion.identity );								
			}
			
			board.UpdateCurrMoveable();
		}	
	}
	
	void OnDestroy () {   
		
		// chess engine end
		chessEngineMgr.End();
		
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
				chessEngineMgr.SetConfigCommand( command.CmdData );
				
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
		
		bool bValidCmd = !cmd.CmdData.InvalidCmd;		
		if( bValidCmd ) {
			
			string strCmd = cmd.CmdData.StrCmd;
			
			switch( strCmd ) {
					
				case "id":		
					return ExcuteIdCommand( cmd.CmdData );						
						
				case "uciok":	
					return ExcuteUciOkCommand( cmd.CmdData );				
					
				case "readyok":		
					return ExcuteReadyOkCommand( cmd.CmdData );							
				
				case "copyprotection":
					return ExcuteCopyProtectionCommand( cmd.CmdData );						
				
				case "registration":
					return ExcuteRegistrationCommand( cmd.CmdData );							
					
				case "option":
					return ExcuteOptionCommand( cmd.CmdData );							
				
				case "info":
					return ExcuteInfoCommand( cmd.CmdData );											
				
				case "bestmove":
					return ExcuteBestMoveCommand( cmd.CmdData );							
					
				default:								
					return false;					
			} // switch	
			
			//return true;
		}	
		
		return false;
	}	
	
}
