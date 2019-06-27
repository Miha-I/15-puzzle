using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_15
{
    public partial class Form1 : Form
    {
        int[,] mas;
        List<Button> buttons;
        Random rnd;
        int distance_between_buttons, indent, sleep, number_of_turns;
        bool is_moove;
        public Form1()
        {
            InitializeComponent();
            buttons = new List<Button>();
            rnd = new Random();
            buttons.Add(button1);
            buttons.Add(button2);
            buttons.Add(button3);
            buttons.Add(button4);
            buttons.Add(button5);
            buttons.Add(button6);
            buttons.Add(button7);
            buttons.Add(button8);
            buttons.Add(button9);
            buttons.Add(button10);
            buttons.Add(button11);
            buttons.Add(button12);
            buttons.Add(button13);
            buttons.Add(button14);
            buttons.Add(button15);
            mas = new int[4, 4];
            distance_between_buttons = 55;
            number_of_turns = 0;
            indent = 15;
            sleep = 2;
            is_moove = true;
            for (int i = 0; i < 16; i++)
            {
                mas[(i / 4) % 4, i % 4] = i + 1;                                                                // Массив рассположения фишек
            }
                mas[3,3] = 0;                                                                                   // Последняя позиция пустая (нет фишки)
            Shuffle();
        }

        public void Shuffle()                                                                                   // Перемешиваем пятнашки передвижением
        {                                                                                                       // выбран этот метод т.к. при рандомном перемешиваниии
            int direction, y = 3, x = 3;                                                                        // можно не собрать пятнашки (две цифры будут рядом к примеру 15 и 14)
            for (int i = 0; i < 400; i++)
            {
                direction = rnd.Next(4);                                                                          // Направление движения
                if (direction == 1)                                                                                // вправо
                {
                    if (x > 0)
                    {
                        mas[y, x] = mas[y, x - 1];
                        mas[y, x - 1] = 0;
                        buttons[mas[y, x] - 1].Left += distance_between_buttons;
                        x--;
                    }
                }
                else if (direction == 2)                                                                           // Влево
                {
                    if (x < 3)
                    {
                        mas[y, x] = mas[y, x + 1];
                        mas[y, x + 1] = 0;
                        buttons[mas[y, x] - 1].Left -= distance_between_buttons;
                        x++;
                    }
                }
                else if (direction == 3)                                                                           // Вверх
                {
                    if (y < 3)
                    {
                        mas[y, x] = mas[y + 1, x];
                        mas[y + 1, x] = 0;
                        buttons[mas[y, x] - 1].Top -= distance_between_buttons;
                        y++;
                    }
                }
                else                                                                                               // Вниз
                {
                    if (y > 0)
                    {
                        mas[y, x] = mas[y - 1, x];
                        mas[y - 1, x] = 0;
                        buttons[mas[y, x] - 1].Top += distance_between_buttons;
                        y--;
                    }
                }
            }
            if(mas[3, 3] != 0)
            {
                mas[y, x] = mas[3, 3];
                buttons[mas[y, x] - 1].Top = y * distance_between_buttons + indent;
                buttons[mas[y, x] - 1].Left = x * distance_between_buttons + indent;
                mas[3, 3] = 0;
            }
            number_of_turns = 0;
        }

        private void button_Click(object sender, EventArgs e)                                               // двигаем
        {
            if (is_moove && sender is Button)
            {
                is_moove = false;
                Button button = sender as Button;
                int x = (button.Left - indent) / distance_between_buttons, y = (button.Top - indent) / distance_between_buttons, i = 0; // Определение координат нажатой фишки
                if (y - 1 >= 0 && mas[y - 1, x] == 0)                                                       // Проверка пустой клетки вверху
                {
                    mas[y, x] = 0;                                                                          // Помечаем текущую позицию как пустую
                    mas[y - 1, x] = button.TabIndex + 1;                                                    // Помечаем пустую позицию перемещаемой фишкой
                    while (i++ < distance_between_buttons)                                                  // Двигаем фишку
                    {
                        button.Top--;
                        System.Threading.Thread.Sleep(sleep);                                               // Скорость движения фишки
                    }
                    number_of_turns++;
                }
                if (y + 1 < 4 && mas[y + 1, x] == 0)                                                        // Проверка пустой клетки внизу
                {
                    mas[y, x] = 0;
                    mas[y + 1, x] = button.TabIndex + 1;
                    while (i++ < distance_between_buttons)
                    {
                        button.Top++;
                        System.Threading.Thread.Sleep(sleep);
                    }
                    number_of_turns++;
                }
                if (x - 1 >= 0 && mas[y, x - 1] == 0)                                                       // Проверка пустой клетки слева
                {
                    mas[y, x] = 0;
                    mas[y, x - 1] = button.TabIndex + 1;
                    while (i++ < distance_between_buttons)
                    {
                        button.Left--;
                        System.Threading.Thread.Sleep(sleep);
                    }
                    number_of_turns++;
                }
                if (x + 1 < 4 && mas[y, x + 1] == 0)                                                        // Проверка пустой клетки справа
                {
                    mas[y, x] = 0;
                    mas[y, x + 1] = button.TabIndex + 1;
                    while (i++ < distance_between_buttons)
                    {
                        button.Left++;
                        System.Threading.Thread.Sleep(sleep);
                    }
                    number_of_turns++;
                }
                Collected();
                is_moove = true;
            }
        }

        void Collected()                                                                        // Проверка собраны ли пятнашки
        {
            for (int i = 0; i < 15; i++)
            {
                if(mas[(i / 4) % 4, i % 4] != i + 1)
                    return;
            }
            if (MessageBox.Show( "Вы собрали пятнашки за " + number_of_turns + " хода(ов)\r\nИграем ещё?", "Конец игры", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                Shuffle();
            else
                Close();
        }
    }
}
