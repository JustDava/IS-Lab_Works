using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IS_Lab_2
{
    public partial class Form1 : Form
    {
        private void KeyUnlock()
        {
            int[] key = textBox2.Text.Split(' ').Select(Int32.Parse).ToArray();
            if (key.Length <= textBox_output.TextLength)
            {
                string output = "";
                char[] transposition = new char[key.Length];
                for (int i = 0; i < textBox_output.TextLength / key.Length; i++)
                {
                    int k = i;
                    for (int j = 0; j < key.Length; j++)
                    {
                        transposition[j] = textBox_output.Text[k];
                        k += textBox_output.TextLength / key.Length;
                    }
                    for (int j = 0; j < key.Length; j++)
                        output += transposition[j];
                }
                string result = "";
                for (int i = 0; i < output.Length; i += key.Length)
                {
                    transposition = new char[key.Length];
                    for (int j = 0; j < key.Length; j++)
                        transposition[key[j] - 1] = output[i + j];
                    for (int j = 0; j < key.Length; j++)
                        result += transposition[j];
                }
                int cut = result.Length - 1;
                while (result[cut] == '_')
                    cut--;
                if (cut != result.Length - 1)
                    result = result.Remove(cut + 1);
                textBox_output.Text = result;
            }
        }
        static class KeySearch
        {
            static private void swap(int[] key, int i, int j)
            {
                int s = key[i];
                key[i] = key[j];
                key[j] = s;
            }
            static public string nextKey(string startKey)
            {
                // преобразование входного ключа в массив целых значений
                int[] key = startKey.Split(' ').Select(Int32.Parse).ToArray();
                int n = key.Length;
                int j = n - 2;
                while (j != -1 && key[j] >= key[j + 1]) j--;
                if (j == -1)
                    return ""; // больше перестановок нет
                int k = n - 1;
                while (key[j] >= key[k]) k--;
                swap(key, j, k);
                int l = j + 1, r = n - 1; // сортируем оставшуюся часть последовательности
                while (l < r)
                    swap(key, l++, r--);
                string output = "" + key[0];
                for (int i = 1; i < n; i++)
                    output += " " + key[i];
                return output;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void обычныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                List<char> outputChar = new List<char>();
                for (int i = 0; i < textBox_input.TextLength; i += 2)
                    outputChar.Add(textBox_input.Text[i]);
                for (int i = 1; i < textBox_input.TextLength; i += 2)
                    outputChar.Add(textBox_input.Text[i]);
                string output = String.Join("", outputChar);
                textBox_output.Text = output;
            }
            else if (radioButton2.Checked)
            {
                List<char> firstPart = new List<char>();
                List<char> secondPart = new List<char>();
                if (textBox_output.TextLength % 2 == 0)
                {
                    for (int i = 0; i < textBox_output.TextLength / 2; i++)
                        firstPart.Add(textBox_output.Text[i]);
                    for (int i = textBox_output.TextLength / 2; i < textBox_output.TextLength; i++)
                        secondPart.Add(textBox_output.Text[i]);
                }
                else
                {
                    for (int i = 0; i <= textBox_output.TextLength / 2; i++)
                        firstPart.Add(textBox_output.Text[i]);
                    for (int i = (textBox_output.TextLength / 2) + 1; i < textBox_output.TextLength; i++)
                        secondPart.Add(textBox_output.Text[i]);
                }
                char[] outputChar = new char[textBox_output.TextLength];
                int j = 0;
                for (int i = 0; i < firstPart.Count; i++)
                {
                    outputChar[j] = firstPart[i];
                    j += 2;
                }
                j = 1;
                for (int i = 0; i < secondPart.Count; i++)
                {
                    outputChar[j] = secondPart[i];
                    j += 2;
                }
                string output = new string(outputChar);
                textBox_output.Text = output;
            }
        }

        private void ключевойToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (textBox2.Text != "")
                {
                    int[] key = textBox2.Text.Split(' ').Select(Int32.Parse).ToArray();
                    if (key.Length <= textBox_input.TextLength)
                    {
                        for (int i = 0; i < textBox_input.TextLength % key.Length; i++)
                            textBox_input.Text += '_';
                        string output = "";
                        for (int i = 0; i < textBox_input.TextLength; i += key.Length)
                        {
                            char[] transposition = new char[key.Length];
                            for (int j = 0; j < key.Length; j++)
                                transposition[j] = textBox_input.Text[i + key[j] - 1];
                            for (int j = 0; j < key.Length; j++)
                                output += transposition[j];
                        }
                        textBox_output.Text = output;
                    }
                }
            }
            else if (radioButton2.Checked)
            {
                int[] key = textBox2.Text.Split(' ').Select(Int32.Parse).ToArray();
                if (key.Length <= textBox_output.TextLength)
                {
                    string output = "";
                    for (int i = 0; i < textBox_output.TextLength; i += key.Length)
                    {
                        char[] transposition = new char[key.Length];
                        for (int j = 0; j < key.Length; j++)
                            transposition[key[j] - 1] = textBox_output.Text[i + j];
                        for (int j = 0; j < key.Length; j++)
                            output += transposition[j];
                    }
                    int cut = output.Length - 1;
                    while (output[cut] == '_')
                        cut--;
                    output = output.Remove(cut + 1);
                    textBox_output.Text = output;
                }

            }
        }

        private void комбинированныйToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                int length;
                int[] key = textBox2.Text.Split(' ').Select(Int32.Parse).ToArray();
                length = textBox_input.TextLength;
                if (key.Length <= textBox_input.TextLength)
                {
                    for (int i = 0; i < textBox_input.TextLength % key.Length; i++)
                        textBox_input.Text += '_';
                    string output = "";
                    char[] transposition = new char[key.Length];
                    for (int i = 0; i < textBox_input.TextLength; i += key.Length)
                    {
                        for (int j = 0; j < key.Length; j++)
                            transposition[j] = textBox_input.Text[i + key[j] - 1];
                        for (int j = 0; j < key.Length; j++)
                            output += transposition[j];
                    }
                    string result = "";
                    transposition = new char[textBox_input.TextLength / key.Length];
                    for (int i = 0; i < key.Length; i++)
                    {
                        int k = i;
                        for (int j = 0; j < output.Length / key.Length; j++)
                        {
                            transposition[j] = output[k];
                            k += key.Length;
                        }
                        for (int j = 0; j < output.Length / key.Length; j++)
                            result += transposition[j];
                    }
                    textBox_output.Text = result;
                }
            }
            else if (radioButton2.Checked)
            {
                KeyUnlock();
            }
        }
    }
}
