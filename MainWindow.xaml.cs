﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Runtime.Remoting.Channels;
using System.Threading;



namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        // глобальные перемемнные
        char rotor_1_poz ;
        char rotor_2_poz ;
        char rotor_3_poz ;
        Button last_bt = new Button();
        bool flag=false;
        // глобальные переменные

        // алфавиты

        string standart_alf = "abcdefghijklmnopqrstuvwxyz";//  
        string alf_rotor_1 = "ekmflgdqvzntowyhxuspaibrcj";// роторы
        string alf_rotor_2 = "ajdksiruxblhwtmcqgznpyfvoe";
        string alf_rotor_3 = "bdfhjlcprtxvznyeiwgakmusqo";
        string alf_rotor_4 = "esovpzjayquirhxlnftgkdcmwb";
        string alf_rotor_5 = "vzbrgityupsdnhlxawmjqofeck";
        string reflector_b = "aybrcudheqfsglipjxknmotzvw";
        string rotor_1 = "ekmflgdqvzntowyhxuspaibrcj";// настройки по умолчанию , выбраны роторы 1 2 3 соотвественно
        string rotor_2 = "ajdksiruxblhwtmcqgznpyfvoe";//
        string rotor_3 = "bdfhjlcprtxvznyeiwgakmusqo";//

        // алфавиты
        char step; // вспомогательная переменная

        public MainWindow()
        {
            InitializeComponent();
            // предварительная инициализация чекбоксов
            rotor_1_poz = rotor_1[0];
            rotor_2_poz = rotor_2[0];
            rotor_3_poz = rotor_3[0];
            Richtextbox_1.AppendText(char.ToString(rotor_1_poz));
            Richtextbox_2.AppendText(char.ToString(rotor_2_poz));
            Richtextbox_3.AppendText(char.ToString(rotor_3_poz));
        }
        
    
        private char enigma_algorith(string first_rotor_alf, string second_rotor_alf, string third_rotor_alf, char first_rotor_poz, char second_rotor_poz, char third_rotor_poz, char letter_input) // основной алгоритм шифрования
        {
            int n = standart_alf.Length;
            //
            //there will be 8 steps in total
            //step 1 
            step = third_rotor_alf[(standart_alf.IndexOf(letter_input) + standart_alf.IndexOf(third_rotor_poz)) % n];
            // step 2
            step = second_rotor_alf[(standart_alf.IndexOf(step) + (standart_alf.IndexOf(second_rotor_poz) - standart_alf.IndexOf(third_rotor_poz)) + n) % n];
            // step_3
            step = first_rotor_alf[(standart_alf.IndexOf(step) + (standart_alf.IndexOf(first_rotor_poz) - standart_alf.IndexOf(second_rotor_poz)) + n) % n];
            // step 4 / reflector B
            step = standart_alf[(standart_alf.IndexOf(step) - standart_alf.IndexOf(first_rotor_poz) + n) % n];
            int poz_step_4 = reflector_b.IndexOf(step);

            if (poz_step_4 % 2 == 0) // analog division om mod 2
                step = reflector_b[poz_step_4 + 1];
            else
                step = reflector_b[poz_step_4 - 1];
            // step_5 
            step = standart_alf[first_rotor_alf.IndexOf(standart_alf[(standart_alf.IndexOf(step) + standart_alf.IndexOf(first_rotor_poz)) % n])];
            //
            //step 6
            step = standart_alf[second_rotor_alf.IndexOf(standart_alf[(standart_alf.IndexOf(step) - (standart_alf.IndexOf(first_rotor_poz) - standart_alf.IndexOf(second_rotor_poz)) + 2*n) % n])];
            // step 7
            step = standart_alf[third_rotor_alf.IndexOf(standart_alf[(standart_alf.IndexOf(step) - (standart_alf.IndexOf(second_rotor_poz) - standart_alf.IndexOf(third_rotor_poz)) + 2*n) % n])];
            //step 8
            step = standart_alf[(standart_alf.IndexOf(step) - standart_alf.IndexOf(third_rotor_poz) + n) % n];

            return step;
        }
        private void draw(char output) // отрисовка высвечивания буквы на выходе 
        {
            Button but = new Button();
            if (flag)
                ((Ellipse)last_bt.Template.FindName("buttonSurface", last_bt)).Fill = new SolidColorBrush(Colors.LightGreen);

            but = (Button)FindName(char.ToString(output));
            last_bt= but;
            flag = true;
            
            ((Ellipse)but.Template.FindName("buttonSurface", but)).Fill = new SolidColorBrush(Colors.Red);

           
        }
        
        private void Button_Click(object sender, RoutedEventArgs e) // универсальный отработчик событий для кнопок алфавита
        {   
            Button this_button = sender as Button ;
            string letter_of_btn = Convert.ToString(((Button)sender).Content).ToLower(); /// получаем букву от нажатой кнопки
            if (letter_of_btn == null)
                return;
            char input = Char.Parse(letter_of_btn);
            //char output = enigma_algorith(rotor_3, rotor_2, rotor_1, 'c', 'v', 'r',input);
            char output = enigma_algorith(rotor_3, rotor_2, rotor_1, rotor_3_poz, rotor_2_poz, rotor_1_poz, input);
            rotors_poz_change();
            draw(output);
            r1.AppendText(char.ToString(input));
            r2.AppendText(char.ToString(output));

        } 
        private void rotors_poz_change() // прокрутка роторов при шифровании
        {
            if (rotor_1_poz == rotor_1[rotor_1.Length - 1]) // если послдений элемент то надо менять позицию и 2 ротора
            {
                if (rotor_2_poz == rotor_2[rotor_2.Length - 1]) // то надо еще и 3 ротор свигать
                {
                    if (rotor_3_poz == rotor_3[rotor_2.Length - 1]) // последний элемент 3 ротора
                    {
                        rotor_3_poz = rotor_2[0];
                        rotor_2_poz = rotor_2[0];
                        rotor_1_poz = rotor_2[0];
                    }
                    else
                    {
                        rotor_3_poz = rotor_3[rotor_3.IndexOf(rotor_3_poz) + 1];// просто вперед
                        rotor_2_poz = rotor_2[0];
                        rotor_1_poz = rotor_1[0];
                    }
                }
                else
                {
                    rotor_2_poz = rotor_2[rotor_2.IndexOf(rotor_2_poz) + 1];// просто вперед
                    rotor_1_poz = rotor_1[0];
                }
            }
            else
            {
                rotor_1_poz = rotor_1[rotor_1.IndexOf(rotor_1_poz) + 1];// просто вперед
            }
            ///
            Richtextbox_1.Document.Blocks.Clear();
            Richtextbox_1.AppendText(char.ToString(rotor_1_poz));

            Richtextbox_2.Document.Blocks.Clear();
            Richtextbox_2.AppendText(char.ToString(rotor_2_poz));

            Richtextbox_3.Document.Blocks.Clear();
            Richtextbox_3.AppendText(char.ToString(rotor_3_poz));

        } 

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e) // выбор первого ротора
        {
            RadioButton this_button = sender as RadioButton;
            switch (this_button.Content)
            {
                case "1":
                    Richtextbox_1.Document.Blocks.Clear();
                    rotor_1 = alf_rotor_1;
                    rotor_1_poz = rotor_1[0];
                    Richtextbox_1.AppendText(char.ToString(rotor_1_poz));
                    break;
                case "2":
                    Richtextbox_1.Document.Blocks.Clear();
                    rotor_1 = alf_rotor_2;
                    rotor_1_poz = rotor_1[0];
                    Richtextbox_1.AppendText(char.ToString(rotor_1_poz)); break;
                case "3":
                    Richtextbox_1.Document.Blocks.Clear();
                    rotor_1 = alf_rotor_3;
                    rotor_1_poz = rotor_1[0];
                    Richtextbox_1.AppendText(char.ToString(rotor_1_poz)); break;
                case "4":
                    Richtextbox_1.Document.Blocks.Clear();
                    rotor_1 = alf_rotor_4;
                    rotor_1_poz = rotor_1[0];
                    Richtextbox_1.AppendText(char.ToString(rotor_1_poz)); break;
                case "5":
                    Richtextbox_1.Document.Blocks.Clear();
                    rotor_1 = alf_rotor_5;
                    rotor_1_poz = rotor_1[0];
                    Richtextbox_1.AppendText(char.ToString(rotor_1_poz)); break;
            }

        } 
        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            RadioButton this_button = sender as RadioButton;
            switch (this_button.Content)
            {
                case "1":
                    Richtextbox_2.Document.Blocks.Clear();
                    rotor_2 = alf_rotor_1;
                    rotor_2_poz = rotor_2[0];
                    Richtextbox_2.AppendText(char.ToString(rotor_2_poz)); break;
                case "2":
                    Richtextbox_2.Document.Blocks.Clear();
                    rotor_2 = alf_rotor_2;
                    rotor_2_poz = rotor_2[0];
                    Richtextbox_2.AppendText(char.ToString(rotor_2_poz)); break;
                case "3":
                    Richtextbox_2.Document.Blocks.Clear();
                    rotor_2 = alf_rotor_3;
                    rotor_2_poz = rotor_2[0];
                    Richtextbox_2.AppendText(char.ToString(rotor_2_poz)); break;
                case "4":
                    Richtextbox_2.Document.Blocks.Clear();
                    rotor_2 = alf_rotor_4;
                    rotor_2_poz = rotor_2[0];
                    Richtextbox_2.AppendText(char.ToString(rotor_2_poz)); break;
                case "5":
                    Richtextbox_2.Document.Blocks.Clear();
                    rotor_2 = alf_rotor_5;
                    rotor_2_poz = rotor_2[0];
                    Richtextbox_2.AppendText(char.ToString(rotor_2_poz)); break;
            }

        } // выбор второго ротора
        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            RadioButton this_button = sender as RadioButton;
            switch (this_button.Content)
            {
                case "1":
                    Richtextbox_3.Document.Blocks.Clear();
                    rotor_3 = alf_rotor_1;
                    rotor_3_poz = rotor_3[0];
                    Richtextbox_3.AppendText(char.ToString(rotor_3_poz)); break;
                case "2":
                    Richtextbox_3.Document.Blocks.Clear();
                    rotor_3 = alf_rotor_2;
                    rotor_3_poz = rotor_3[0];
                    Richtextbox_3.AppendText(char.ToString(rotor_3_poz)); break;
                case "3":
                    Richtextbox_3.Document.Blocks.Clear();
                    rotor_3 = alf_rotor_3;
                    rotor_3_poz = rotor_3[0];
                    Richtextbox_3.AppendText(char.ToString(rotor_3_poz)); break;
                case "4":
                    Richtextbox_3.Document.Blocks.Clear();
                    rotor_3 = alf_rotor_4;
                    rotor_3_poz = rotor_3[0];
                    Richtextbox_3.AppendText(char.ToString(rotor_3_poz)); break;
                case "5":
                    Richtextbox_3.Document.Blocks.Clear();
                    rotor_3 = alf_rotor_5;
                    rotor_3_poz = rotor_3[0];
                    Richtextbox_3.AppendText(char.ToString(rotor_3_poz)); break;
            }

        } // выбор третьего ротора

        private void clear(object sender, RoutedEventArgs e)
        {
            r1.Document.Blocks.Clear();
            r2.Document.Blocks.Clear();


        }
        private void commutator(object sender, RoutedEventArgs e)// коммутатор
        {
            
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (standart_alf.Contains(e.Key.ToString().ToLower()))
            {
                char input = char.Parse(e.Key.ToString().ToLower());
                char output = enigma_algorith(rotor_3, rotor_2, rotor_1, rotor_3_poz, rotor_2_poz, rotor_1_poz, input);
                rotors_poz_change();
                draw(output);
                r1.AppendText(char.ToString(input));
                r2.AppendText(char.ToString(output));

            }

        }
        private void Button_Click_up_1(object sender, RoutedEventArgs e) // прокрутка ротора вручную
        {
        if (rotor_1_poz == rotor_1[rotor_1.Length - 1])
            rotor_1_poz = rotor_1[0];
        else
            rotor_1_poz = rotor_1[rotor_1.IndexOf(rotor_1_poz) + 1];// просто вперед

        Richtextbox_1.Document.Blocks.Clear();
        Richtextbox_1.AppendText(char.ToString(rotor_1_poz));
        }

        private void Button_Click_up_2(object sender, RoutedEventArgs e)// прокрутка ротора вручную
        {
            if (rotor_2_poz == rotor_2[rotor_2.Length - 1])
                rotor_2_poz = rotor_2[0];
            else
                rotor_2_poz = rotor_2[rotor_2.IndexOf(rotor_2_poz) + 1];// просто вперед

            Richtextbox_2.Document.Blocks.Clear();
            Richtextbox_2.AppendText(char.ToString(rotor_2_poz));
        }

        private void Button_Click_up_3(object sender, RoutedEventArgs e) // прокрутка ротора вручную
        {
            if (rotor_3_poz == rotor_3[rotor_3.Length - 1])
                rotor_3_poz = rotor_3[0];
            else
                rotor_3_poz = rotor_1[rotor_3.IndexOf(rotor_3_poz) + 1];// просто вперед

            Richtextbox_3.Document.Blocks.Clear();
            Richtextbox_3.AppendText(char.ToString(rotor_3_poz));
        }

        private void Button_Click_down_1(object sender, RoutedEventArgs e) // прокрутка ротора вручную
        {
            if (rotor_1_poz == rotor_1[0])
                rotor_1_poz = rotor_1[rotor_1.Length-1];
            else
                rotor_1_poz = rotor_1[rotor_1.IndexOf(rotor_1_poz) - 1];// просто назад

            Richtextbox_1.Document.Blocks.Clear();
            Richtextbox_1.AppendText(char.ToString(rotor_1_poz));
        }

        private void Button_Click_down_2(object sender, RoutedEventArgs e) // прокрутка ротора вручную
        {
            if (rotor_2_poz == rotor_2[0])
                rotor_2_poz = rotor_2[rotor_1.Length - 1];
            else
                rotor_2_poz = rotor_2[rotor_2.IndexOf(rotor_2_poz) - 1];// просто назад

            Richtextbox_2.Document.Blocks.Clear();
            Richtextbox_2.AppendText(char.ToString(rotor_2_poz));
        }

        private void Button_Click_down_3(object sender, RoutedEventArgs e)// прокрутка ротора вручную
        {
            if (rotor_3_poz == rotor_3[0])
                rotor_3_poz = rotor_3[rotor_1.Length - 1];
            else
                rotor_3_poz = rotor_3[rotor_3.IndexOf(rotor_3_poz) - 1];// просто назад

            Richtextbox_3.Document.Blocks.Clear();
            Richtextbox_3.AppendText(char.ToString(rotor_3_poz));
        }
    }
}
