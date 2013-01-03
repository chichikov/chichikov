using UnityEngine;
using System.Collections;


//namespace BattleChess {	
	
public class ChessPiece {
	
	public GameObject gameObject;
	
	public PlayerSide playerSide;
	public PieceType pieceType;
	public PiecePlayerType piecePlayerType;		
	
	public ChessPosition position;		
	
	public bool bEnPassantCapture;
	
	public ParticleSystem movablePSystem;
	
	
	public ChessPiece() {
		
		gameObject = null;		
		playerSide = PlayerSide.e_NoneSide;
		pieceType = PieceType.e_None;
		piecePlayerType = PiecePlayerType.eNone_Piece;
		position.pos = BoardPosition.InvalidPosition;
		bEnPassantCapture = false;
	}	
			
	public ChessPiece( GameObject gameObject, PlayerSide playerSide, 
		PiecePlayerType piecePlayerType, int nPile, int nRank ) {
		
		this.gameObject = gameObject;
		this.playerSide = playerSide;
		this.pieceType = ChessUtility.GetPieceType( piecePlayerType );
		this.piecePlayerType = piecePlayerType;
		this.position.SetPosition( nRank, nPile );		
		
		this.bEnPassantCapture = false;
		
		if( this.gameObject != null )
			this.gameObject.transform.position = this.position.Get3DPosition();
	}	
	
	public void SetPiece( ChessPiece chessPiece ) {
		
		this.gameObject = chessPiece.gameObject;
		this.playerSide = chessPiece.playerSide;
		this.pieceType = chessPiece.pieceType;
		this.piecePlayerType = chessPiece.piecePlayerType;
		this.bEnPassantCapture = chessPiece.bEnPassantCapture;
				
		if( this.gameObject != null )
			this.gameObject.transform.position = this.position.Get3DPosition();		
	}	
	
	public void ClearPiece( bool bDestryGameObject = false ) {		
		
		if( bDestryGameObject && gameObject != null ) {
			
			MonoBehaviour.Destroy(gameObject);
		}
		
		gameObject = null;
		
		playerSide = PlayerSide.e_NoneSide;
		pieceType = PieceType.e_None;
		piecePlayerType = PiecePlayerType.eNone_Piece;						
		bEnPassantCapture = false;		
	}		
	
	public void CopyFrom( ChessPiece chessPiece ) {
		
		this.gameObject = chessPiece.gameObject;
		this.playerSide = chessPiece.playerSide;
		this.pieceType = chessPiece.pieceType;
		this.piecePlayerType = chessPiece.piecePlayerType;
		this.position = chessPiece.position;
		this.bEnPassantCapture = chessPiece.bEnPassantCapture;
	}
	
	
	
	public void SetMovableEffect( ParticleSystem movablePSystem ) {
		
		if( position.pos == BoardPosition.InvalidPosition )
			return;					
		
		this.movablePSystem = movablePSystem;							
		this.movablePSystem.Stop();
		
		int nRank = 0, nPile = 0;
		ChessPosition.GetPositionIndex( position.pos, ref nRank, ref nPile );			
		
		Vector3 pos = Vector3.zero;
		pos.x = nRank - 3.5f;
		pos.y = 0.01f;
		pos.z = nPile - 3.5f;
		Quaternion rot = Quaternion.identity;
		
		this.movablePSystem.gameObject.transform.position = pos;
		this.movablePSystem.gameObject.transform.rotation = rot;
		
		this.movablePSystem.gameObject.renderer.material.SetColor( "_Color", Color.blue );
	}
	
	public void ShowMovableEffect( bool bShow ) {
		
		if( movablePSystem == null )
			return;
		
		if( bShow ) {
			
			movablePSystem.renderer.enabled = true;
			movablePSystem.Play();				
		}
		else{
			
			movablePSystem.Stop();
			movablePSystem.renderer.enabled = false;
		}			
	}
	
	public bool IsBlank() {
		
		if( gameObject == null || piecePlayerType == PiecePlayerType.eNone_Piece )
			return true;
		return false;			
	}
	
	public bool IsEnemy( PlayerSide otherPlayerSide ) {
		
		if( IsBlank() == false && playerSide != otherPlayerSide )
			return true;
		return false;			
	}	
}	
//}

