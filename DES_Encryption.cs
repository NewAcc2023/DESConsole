﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESConsole
{
	using System;
	using System.Text;

	class DES_Encryption
	{
		// constants regarding the keys

	int[] pc_1 = new int[56]{ 57 ,49 ,41 ,33 ,25 ,17 ,9  ,
				1  ,58 ,50 ,42 ,34 ,26 ,18 ,
				10 ,2  ,59 ,51 ,43 ,35 ,27 ,
				19 ,11 ,3  ,60 ,52 ,44 ,36 ,
				63 ,55 ,47 ,39 ,31 ,23 ,15 ,
				7  ,62 ,54 ,46 ,38 ,30 ,22 ,
				14 ,6  ,61 ,53 ,45 ,37 ,29 ,
				21 ,13 ,5  ,28 ,20 ,12 ,4 };

		int[] numLeftShift =new int[16] { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 }; // number of bits to shift for each iteration

	int[] pc_2= new int[48]  { 14 ,17 ,11 ,24 ,1  ,5  ,
				3  ,28 ,15 ,6  ,21 ,10 ,
				23 ,19 ,12 ,4  ,26 ,8  ,
				16 ,7  ,27 ,20 ,13 ,2  ,
				41 ,52 ,31 ,37 ,47 ,55 ,
				30 ,40 ,51 ,45 ,33 ,48 ,
				44 ,49 ,39 ,56 ,34 ,53 ,
				46 ,42 ,50 ,36 ,29 ,32 };

	// constants regarding the plain text

	int[] IP_t = new int[64] { 58 ,50 ,42 ,34 ,26 ,18 ,10 ,2 ,  // intital permutation table
				60 ,52 ,44 ,36 ,28 ,20 ,12 ,4 ,
				62 ,54 ,46 ,38 ,30 ,22 ,14 ,6 ,
				64 ,56 ,48 ,40 ,32 ,24 ,16 ,8 ,
				57 ,49 ,41 ,33 ,25 ,17 ,9  ,1 ,
				59 ,51 ,43 ,35 ,27 ,19 ,11 ,3 ,
				61 ,53 ,45 ,37 ,29 ,21 ,13 ,5 ,
				63 ,55 ,47 ,39 ,31 ,23 ,15 ,7 };

	int[] E_t =new int[48] { 32 ,1  ,2  ,3  ,4  ,5  , // expantion table
				4  ,5  ,6  ,7  ,8  ,9  ,
				8  ,9  ,10 ,11 ,12 ,13 ,
				12 ,13 ,14 ,15 ,16 ,17 ,
				16 ,17 ,18 ,19 ,20 ,21 ,
				20 ,21 ,22 ,23 ,24 ,25 ,
				24 ,25 ,26 ,27 ,28 ,29 ,
				28 ,29 ,30 ,31 ,32 ,1 };

		int[,,] S =new int[8,4,16] {                        // S-box
		{
			{ 14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,7 },
			{ 0,15,7,4,14,2,13,1,10,6,12,11,9,5,3,8 },
			{ 4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0 },
			{ 15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13 }
		},
		{
			{ 15,1,8,14,6,11,3,4,9,7,2,13,12,0,5,10 },
			{ 3,13,4,7,15,2,8,14,12,0,1,10,6,9,11,5 },
			{ 0,14,7,11,10,4,13,1,5,8,12,6,9,3,2,15 },
			{ 13,8,10,1,3,15,4,2,11,6,7,12,0,5,14,9 }
		},
		{
			{ 10,0,9,14,6,3,15,5,1,13,12,7,11,4,2,8 },
			{ 13,7,0,9,3,4,6,10,2,8,5,14,12,11,15,1 },
			{ 13,6,4,9,8,15,3,0,11,1,2,12,5,10,14,7 },
			{ 1,10,13,0,6,9,8,7,4,15,14,3,11,5,2,12 }
		},
		{
			{ 7,13,14,3,0,6,9,10,1,2,8,5,11,12,4,15 },
			{ 13,8,11,5,6,15,0,3,4,7,2,12,1,10,14,9 },
			{ 10,6,9,0,12,11,7,13,15,1,3,14,5,2,8,4 },
			{ 3,15,0,6,10,1,13,8,9,4,5,11,12,7,2,14 }
		},
		{
			{ 2,12,4,1,7,10,11,6,8,5,3,15,13,0,14,9 },
			{ 14,11,2,12,4,7,13,1,5,0,15,10,3,9,8,6 },
			{ 4,2,1,11,10,13,7,8,15,9,12,5,6,3,0,14 },
			{ 11,8,12,7,1,14,2,13,6,15,0,9,10,4,5,3 }
		},
		{
			{ 12,1,10,15,9,2,6,8,0,13,3,4,14,7,5,11 },
			{ 10,15,4,2,7,12,9,5,6,1,13,14,0,11,3,8 },
			{ 9,14,15,5,2,8,12,3,7,0,4,10,1,13,11,6 },
			{ 4,3,2,12,9,5,15,10,11,14,1,7,6,0,8,13 }
		},
		{
			{ 4,11,2,14,15,0,8,13,3,12,9,7,5,10,6,1 },
			{ 13,0,11,7,4,9,1,10,14,3,5,12,2,15,8,6 },
			{ 1,4,11,13,12,3,7,14,10,15,6,8,0,5,9,2 },
			{ 6,11,13,8,1,4,10,7,9,5,0,15,14,2,3,12 }
		},
		{
			{ 13,2,8,4,6,15,11,1,10,9,3,14,5,0,12,7 },
			{ 1,15,13,8,10,3,7,4,12,5,6,11,0,14,9,2 },
			{ 7,11,4,1,9,12,14,2,0,6,10,13,15,3,5,8 },
			{ 2,1,14,7,4,10,8,13,15,12,9,0,3,5,6,11 }
		}
	};

	int[] P = new int[32] { 16 ,7  ,20 ,21 ,
				29 ,12 ,28 ,17 ,
				1  ,15 ,23 ,26 ,
				5  ,18 ,31 ,10 ,
				2  ,8  ,24 ,14 ,
				32 ,27 ,3  ,9  ,
				19 ,13 ,30 ,6  ,
				22 ,11 ,4  ,25 };

	int[] P_1 = new int[64] { 40 ,8  ,48 ,16 ,56 ,24 ,64 ,32 ,
				39 ,7  ,47 ,15 ,55 ,23 ,63 ,31 ,
				38 ,6  ,46 ,14 ,54 ,22 ,62 ,30 ,
				37 ,5  ,45 ,13 ,53 ,21 ,61 ,29 ,
				36 ,4  ,44 ,12 ,52 ,20 ,60 ,28 ,
				35 ,3  ,43 ,11 ,51 ,19 ,59 ,27 ,
				34 ,2  ,42 ,10 ,50 ,18 ,58 ,26 ,
				33 ,1  ,41 ,9  ,49 ,17 ,57 ,25 };

		// Constants regarding the keys

		// Some functions

		string ShiftBit(string s, int n)
		{
			string k = "";

			for (int i = n; i < s.Length; i++)
				k += s[i];

			for (int i = 0; i < n; i++)
				k += s[i];

			return k;
		}

		void ExpandR(string r, string r32) // Expanding according to expansion table E_t
		{
			r = "";
			for (int j = 0; j < 48; j++)
			{
				r += r32[E_t[j] - 1];
			}
		}

		string XorAdd(string s1, string s2)
		{
			string result = "";
			for (int j = 0; j < s1.Length; j++)
			{
				if (s1[j] != s2[j])
					result += '1';
				else
					result += '0';
			}
			return result;
		}

		string GetElementFromBox(string s, int k)
		{
			int dec1 = 0, dec2 = 0, pwr = 0;
			dec1 = (int)(s[0] - '0') * 2 + (int)(s[5] - '0');
			for (int i = s.Length - 2; i >= 1; i--)
			{
				dec2 += (int)(s[i] - '0') * (int)Math.Pow(2, pwr++);
			}

			return DecToBin(S[k,dec1,dec2]);
		}

		public void Encrypt(string plainTxt, string key)
		{
			// Making sub-keys

			string key64 = HexToBin(key);

			string key56 = "";
			string keyFirstHalf = "";
			string keySecondHalf = "";

			for (int i = 0; i < 56; i++)
				key56 += key64[pc_1[i] - 1];

			for (int i = 0; i < 28; i++)
				keyFirstHalf += key56[i];

			for (int i = 28; i < 56; i++)
			{
				keySecondHalf += key56[i];
			}

			string[] LKey = new string[16];
			string[] RKey = new string[16];

			LKey[0] = ShiftBit(keyFirstHalf, numLeftShift[0]);  // Shifting the bits according to numLeftShift
			RKey[0] = ShiftBit(keySecondHalf, numLeftShift[0]);

			for (int i = 1; i < 16; i++)
			{
				LKey[i] = ShiftBit(LKey[i - 1], numLeftShift[i]);
				RKey[i] = ShiftBit(RKey[i - 1], numLeftShift[i]);
			}

			string[] key48 = new string[16];
			string[] keys56 = new string[16];

			for (int i = 0; i < 16; i++)
			{
				keys56[i] = LKey[i] + RKey[i]; // Making 56 bits keys
			}
			for (int i = 0; i < 16; i++)
			{
				key48[i] = "";
				for (int j = 0; j < 48; j++)
					key48[i] += keys56[i][pc_2[j] - 1]; // Making 48 bits keys
			}

			// Working on the plain text

			string plainTxt64 = HexToBin(plainTxt); // Transforming text from 16-bits hex to 64-bits bin

			string IP = ""; // Permuted text

			for (int i = 0; i < 64; i++)
				IP += plainTxt64[IP_t[i] - 1];

			string L = "";
			string R = "";

			for (int i = 0; i < 32; i++)
				L += IP[i];

			for (int i = 32; i < 64; i++)
				R += IP[i];

			string[] L32 = new string[16];
			string[] R32 = new string[16];
			string[] RXorK = new string[16];
			string[] R48 = new string[16]; // Making R32 48 bits so that we can XOR it with key48 (which is 48 bits)
			string[] SR = new string[16];
			string[,] s = new string[16,16];
			string[] s1 = new string[16];
			string[] PR = new string[16];

			R48[0] = "";
			for (int j = 0; j < 48; j++)
				R48[0] += R[E_t[j] - 1];

			RXorK[0] = XorAdd(R48[0], key48[0]); // Fill the RXorK array

			for (int j = 0; j < 48; j += 6) // Dividing each value of RXorK to 8 strings containing 6 characters each
				for (int k = j; k < j + 6; k++)
					s[0,(j / 6)] += RXorK[0][k];

			s1[0] = "";
			for (int j = 0; j < 8; j++)
				s1[0] += GetElementFromBox(s[0,j], j);

			for (int j = 0; j < 32; j++)
				PR[0] += s1[0][P[j] - 1];

			L32[0] = R;
			R32[0] = "";
			R32[0] = XorAdd(PR[0], L);

			for (int i = 1; i < 16; i++)
			{
				L32[i] = R32[i - 1];
				R48[i] = "";
				for (int j = 0; j < 48; j++)
					R48[i] += R32[i - 1][E_t[j] - 1];

				RXorK[i] = XorAdd(R48[i], key48[i]); // Fill the RXorK

				for (int j = 0; j < 48; j += 6) // Dividing each value of RXorK to 8 strings containing 6 characters each
					for (int k = j; k < j + 6; k++)
						s[i,j / 6] += RXorK[i][k];

				s1[i] = "";
				for (int j = 0; j < 8; j++)
					s1[i] += GetElementFromBox(s[i,j], j);

				for (int j = 0; j < 32; j++)
					PR[i] += s1[i][P[j] - 1];

				L32[i] = R32[i - 1];
				R32[i] = "";
				R32[i] = XorAdd(PR[i], L32[i - 1]);
			}

			string encryptedBin = "";
			string RL;

			RL = R32[15] + L32[15];

			for (int i = 0; i < 64; i++)
				encryptedBin += RL[P_1[i] - 1];

			Console.WriteLine(BinToHex(encryptedBin));
		}

		string BinToHex(string s)
		{
			string hex = "";
			for (int i = 0; i < s.Length; i += 4)
			{
				string k = "";
				for (int j = i; j < i + 4; j++)
					k += s[j];
				if (k == "0000")
					hex += '0';
				else if (k == "0001")
					hex += '1';
				else if (k == "0010")
					hex += '2';
				else if (k == "0011")
					hex += '3';
				else if (k == "0100")
					hex += '4';
				else if (k == "0101")
					hex += '5';
				else if (k == "0110")
					hex += '6';
				else if (k == "0111")
					hex += '7';
				else if (k == "1000")
					hex += '8';
				else if (k == "1001")
					hex += '9';
				else if (k == "1010")
					hex += 'A';
				else if (k == "1011")
					hex += 'B';
				else if (k == "1100")
					hex += 'C';
				else if (k == "1101")
					hex += 'D';
				else if (k == "1110")
					hex += 'E';
				else if (k == "1111")
					hex += 'F';
			}
			return hex;
		}

		string HexToBin(string s)
		{
			string bin = "";
			for (int i = 0; i < s.Length; i++)
			{
				switch (s[i])
				{
					case '0': bin += "0000"; break;
					case '1': bin += "0001"; break;
					case '2': bin += "0010"; break;
					case '3': bin += "0011"; break;
					case '4': bin += "0100"; break;
					case '5': bin += "0101"; break;
					case '6': bin += "0110"; break;
					case '7': bin += "0111"; break;
					case '8': bin += "1000"; break;
					case '9': bin += "1001"; break;
					case 'A':
					case 'a': bin += "1010"; break;
					case 'B':
					case 'b': bin += "1011"; break;
					case 'C':
					case 'c': bin += "1100"; break;
					case 'D':
					case 'd': bin += "1101"; break;
					case 'E':
					case 'e': bin += "1110"; break;
					case 'F':
					case 'f': bin += "1111"; break;

				}
			}
			return bin;
		}

		string DecToBin(int n)
		{
			string bin = "";
			while (n > 0)
			{
				bin = (char)(n % 2 + '0') + bin;
				n /= 2;
			}
			while (bin.Length < 4)
				bin = '0' + bin;
			return bin;
		}
	}
}
