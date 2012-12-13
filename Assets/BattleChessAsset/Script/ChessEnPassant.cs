using UnityEngine;
using System.Collections;

public struct ChessEnPassant {

	public int Rank { get; set; }
	public int Pile { get; set; }
	public bool Available { get; set; }
	
	
	string GetPileFenString() {
		
		string strRetPileFen;
		int nPile = Pile + 1;
		switch( nPile ) {
			
			case 1:
				strRetPileFen = "a";
			break;
			
			case 2:
				strRetPileFen = "b";
			break;
			
			case 3:
				strRetPileFen = "c";
			break;
			
			case 4:
				strRetPileFen = "d";
			break;
			
			case 5:
				strRetPileFen = "e";
			break;
			
			case 6:
				strRetPileFen = "f";
			break;
			
			case 7:
				strRetPileFen = "g";
			break;
			
			case 8:
				strRetPileFen = "h";
			break;
			
			default:
				strRetPileFen = "-";
				UnityEngine.Debug.LogError( "En passant Target Move error - Fen String" );
			break;					
		}
		
		return strRetPileFen;
	}
	
	public string GetFenString() {		
		
		string strRetFen = " ";
		if( Available ) {
			
			strRetFen += GetPileFenString() + (Rank + 1);
		}
		else {
			
			strRetFen += "-";
		}
		
		return strRetFen;
	}
}
