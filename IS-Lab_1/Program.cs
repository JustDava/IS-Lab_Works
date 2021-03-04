using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.CodeDom;
using System.ComponentModel;
using System.Configuration;
using System.Threading;

namespace IS_Lab_1
{
    class Plaifer
    {
        public static T ReadValueFromConsole<T>(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                try
                {
                    return (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
                }
                catch (Exception)
                {
                    Console.WriteLine("Возникла ошибка. Повторите ввод");
                }
            }

        }
        public static void ActivateMenu()
        {
            var alph = /*ReadValueFromConsole<string>("Введите алфавит");*/
            // АБВГДЕЖЗИКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ
            "АБВГДЕЖЗИКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

            var key = ReadValueFromConsole<string>("\nВведите ключевое слово");
            // ТОВАРИЩ
            //"ТОВАРИЩ";

            var pc = new PlayfairCipher(alph, key);
            bool active = true;
            while (active)
            {
                Console.Clear();
                var choise = ReadValueFromConsole<int>(@"Выберите действие:
                    1. Зашифровать указанную строку
                    2. Десшифровать указанную строку
                    3. THERE IS NO ESCAPE");
                switch (choise)
                {
                    case 1:
                        Console.Clear();
                        Console.Write("Шифрование строки.");
                        var normalMessage = ReadValueFromConsole<string>("\nВведите строку для шифрования: ");
                        // КОД ПЛЕЙФЕЙЕРА ОСНОВАН НА ИСПОЛЬЗОВАНИИ МАТРИЦЫ БУКВ
                        // "КОД ПЛЕЙФЕЙЕРА ОСНОВАН НА ИСПОЛЬЗОВАНИИ МАТРИЦЫ БУКВ";
                        var crypted = pc.Crypt(normalMessage);
                        Console.WriteLine("\nResult: " + crypted);
                        Console.WriteLine("De-crypted back: " + pc.Uncrypt(crypted));
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("Дешифрование строки.");
                        var cryptedMessage = ReadValueFromConsole<string>("\nВведите строку для дешифровки: ");
                        // МОЩЕЯВЧЪЛТАПЯВМОМРЗФИЫПТБКВИХБЦБЩШЪЧШЩИВТЧОАДХОПАБТИВАРМЖИ
                        //"МОЩЕЯВЧЪЛТАПЯВМОМРЗФИЫПТБКВИХБЦБЩШЪЧШЩИВТЧОАДХОПАБТИВАРМЖИ";
                        var normal = pc.Uncrypt(cryptedMessage);
                        Console.WriteLine("\nResult: " + normal);
                        Console.WriteLine("Crypted back: " + pc.Crypt(normal));
                        Console.ReadKey();
                        break;
                    case 3:
                        active = new Random().Next(3) != 0;
                        if (!active)
                        {
                            Console.WriteLine("Lucky One");
                            Thread.Sleep(2 * 1000);
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }

    class PlayfairCipher
    {
        string Alphabet { get; set; }
        string KeyWord { get; set; }
        public PlayfairCipher(string Alphabet, string KeyWord)
        {
            this.KeyWord = string.Join("",
                KeyWord.ToLower()
                .Replace("й", "и")
                .Replace("ё", "е")
                .Replace("ъ", "ь")
                .Replace(" ", "")
                .Distinct());

            this.Alphabet = Alphabet.ToLower()
                .Replace("й", "")
                .Replace("ё", "")
                .Replace(" ", "")
                .Replace("ь", "");
        }

        public string Crypt(string Message)
        {
            Func<string[,], int[], int[], string> getPair = GetPairForCrypt;

            return CryptOrUncrypt(Message, getPair);
        }

        public string Uncrypt(string Message)
        {
            Func<string[,], int[], int[], string> getPair = GetPairForUncrypt;

            return CryptOrUncrypt(Message, getPair);
        }

        string CryptOrUncrypt(string Message, Func<string[,], int[], int[], string> GetPair)
        {
            var alphMatrix = CreateAlphMatrix();
            var bigrams = CreateBigrams(Message);

            var returnMessage = "";

            var fIndex = new int[] { 0, 0 };
            var sIndex = new int[] { 0, 0 };

            foreach (var bi in bigrams)
            {
                fIndex = GetIndex(alphMatrix, bi.FirstLetter);
                sIndex = GetIndex(alphMatrix, bi.SecondLetter);

                returnMessage += GetPair(alphMatrix, fIndex, sIndex);
            }

            return returnMessage;
        }
        string GetPairForUncrypt(string[,] AplhMatrix, int[] FirstLetterIndexs, int[] SecondLetterIndexs)
        {
            string pair = "";

            if (FirstLetterIndexs[0] == SecondLetterIndexs[0])
            {
                pair += AplhMatrix[
                    FirstLetterIndexs[0],
                      ((FirstLetterIndexs[1] - 1 + AplhMatrix.GetLength(1)) % AplhMatrix.GetLength(1))];

                pair += AplhMatrix[
                    SecondLetterIndexs[0],
                      ((SecondLetterIndexs[1] - 1 + AplhMatrix.GetLength(1)) % AplhMatrix.GetLength(1))];
            }
            // В одном столбце
            else if (FirstLetterIndexs[1] == SecondLetterIndexs[1])
            {
                pair += AplhMatrix[
                 ((FirstLetterIndexs[0] - 1 + AplhMatrix.GetLength(0)) % AplhMatrix.GetLength(0)),
                    FirstLetterIndexs[1]];


                pair += AplhMatrix[
                ((SecondLetterIndexs[0] - 1 + AplhMatrix.GetLength(0)) % AplhMatrix.GetLength(0)),
                    SecondLetterIndexs[1]];
            }
            // В одном блое
            else
            {
                pair += AplhMatrix[
                   FirstLetterIndexs[0],
                   SecondLetterIndexs[1]];

                pair += AplhMatrix[
                    SecondLetterIndexs[0],
                    FirstLetterIndexs[1]];
            }

            return pair;
        }
        string GetPairForCrypt(string[,] AplhMatrix, int[] FirstLetterIndexs, int[] SecondLetterIndexs)
        {
            string pair = "";

            if (FirstLetterIndexs[0] == SecondLetterIndexs[0])
            {
                pair += AplhMatrix[
                 FirstLetterIndexs[0],
                   ((FirstLetterIndexs[1] + 1) % AplhMatrix.GetLength(1))];

                pair += AplhMatrix[
                    SecondLetterIndexs[0],
                      ((SecondLetterIndexs[1] + 1) % AplhMatrix.GetLength(1))];
            }

            // В одном столбце
            else if (FirstLetterIndexs[1] == SecondLetterIndexs[1])
            {
                pair += AplhMatrix[
                 ((FirstLetterIndexs[0] + 1) % AplhMatrix.GetLength(0)),
                    FirstLetterIndexs[1]];

                pair += AplhMatrix[
                ((SecondLetterIndexs[0] + 1) % AplhMatrix.GetLength(0)),
                    SecondLetterIndexs[1]];
            }

            // В одном блоке
            else
            {
                pair += AplhMatrix[
                   FirstLetterIndexs[0],
                   SecondLetterIndexs[1]];

                pair += AplhMatrix[
                    SecondLetterIndexs[0],
                    FirstLetterIndexs[1]];
            }

            return pair;
        }
        int[] GetIndex(string[,] matrix, string Letter)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == Letter)
                        return new int[] { i, j };
                }
            }
            throw new Exception();
        }

        IEnumerable<Bigram> CreateBigrams(string normalMessage)
        {
            normalMessage = normalMessage.ToLower()
                .Replace("й", "и")
                .Replace("ё", "е")
                .Replace("ь", "ъ")
                .Replace(" ", "");
            var temp = new List<Bigram>();

            for (int i = 0; i < normalMessage.Length; i += 2)
            {
                if (i == normalMessage.Length - 1)
                {
                    temp.Add(new Bigram()
                    {
                        FirstLetter = normalMessage[i].ToString(),
                        SecondLetter = "х"
                    });
                }
                else if (normalMessage[i] == normalMessage[i + 1])
                {
                    temp.Add(new Bigram()
                    {
                        FirstLetter = normalMessage[i].ToString(),
                        SecondLetter = "х"
                    });
                    i--;
                }
                else
                {
                    temp.Add(new Bigram()
                    {
                        FirstLetter = normalMessage[i].ToString(),
                        SecondLetter = normalMessage[i + 1].ToString()
                    });
                }
            }

            return temp;
        }
        string[,] CreateAlphMatrix()
        {
            var let = (KeyWord + Alphabet).Select(x => x.ToString()).Distinct().ToArray();

            var alphMatrix = new string[5, 6];

            var temp_index = 0;

            Console.WriteLine("New alphabet:");

            for (int i = 0; i < alphMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < alphMatrix.GetLength(1); j++)
                {
                    alphMatrix[i, j] = let[temp_index];
                    Console.Write(alphMatrix[i, j] + " ");
                    temp_index++;
                }
                Console.WriteLine("");
            }

            return alphMatrix;
        }

