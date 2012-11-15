using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class ChessEngineConfig {	
	
	public class Option {		
		
		public string Name {
			get; set;
		}
		
		public string Type {
			get; set;
		} 
		
		public string Default {
			get; set;
		} 
		
		public string Min {
			get; set;
		} 
		
		public string Max {
			get; set;
		} 
		
		public Queue<string> queueVar;
		
		public Option() {
			
			queueVar = new Queue<string>();
		}
		
		public void AddVar( string strVar ) {
			
			queueVar.Enqueue( strVar );
		}
		
		public void ClearAllVar() {
			
			queueVar.Clear();
		}
	}
	
	public string Name {
		get; set;
	}
	
	public string Authur {
		get; set;
	}
	
	public Dictionary<string, Option> mapOption;
	
	
	
	public ChessEngineConfig() {
		
		mapOption = new Dictionary<string, Option>();		
	}
	
	public void AddOption( Option option ) {
		
		mapOption[option.Name] = option;		
	}
	
	public void ClearAllOption( Option option ) {
		
		mapOption.Clear();		
	}
	
	public bool SetConfigCommand( CommandBase.CommandData commandData ) {
		
		bool bRet = false;		
		if( commandData.strCmd == "id" ) {
			
			CommandBase.CommandData subCmdData = commandData.queueSubCmdData.Peek();
			if( subCmdData != null && subCmdData.strCmd == "name" ) {
				Name = subCmdData.queueStrValue.Peek();				
				bRet = true;
			}
			else if( subCmdData != null && subCmdData.strCmd == "author" ) {
				Authur = subCmdData.queueStrValue.Peek();
				bRet = true;
			}					
		}
		else if( commandData.strCmd == "option" ) {
			
			Option option = new Option();
			
			foreach( CommandBase.CommandData subCmdData in commandData.queueSubCmdData ) {
			
				if( subCmdData != null ) {					
					
					if( subCmdData.strCmd == "name" ) {
						
						option.Name = subCmdData.queueStrValue.Peek();												
					}
					else if( subCmdData.strCmd == "type" ) {
						
						option.Type = subCmdData.queueStrValue.Peek();						
					}
					else if( subCmdData.strCmd == "default" ) {
						
						 option.Default = subCmdData.queueStrValue.Peek();
					}
					else if( subCmdData.strCmd == "min" ) {
						
						 option.Min = subCmdData.queueStrValue.Peek();
					}
					else if( subCmdData.strCmd == "max" ) {
						
						 option.Max = subCmdData.queueStrValue.Peek();
					}
					else if( subCmdData.strCmd == "var" ) {
						
						foreach( string strCurrVar in subCmdData.queueStrValue )
						 option.queueVar.Enqueue( strCurrVar );
					}
				}
			}
			
			AddOption( option );
			bRet = true;
		}
		
		return bRet;
	}
}
