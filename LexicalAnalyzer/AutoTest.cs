//////////////////////////////////////////////////////////////////////////////////////////////////////////
// Autotest.cs - Project #2																				//
// ver 1.0																								//
//Language: C# VS2017																					//
//Platform : Dell Inspiron 15 3543 Windows 10															//
// Application: Lexical Scanner Project- Software Modelling Analysis									//
// Author: Harsh Doshi SUID 980107176																	//
//////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package provides the code output demonstrating all the requirements for the project.
  
 
 * Build Process
 * --------------------
 * Required Files:ITokCollection.cs
                 :Semiexp.cs


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
using SemiExpression;
using I_Token_Collection;

namespace Automated_Test_Unit
{
	using Token = StringBuilder;
	using Semi = StringBuilder;
	public class Autotest
	{
		public static void Main(string[] args)
		{
			StringBuilder req = new StringBuilder();
			Console.Write("\n  Lexical Scanner Project");


			string path = "../../../TextFile2.txt";
			string completepath = System.IO.Path.GetFullPath(path);

			ITokCollection semiexp = new Semiexp();
			condition1(completepath, semiexp);
			condition2(completepath, semiexp);
			condition3(completepath, semiexp);
			condition4(completepath, semiexp);
			condition5(completepath, semiexp);
			condition6(completepath, semiexp);
			condition7(completepath, semiexp);
			Console.Read();

		}


		private static void condition1(string completepath, ITokCollection semiexp)
		{
			StringBuilder req = new StringBuilder();
			req.Append("\n - ///////// Requirement  1  ////////////////////");
			req.Append("\n -The project uses Visual Studio 2017 and its C# Windows Console Projects, as provided.");
			Console.Write(req);
			Console.Write("\n");
		}

		private static void condition2(string completepath, ITokCollection semiexp)
		{
			StringBuilder req = new StringBuilder();
			req.Append(" \n - ///////// Requirement  2  ////////////////////");
			req.Append(" \n - The project uses the .Net System.IO and System.Text for all I/O.");
			Console.Write(req);
			Console.Write("\n");
		}

		private static void condition3(string completepath, ITokCollection semiexp)
		{
			StringBuilder req = new StringBuilder();
			req.Append("\n -///////// Requirement  3  ////////////////////");
			req.Append("\n - The project provides C# packages for Tokenizing, \n collecting SemiExpressions, and a scanner interface, ITokCollection");
			Console.Write(req);
			Console.Write("\n");
		}

		public static void condition4(string completepath, ITokCollection semiexp)
		{
			StringBuilder req = new StringBuilder();
			req.Append(" \n ///////// Requirement  4 and Requirement 5   ////////////////////");
			req.Append("\n - The project contains Tokenizer package that declares and defines a Toker class that implements the State Pattern with derived classes \n for collecting the following token types:.");
			req.Append("\n -alphanumeric tokens");
			req.Append("\n - punctuator tokens");
			req.Append("\n - Special single char and Double Special Char");
			req.Append("\n - Single Line Comments");
			req.Append("\n - Multi Line Comments");
			req.Append("\n - Double Quotes and Strings");
			Console.Write(req);
			if (!semiexp.open(completepath))
			{
				Console.WriteLine("\n Cannot open {0}\n ", completepath);

			}
			else
			{
				Console.Write("\n Currently processing file: {0}", completepath);
				try
				{
					List<Token> tokenList = semiexp.getTokens();
				}
				catch (Exception e)
				{
					Console.Write("Exception occurred:'{0}'", e);


				}
			}
		}


		public static void condition5(string completepath, ITokCollection semiexp)
		{

			StringBuilder req = new StringBuilder();
			req.Append(" \n -///////// Requirement  6 , Requirement 7 and Requirement 8  ////////////////////");
			req.Append(" \n -The project provides provide a SemiExpression package containing a class SemiExp used to retrieve collections \n of tokens by calling Toker::getTok() repeatedly until SemiExpression termination condition");
			req.Append("\n The project provides rules to handle special termination characters in the for loop");
			Console.Write(req);
			Console.Write("\n");
			if (!semiexp.open(completepath))
			{
				Console.Write("\n Unable to open {0}\n", completepath);
			}
			else
			{
				Console.Write("\n Currently processing file: {0}", completepath);
				try
				{
					semiexp.get();
				}
				catch (Exception e)
				{
					Console.Write("Exception occurred:'{0}'", e);


				}

			}

		}
		private static void condition6(string completepath, ITokCollection semiexp)
		{
			StringBuilder req = new StringBuilder();
			req.Append("\n - ///////// Requirement  9  ////////////////////");
			req.Append("\n -The project implements the interface ITokenCollection with a declared method get().");
			Console.Write(req);
			Console.Write("\n");
		}

		private static void condition7(string completepath, ITokCollection semiexp)
		{
			StringBuilder req = new StringBuilder();
			req.Append("\n - ///////// Requirement  10  ////////////////////");
			req.Append("\n -The project is using the automated test suit for implementing all the special cases \n for the Tokenizer and SemiExpression package.");
			Console.Write(req);
			Console.Write("\n");
		}

	}

}
