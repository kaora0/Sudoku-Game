using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        const int block_ = 3; // Размер блока
        const int butt_ = 50; // Размер кнопки
        public int[,] arr = new int[block_ * block_, block_ * block_]; // Игровое поле
        public Button[,] buttons = new Button[block_ * block_, block_ * block_];

        public Form1()
        {
            InitializeComponent();
            GenerateArr();
        }

        public void GenerateArr() // Заполняем поле
        {
            for(int i = 0; i < block_ * block_; i++)
            {
                for(int j = 0; j < block_ * block_; j++)
                {
                    arr[i, j] = (i * block_ + i / block_ + j) % (block_ * block_) + 1;
                    buttons[i, j] = new Button();
                }
            }
            
            Random r = new Random();
            for(int i = 0; i < 40; i++)
            {
                ShuffleMap(r.Next(0, 5));
            }
           
            CreateBut();
            HideCells();
        }

        public void CreateBut() // Создаем кнопки
        {
            for (int i = 0; i < block_ * block_; i++)
            {
                for (int j = 0; j < block_ * block_; j++)
                {
                    Button button = new Button();

                    buttons[i, j] = button;
                    button.Size = new Size(butt_, butt_);
                    button.Text = arr[i, j].ToString();
                    button.Click += OnCellPressed;
                    button.Location = new Point(j * butt_, i * butt_);

                    this.Controls.Add(button);
                }
            }
        }

        ////////////////////////////////////////////////////////////////Перетасовка////////////////////////////////////////////////////////////////

        public void MatrixTransposition() // Транспонирование
        {
            int[,] tArr = new int[block_ * block_, block_ * block_];
            for (int i = 0; i < block_ * block_; i++)
            {
                for (int j = 0; j < block_ * block_; j++)
                {
                    tArr[i, j] = arr[j, i];
                }
            }
            arr = tArr;
        }

        public void SwapRowsInBlock() // Смена строк в пределах одного блока
        {
            Random r = new Random();

            var block = r.Next(0, block_);
            var row1 = r.Next(0, block_);
            var line1 = block * block_ + row1;
            var row2 = r.Next(0, block_);

            while (row1 == row2)
                row2 = r.Next(0, block_);

            var line2 = block * block_ + row2;

            for (int i = 0; i < block_ * block_; i++)
            {
                var temp = arr[line1, i];
                arr[line1, i] = arr[line2, i];
                arr[line2, i] = temp;
            }
        }

        public void SwapColumnsInBlock() // Смена столбцов в пределах одного блока
        {
            Random r = new Random();

            var block = r.Next(0, block_);
            var сolumn1 = r.Next(0, block_);
            var line1 = block * block_ + сolumn1;
            var сolumn2 = r.Next(0, block_);

            while (сolumn1 == сolumn2)
                сolumn2 = r.Next(0, block_);

            var line2 = block * block_ + сolumn2;

            for (int i = 0; i < block_ * block_; i++)
            {
                var temp = arr[i, line1];
                arr[i, line1] = arr[i, line2];
                arr[i, line2] = temp;
            }
        }

        public void SwapBlocksInRow() // Смена блоков по горизонтали
        {
            Random r = new Random();

            var block1 = r.Next(0, block_);
            var block2 = r.Next(0, block_);

            while (block1 == block2)
                block2 = r.Next(0, block_);

            block1 *= block_;
            block2 *= block_;

            for (int i = 0; i < block_ * block_; i++)
            {
                var k = block2;
                for (int j = block1; j < block1 + block_; j++)
                {
                    var temp = arr[j, i];
                    arr[j, i] = arr[k, i];
                    arr[k, i] = temp;
                    k++;
                }
            }
        }

        public void SwapBlocksInColumn()
        {
            Random r = new Random();

            var block1 = r.Next(0, block_);
            var block2 = r.Next(0, block_);

            while (block1 == block2)
                block2 = r.Next(0, block_);

            block1 *= block_;
            block2 *= block_;

            for (int i = 0; i < block_ * block_; i++)
            {
                var k = block2;
                for (int j = block1; j < block1 + block_; j++)
                {
                    var temp = arr[i, j];
                    arr[i, j] = arr[i, k];
                    arr[i, k] = temp;
                    k++;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ShuffleMap(int i)
        {
            switch (i)
            {
                case 0:
                    MatrixTransposition();
                    break;
                case 1:
                    SwapRowsInBlock();
                    break;
                case 2:
                    SwapColumnsInBlock();
                    break;
                case 3:
                    SwapBlocksInRow();
                    break;
                case 4:
                    SwapBlocksInColumn();
                    break;
                default:
                    MatrixTransposition();
                    break;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void HideCells() // Кнопки для игры
        {
            int N = 40;
            Random r = new Random();
            while (N > 0)
            {
                for (int i = 0; i < block_ * block_; i++)
                {
                    for (int j = 0; j < block_ * block_; j++)
                    {
                        if (!string.IsNullOrEmpty(buttons[i, j].Text)){
                            int a = r.Next(0, 3);
                            buttons[i, j].Text = a == 0 ? "" : buttons[i, j].Text;
                            buttons[i, j].Enabled = a == 0 ? true : false;

                            if (a == 0)
                                N--;
                            if (N <= 0)
                                break;
                        }
                    }
                    if (N <= 0)
                        break;
                }
            }
        }


        public void OnCellPressed(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            string buttonText = pressedButton.Text;
            if (string.IsNullOrEmpty(buttonText))
            {
                pressedButton.Text = "1";
            }
            else
            {
                int num = int.Parse(buttonText);
                num++;
                if (num == 10)
                    num = 1;
                pressedButton.Text = num.ToString();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < block_ * block_; i++)
            {
                for(int j = 0; j < block_ * block_; j++)
                {
                    var btnText = buttons[i, j].Text;
                    if(btnText != arr[i, j].ToString())
                    {
                        MessageBox.Show("Обнаружены совпадения!\nРешение неверно");
                        return;
                    }
                }
            }
            MessageBox.Show("Все верно!");
            for(int i = 0; i < block_ * block_; i++)
            {
                for (int j = 0; j < block_ * block_; j++)
                {
                    this.Controls.Remove(buttons[i, j]);
                }
            }
            GenerateArr();
        }
    }
}
