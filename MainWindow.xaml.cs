using System;
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

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        string standart_alf = "abcdefghijklmnopqrstuvwxyz";
        string alf_rotor_1 =  "ekmflgdqvzntowyhxuspaibrcj";
        string alf_rotor_2 =  "ajdksiruxblhwtmcqgznpyfvoe";
        string alf_roter_3 =  "bdfhjlcprtxvznyeiwgakmusqo";
        string alf_roter_4 =  "esovpzjayquirhxlnftgkdcmwb";
        string alf_rotor_5 =  "vzbrgityupsdnhlxawmjqofeck";
        string reflector_b =  "aybrcudheqfsglipjxknmotzvw";
        char step;

        //private void animation_of_change_rotor_letter()
        private char enigma_algorith(string first_rotor_alf, string second_rotor_alf, string third_rotor_alf, char first_rotor_poz, char second_rotor_poz, char third_rotor_poz, char letter_input)
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
            step = standart_alf[second_rotor_alf.IndexOf(standart_alf[(standart_alf.IndexOf(step) - (standart_alf.IndexOf(first_rotor_poz) - standart_alf.IndexOf(second_rotor_poz)) + n) % n])];
            // step 7
            step = standart_alf[third_rotor_alf.IndexOf(standart_alf[(standart_alf.IndexOf(step) - (standart_alf.IndexOf(second_rotor_poz) - standart_alf.IndexOf(third_rotor_poz)) + n) % n])];
            //step 8
            step = standart_alf[(standart_alf.IndexOf(step) - standart_alf.IndexOf(third_rotor_poz) + n) % n];

            return step;



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string letter_of_btn = e.ContentStringFormat.ToString(); /// получаем букву от нажатой кнопки
            char input = 'a';
            enigma_algorith(alf_roter_3, alf_rotor_2, alf_rotor_1, 'c', 'v', 'r',input);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
