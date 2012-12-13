using UnityEngine;
using System.Collections;


//namespace BattleChess {	
	
	
public struct ChessPosition {
		
	public BoardPosition pos;	
	public BoardPositionType posType;		
	
	
	public ChessPosition( int nRank, int nPile ) {
		
		posType = GetPositionType( nRank, nPile );
		if( posType != BoardPositionType.eNone ) {
		
			pos = (BoardPosition)(nPile * ChessData.nNumRank + nRank);
		}
		else {
			
			pos = BoardPosition.InvalidPosition;			
		}	
	}
	
	public ChessPosition( BoardPosition boardPos ) {
		
		posType = GetPositionType( boardPos );
		if( posType != BoardPositionType.eNone ) {			
		
			pos = boardPos;
		}
		else {
			
			pos = BoardPosition.InvalidPosition;				
		}	
	}			
	
	
	public bool SetPosition( int nRank, int nPile ) {
		
		posType = GetPositionType( nRank, nPile );
		if( posType != BoardPositionType.eNone ) {
		
			pos = (BoardPosition)(nPile * ChessData.nNumRank + nRank);
			return true;
		}
		else {
			
			pos = BoardPosition.InvalidPosition;			
		}	
		
		return false;
	}
	
	public bool SetPosition( BoardPosition boardPos ) {
		
		posType = GetPositionType( boardPos );
		if( posType != BoardPositionType.eNone ) {
		
			pos = boardPos;
			return true;
		}
		else {
			
			pos = BoardPosition.InvalidPosition;			
		}	
		
		return false;
	}
	
	public bool SetPosition( ChessPosition chessPos ) {			
		
		if( chessPos.posType != BoardPositionType.eNone ) {
		
			pos = chessPos.pos;
			posType = chessPos.posType;
			return true;
		}
		else {
			
			pos = BoardPosition.InvalidPosition;			
			posType = BoardPositionType.eNone;
		}	
		
		return false;
	}
	
	public Vector3 Get3DPosition() {
		
		Vector3 vRet = Vector3.zero;
		if( IsInvalidPos() )
			return vRet;
		
		int nRank = 0, nPile = 0;			
		GetPositionIndex( ref nRank, ref nPile );
		
		vRet.x = nRank - 3.5f;
		vRet.z = nPile - 3.5f;
		
		return vRet;
	}
	
	public bool MovePosition( int nRankMove, int nPileMove ) {			
				
		int nMovedRank, nMovedPile, nSrcRank = 0, nSrcPile = 0;
		GetPositionIndex( ref nSrcRank, ref nSrcPile );			
		
		nMovedRank = nSrcRank + nRankMove;
		nMovedPile = nSrcPile + nPileMove;
			
		if( nMovedRank >= 0 && nMovedRank < ChessData.nNumRank &&
			nMovedPile >= 0 && nMovedPile < ChessData.nNumPile ) {
			
			SetPosition( nMovedRank, nMovedPile);
			return true;
		}
		
		pos = BoardPosition.InvalidPosition;
		posType = BoardPositionType.eNone;
		
		return false;
	}
	
	public bool IsInvalidPos() {
		
		if( pos >= BoardPosition.InvalidPosition || posType == BoardPositionType.eNone )
			return true;
		return false;
	}		
	
	public bool IsLeftBoundary() {
		
		if( (posType & BoardPositionType.eLeft) == BoardPositionType.eLeft )
			return true;
		return false;
	}
	
	public bool IsRightBoundary() {
		
		if( (posType & BoardPositionType.eRight) == BoardPositionType.eRight )
			return true;
		return false;
	}
	
	public bool IsTopBoundary() {
		
		if( (posType & BoardPositionType.eTop) == BoardPositionType.eTop )
			return true;
		return false;
	}
	
	public bool IsBottomBoundary() {
		
		if( (posType & BoardPositionType.eBottom) == BoardPositionType.eBottom )
			return true;
		return false;
	}
	
	public bool IsLeftTopBoundary() {
		
		if( IsLeftBoundary() && IsTopBoundary() )
			return true;
		return false;
	}
	
	public bool IsLeftBottomBoundary() {
		
		if( IsLeftBoundary() && IsBottomBoundary() )
			return true;
		return false;
	}
	