        class Bigram
        {
            public string FirstLetter { get; set; }
            public string SecondLetter { get; set; }

            public override string ToString()
            {
                return FirstLetter + " " + SecondLetter;
            }
        }
    }
    class Program
    {
        static string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя ,.";
        static string FileRead(string name)
        {
            StreamReader sr = new StreamReader(name);
            string line = sr.ReadToEnd();
            Console.WriteLine('\n' + line);
            sr.Close();
            return line;
        }
        // 1ое задание
        static string Encr(string text, int key)
        {
            var codeText = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (text[i] == alphabet[j])
                    {
                        codeText.Append(alphabet[(j + key) % alphabet.Length]);
                    }
                }
            }
            return codeText.ToString();
        }
        static string Descr(string codeText, int key)
        {
            var decodeText = new StringBuilder();
            for (int i = 0; i < codeText.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (codeText[i] == alphabet[j])
                    {
                        decodeText.Append(alphabet[(j - key + alphabet.Length) % alphabet.Length]);
                    }
                }
            }
            return decodeText.ToString();
        }
        // 2ое задание
        static string MultipEncr(string text, int key)
        {
            var codeText = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (text[i] == alphabet[j])
                    {
                        codeText.Append(alphabet[(j * key) % alphabet.Length]);
                    }
                }
            }
            return codeText.ToString();
        }

        static void Frequency(string text)
        {
            int i = 0;
            List<char> c = new List<char>();
            foreach (char ch in text)
            {
                if (!c.Contains(ch))
                {
                    i = 0;
                    for (int l = 0; text.Length > l; l++)
                    {
                        if (ch == text[l]) i++;
                    }
                    c.Add(ch);
                    Console.Write("(" + ch + " = " + i.ToString() + ")" + " ");
                }
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Введите имя файла");
            string text = FileRead(Console.ReadLine());
            StreamWriter sw1 = new StreamWriter("1шифр.txt");
            sw1.WriteLine(Encr(text, 5));
            StreamWriter sw2 = new StreamWriter("1дешифр.txt");
            sw2.WriteLine(Descr(Encr(text, 5), 5));
            sw1.Close();
            sw2.Close();

            StreamWriter sw3 = new StreamWriter("2шифр.txt");
            sw3.WriteLine(MultipEncr(text, 5));
            StreamWriter sw4 = new StreamWriter("2дешифр.txt");
            sw4.WriteLine(MultipEncr(MultipEncr(text, 5), 29));
            sw3.Close();
            sw4.Close();

            Console.WriteLine("\nЧастотный анализ аддитивного шифра\n");
            Frequency(Encr(text, 5));
            Console.WriteLine("\n\nЧастотный анализ мультипликативного шифра\n");
            Frequency(MultipEncr(text, 5));
            Console.WriteLine("\n\n\n\n");
            string str = Console.ReadLine();
            Console.Clear();
            if (str == "Плейфер")
            {
                Plaifer.ActivateMenu();
            }
        }
    }
}
