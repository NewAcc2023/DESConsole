using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DESConsole
{
	internal class Program
	{
		static void Main(string[] args)
		{
			DES_Encryption des = new DES_Encryption();
			des.Encrypt("0123456789ABCDEF", "133457799BBCDFF1");
			
		
		}
	}
}
