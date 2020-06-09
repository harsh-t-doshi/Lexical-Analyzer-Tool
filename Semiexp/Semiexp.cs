//////////////////////////////////////////////////////////////////////////////////////////////////////////
// Semiexp.cs - Project #2																				//
// ver 1.0																								//
//Language: C# VS2017																					//
//Platform : Dell Inspiron 15 3543 Windows 10															//
// Application: Lexical Scanner Project- Software Modelling Analysis									//
// Author: Harsh Doshi SUID 980107176																	//
//////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------

 * This package forms the semiexpression from tokens based on the terminating conditions
  
 
 * Build Process
 * --------------------
 * Required Files:Toker.cs
                 :ITokenInterface.cs

* Public Interface
* Semiexp() semi = new Semiexp()
* semi.get()
* semi.open(completepath)
* 
 * Maintenance History
 * -------------------
 * ver 1.0 : 4th October 2018
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tokenizer;
using I_Token_Collection;

namespace SemiExpression
{
	using Token = StringBuilder;
	using Semi = StringBuilder;

	public class Semiexp : ITokCollection
	{
		Toker toker = null;
		Semi semi = null;

		public Semiexp()
		{
			toker = new Toker();
			semi = new Semi();
		}
		//------------------------ open the token source file------------------>
		public bool open(string completepath)
		{

			return toker.open(completepath);

		}
		//------------- Getting list of tokens------------------------>
		public List<Token> getTokens()
		{
			List<Token> tokenlst = new List<Token>();
			tokenlst = toker.getToken();
			return tokenlst;
		}

		//---------------- Checking Terminating conditions--------------->
		public void get()
		{
			Console.WriteLine("\n  ");
			Console.WriteLine("Printing SemiExpression \n");
			string[] terminator = { ";", "{", "}" };
			while (!toker.isDone())
			{
				Token tok = toker.getTok();
				semi.Append(tok);
				foreach (string terminate in terminator)
				{
					if (tok.ToString().Contains(terminate))
					{
						Console.WriteLine("\n" + semi);
						semi.Length = 0;
					}
				}
				if (tok.ToString().Equals("for"))
				{

					while (!tok.ToString().Contains("{"))
					{
						tok = toker.getTok();
						semi.Append(tok);
					}
					Console.Write(" " + semi);
					semi.Length = 0;
				}
				else if (tok.ToString().Contains("#"))
				{

					Console.Write("\n" + semi);
					semi.Length = 0;
				}
			}
		}

		public ITokCollection getSemi()
		{
			throw new NotImplementedException();
		}

		//	ITokCollection ITokCollection.getSemi()
		//{
		//			throw new NotImplementedException();
		//	}

#if (TEST_SEMI)

		class DemoToker
		{
			static bool testToker(string path)
			{
				Semiexp semi = new Semiexp();

				string fqf = System.IO.Path.GetFullPath(path);
				if (!semi.open(fqf))
				{
					Console.Write("\n can't open {0}\n", fqf);
					return false;
				}
				else
				{
					Console.Write("\n Processing file: {0}", fqf);

				}
				semi.get();
				/*	while (!toker.isDone())
					{
						Token tok = toker.getTok();
						Console.Write("\n -- line#{0, 4} : {1}", toker.lineCount(), tok);
					}
					toker.close();
				*/
				return true;
			}
			static void Main(string[] args)
			{
				Console.Write("\n  Demonstrate Semiexp class");
				Console.Write("\n ============================= \n");
				StringBuilder msg = new StringBuilder();
				msg.Append("\n");
				Console.Write(msg);
				testToker("../../../TextFile2.txt");
				Console.Write("\n\n");
				Console.Read();
			}
		}
	}
}
#endif


