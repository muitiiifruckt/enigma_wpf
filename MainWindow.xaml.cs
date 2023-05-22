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
        char rotor_1_poz ;
        char rotor_2_poz ;
        char rotor_3_poz ;
        public MainWindow()
        {
            InitializeComponent();
            rotor_1_poz = rotor_1[0];
            rotor_2_poz = rotor_2[0];
            rotor_3_poz = rotor_3[0];
            Richtextbox_1.AppendText(char.ToString(rotor_1_poz));
            Richtextbox_2.AppendText(char.ToString(rotor_2_poz));
            Richtextbox_3.AppendText(char.ToString(rotor_3_poz));
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            //timer.Start();

        }
        string standart_alf = "abcdefghijklmnopqrstuvwxyz";//  
        string alf_rotor_1 =  "ekmflgdqvzntowyhxuspaibrcj";// роторы
        string alf_rotor_2 =  "ajdksiruxblhwtmcqgznpyfvoe";
        string alf_rotor_3 =  "bdfhjlcprtxvznyeiwgakmusqo";
        string alf_rotor_4 =  "esovpzjayquirhxlnftgkdcmwb";
        string alf_rotor_5 =  "vzbrgityupsdnhlxawmjqofeck";
        string reflector_b =  "aybrcudheqfsglipjxknmotzvw";
        string rotor_1 = "ekmflgdqvzntowyhxuspaibrcj";// настройки по умолчанию , выбраны роторы 1 2 3 соотвественно
        string rotor_2 = "ajdksiruxblhwtmcqgznpyfvoe";//
        string rotor_3 = "bdfhjlcprtxvznyeiwgakmusqo";//

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
            step = standart_alf[second_rotor_alf.IndexOf(standart_alf[(standart_alf.IndexOf(step) - (standart_alf.IndexOf(first_rotor_poz) - standart_alf.IndexOf(second_rotor_poz)) + 2*n) % n])];
            // step 7
            step = standart_alf[third_rotor_alf.IndexOf(standart_alf[(standart_alf.IndexOf(step) - (standart_alf.IndexOf(second_rotor_poz) - standart_alf.IndexOf(third_rotor_poz)) + 2*n) % n])];
            //step 8
            step = standart_alf[(standart_alf.IndexOf(step) - standart_alf.IndexOf(third_rotor_poz) + n) % n];

            return step;
        }
        async private void draw(char output)
        {
            Button but = new Button();

            but = (Button)FindName(char.ToString(output));
            
            ((Ellipse)but.Template.FindName("buttonSurface", but)).Fill = new SolidColorBrush(Colors.Red);

            await Task.Run(() => draw_2(output));
        }
        private void draw_2(char output)
        {
            Button but = new Button();

            but = (Button)FindName(char.ToString(output));
            Thread.Sleep(1000);
            ((Ellipse)but.Template.FindName("buttonSurface", but)).Fill = new SolidColorBrush(Colors.Black);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {   
            Button this_button = sender as Button ;
            string letter_of_btn = Convert.ToString(((Button)sender).Content).ToLower(); /// получаем букву от нажатой кнопки
            if (letter_of_btn == null)
                return;
            char input = Char.Parse(letter_of_btn);
            char output = enigma_algorith(rotor_3, rotor_2, rotor_1, 'c', 'v', 'r',input);
            draw(output);

        }
        //private void change_rotor_poz()
        private void output_char_light_letter(char output )
        {
           var btn = FindName(char.ToString(output));
           // (btn as Button).controlayout = 
        } 
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
        }


        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
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
        private void ch()
        {
            Richtextbox_1.Document.Blocks.Clear();
            Richtextbox_1.AppendText(char.ToString(rotor_1_poz));

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

        }
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

        }
   
 
        private void timer_Tick(object sender, EventArgs e)
        {
            Richtextbox_1.Document.Blocks.Clear();
            char rotor_1_poz = rotor_1[0];
            char rotor_2_poz = rotor_2[0];
            char rotor_3_poz = rotor_3[0];
            Richtextbox_1.AppendText(char.ToString(rotor_1_poz));
            Richtextbox_2.AppendText(char.ToString(rotor_2_poz));
            Richtextbox_3.AppendText(char.ToString(rotor_3_poz));
        }

        private void Button_Click_up_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_up_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_up_3(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_down_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_down_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_down_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
