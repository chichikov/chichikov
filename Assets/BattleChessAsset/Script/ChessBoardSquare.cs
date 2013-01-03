using UnityEngine;
using System.Collections;

public class ChessBoardSquare {	
	
	public ChessPiece piece;		
	
	public ChessPosition position;
	
	public ParticleSystem movablePSystem;
	
	
	public ChessBoardSquare() {
		
		piece = null;		
		position.pos = BoardPosition.InvalidPosition;
		movablePSystem = null;
	}	
			
	public ChessBoardSquare( ChessPiece piece, ParticleSystem moveablePSystem, int nPile, int nRank ) {
		
		this.position.SetPosition( nRank, nPile );
		this.piece = piece;	
		this.piece.SetPosition( this.position.Get3DPosition() );
		
		SetMovableEffect( moveablePSystem );
	}	
	
	public void SetPiece( ChessPiece chessPiece ) {
		
		piece = chessPiece;
		piece.SetPosition( this.position.Get3DPosition() );			
	}	
	
	public void ClearPiece() {				
		
		if( IsBlank() )
			return;
			
		piece.Clear();
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
		
		if( piece == null )
			return true;
		return false;			
	}	
	
	public bool IsInvalidPos() {
		
		return position.IsInvalidPos();
	}
}