	public bool IsRightTopBoundary() {
		
		if( IsRightBoundary() && IsTopBoundary() )
			return true;
		return false;
	}
	
	public bool IsRightBottomBoundary() {
		
		if( IsRightBoundary() && IsBottomBoundary() )
			return true;
		return false;
	}
	
	public bool IsBoundary() {
		
		if( IsLeftBoundary() || IsRightBoundary() || 
			IsTopBoundary() || IsBottomBoundary() )
			return true;
		return false;
	}	
	
	
	
	public BoardPositionType GetPositionIndex( ref int nRank, ref int nPile ) {
		
		BoardPositionType retBoardPos = GetPositionType( nRank, nPile );
		if( retBoardPos != BoardPositionType.eNone ) {				
		
			int nPos = (int)pos;
			nRank = nPos % ChessData.nNumRank;
			nPile = nPos / ChessData.nNumPile;
		}
		
		return retBoardPos;
	}
	
	
	
	// static function		
	public static BoardPositionType GetPositionIndex( BoardPosition pos, ref int nRank, ref int nPile ) {
		
		BoardPositionType retBoardPos = GetPositionType( nRank, nPile );
		if( retBoardPos != BoardPositionType.eNone ) {				
		
			int nPos = (int)pos;
			nRank = nPos % ChessData.nNumRank;
			nPile = nPos / ChessData.nNumPile;
		}
		
		return retBoardPos;
	}
	
	public static BoardPositionType GetPositionType( int nRank, int nPile ) {
		
		BoardPositionType retPosType = BoardPositionType.eNone;
		if( nRank >= 0 && nRank <= ChessData.nNumRank && 
			nPile >= 0 && nPile <= ChessData.nNumPile ) {
			
			retPosType |= BoardPositionType.eInside;
				
			if( nRank == 0 )
				retPosType |= BoardPositionType.eLeft;
			
			if( nRank == ChessData.nNumRank - 1 )
				retPosType |= BoardPositionType.eRight;
			
			if( nPile == 0 )
				retPosType |= BoardPositionType.eBottom;
			
			if( nRank == 0 )
				retPosType |= BoardPositionType.eTop;								
		}
		
		return retPosType;
	}
	
	public static BoardPositionType GetPositionType( BoardPosition pos ) {
		
		BoardPositionType retPosType = BoardPositionType.eNone;
		
		int nRank = 0, nPile = 0;
		retPosType = GetPositionIndex( pos, ref nRank, ref nPile );			
		return retPosType;			
	}
	
	public static bool GetRankPilePos( Vector3 vPos, ref int nRank, ref int nPile ) {		
	
		int nBoardWidth = (int)ChessData.fBoardWidth;
		
		nRank = (int)(vPos.x + 4.0f);																				
		nPile = (int)(vPos.z + 4.0f);	
		
		if( nRank >= 0 && nRank < nBoardWidth && 
			nPile >= 0 && nPile < nBoardWidth ) {												
			
			return true;
		}		
		
		return false;
	}
	
	
	//
	public override bool Equals(System.Object obj)
    {
        // If parameter cannot be cast to ThreeDPoint return false:
        ChessPosition rho = (ChessPosition)obj;	       

        // Return true if the fields match:
        return base.Equals(obj) && pos == rho.pos && posType == rho.posType;
    }

    public bool Equals(ChessPosition rho)
    {
        // Return true if the fields match:
        return base.Equals((System.Object)rho) && pos == rho.pos && posType == rho.posType;
    }
	
	public override int GetHashCode()
    {
        return base.GetHashCode() ^ (int)pos ^ (int)posType;
    }

	
	public static bool operator ==( ChessPosition lho, ChessPosition rho )
	{
	    // If both are null, or both are same instance, return true.
	    if (System.Object.ReferenceEquals(lho, rho))
	    {
	        return true;
	    }			    
	
	    // Return true if the fields match:
	    return lho.pos == rho.pos && lho.posType == rho.posType;
	}
	
	public static bool operator !=( ChessPosition lho, ChessPosition rho)
	{
	    return !(lho == rho);
	}	
}
//}