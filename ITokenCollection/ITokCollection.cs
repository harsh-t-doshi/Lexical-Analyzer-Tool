//////////////////////////////////////////////////////////////////////////////////////////////////////////
// ITokCollection.cs - Project #2																		//
// ver 1.0																								//
//Language: C# VS2017																					//
//Platform : Dell Inspiron 15 3543 Windows 10															//
// Application: Lexical Scanner Project- Software Modelling Analysis									//
// Author: Harsh Doshi SUID 980107176																	//
//////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package provides an interface for token collection
 * 
 * 
 * Public Interfaces
 * ------------------------
 * getSemi()
 * getTokens()
 * bool open(completepath)
 

 * Maintenance History
 * -------------------
 * ver 1.0 : 7th October 2018
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_Token_Collection
{
	public interface ITokCollection
	{
		void get();

		bool open(string completepath);
		List<StringBuilder> getTokens();
		ITokCollection getSemi();
	}
}
