using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

//1.Создайте оконное приложение, использующее механизм задач (класс Task). Пользователь вводит в текстовое
//поле некоторый текст. По нажатию на кнопку приложение должно проанализировать текст и вывести отчет. Отчёт
//содержит информацию о:
//■■ Количестве предложений;
//■■ Количестве символов;
//■■ Количестве слов;
//■■ Количестве вопросительных предложений;
//■■ Количестве восклицательных предложений.
//Отчёт в зависимости от выбора пользователя отображается на экране или сохраняется в файл.
//Задание 2
//Добавьте к первому заданию возможность остановки и повторного запуска анализа текста
//Задание 3
//Добавьте к первому заданию возможность выбора типа информации, помещаемой в отчёт. Например,
//пользователь может выбрать, что его интересует только количество предложений и слов.
namespace AnalizeTextReport
{
    public partial class Form1 : Form
    {
        public CancellationTokenSource cancel = new CancellationTokenSource();
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e) //REPORT
        {
            string inputText = textBox1.Text;           
            var report = await Task.Run(() => AnalizeText(inputText));
            if (radioButton1.Checked)// показать отчет на экране
            {
                textBox2.Text = report;
            }
            else if (radioButton2.Checked)//сохранить в файл
            {             
                try
                {
                    FileStream file = new FileStream("AnalizeText.txt", FileMode.Create, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(file);
                    writer.WriteLine(report);
                    MessageBox.Show("Отчет сохранен в файл.");
                    writer.Close();
                    file.Close();
                   

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
                }
                
            }
        }
        private int CountSentence(string[] strings)
        {
            int count = 0;
            if (strings.Length != 1)
            {
                int i = 0;
                foreach (var str in strings)
                {
                    if (i == strings.Length - 1)
                        break;
                    if (str != string.Empty)
                        count++;
                    i++;
                }
            }
            return count;
        }

        public string AnalizeText(string text)
        {
            int sentenceCount = CountSentence(text.Split(new char[] { '.', '!', '?' }));
            int characterCount = text.Length;
            int wordCount = text.Split(new char[] { ' ', ':', ';', ',', '!','?','-','.'}, StringSplitOptions.RemoveEmptyEntries).Length;
            int questionCount = CountSentence(text.Split(new char[] { '?' }));
            int exclamationCount = CountSentence(text.Split(new char[] { '!' }));
            StringBuilder report = new StringBuilder();
            report.AppendLine($"Количество предложений: {sentenceCount}");
            report.AppendLine($"Количество символов: {characterCount}");
            report.AppendLine($"Количество слов: {wordCount}");
            report.AppendLine($"Количество вопросительных предложений: {questionCount}");
            report.AppendLine($"Количество восклицательных предложений: {exclamationCount}");

            return report.ToString();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) /// show on screen
        {

        }

        private void button3_Click(object sender, EventArgs e)////RE-START
        {
            cancel.Cancel();
            MessageBox.Show("Re-START");

        }
        //        Добавьте к первому заданию возможность выбора типа информации, помещаемой в отчёт.Например,
        //пользователь может выбрать, что его интересует только количество предложений и слов.
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    AnalizeText(textBox1.Text);

                    int sentenceCount = textBox1.Text.Split(new char[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
                    StringBuilder report = new StringBuilder();
                    report.AppendLine($"Количество предложений: {sentenceCount}");

                    FileStream file = new FileStream("Analize1.txt", FileMode.Create, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(file);
                    writer.WriteLine(report);
                    MessageBox.Show("Кол-во предложений сохранено в файл.");
                    writer.Close();
                    file.Close();
                    break;

                case 1:
                    AnalizeText(textBox1.Text);

                    int characterCount = textBox1.Text.Length;
                    StringBuilder report1 = new StringBuilder();
                    report1.AppendLine($"Количество символов: {characterCount}");

                    FileStream file2 = new FileStream("Analize2.txt", FileMode.Create, FileAccess.Write);
                    StreamWriter writer2 = new StreamWriter(file2);
                    writer2.WriteLine(report1);
                    MessageBox.Show("Кол-во символов сохранено в файл.");
                    writer2.Close();
                    file2.Close();
                    break;

                case 2:
                    AnalizeText(textBox1.Text);

                    int wordCount = textBox1.Text.Length;
                    StringBuilder report2 = new StringBuilder();
                    report2.AppendLine($"Количество символов: {wordCount}");

                    FileStream file3 = new FileStream("Analize3.txt", FileMode.Create, FileAccess.Write);
                    StreamWriter writer3 = new StreamWriter(file3);
                    writer3.WriteLine(report2);
                    MessageBox.Show("Кол-во слов сохранено в файл.");
                    writer3.Close();
                    file3.Close();
                    break;
            }
        }

        private async void button2_Click(object sender, EventArgs e)//STOP
        {
            cancel = new CancellationTokenSource();
            try
            {
                await Task.Run(() => AnalizeText(cancel.Token.ToString()), cancel.Token);
                MessageBox.Show("STOP!");
            }
            catch (OperationCanceledException)
            {

                MessageBox.Show("Задача отменена.");
            }
        }
    }
}
