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
using System.Text.RegularExpressions;
using AAJGen;

namespace KeyboardTrainer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RandomGen rnd;
        System.Timers.Timer timer=new System.Timers.Timer();
        int count=0;
        int wordsCount = 10;
        public int fails { get; set; } = 0;
        int currtextind = 0;
        string text = "";
        public MainWindow()
        {
            InitializeComponent();
            SwitchButtons();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Tick;
            Start.Click += On_Start;
            Stop.Click += On_Stop;
            rnd = (bool)Case.IsChecked ? new RandomGen(Option.IncludeCapitalAndSpecial) : new RandomGen(Option.Default);
            FailsC.Content = fails.ToString();
            Text.Focusable = false;
            Selection.Value = 0;
            Text.Text = " ";
        }
        private void On_Stop(object sender, RoutedEventArgs e)
        {
            text = "";
            Stop.IsEnabled = false;
            Start.IsEnabled = true;
            Text.Text = " ";
            Fails.Content = "Fails :";
            fails = 0;
            timer.Stop();
            currtextind = 0;
            Case.IsEnabled = true;
            FailsC.Content = fails.ToString();
            Selection.Value = 0;
        }
        private void On_Start(object sender, RoutedEventArgs e)
        {
            text = "";
            Text.Text = "";
            rnd = (bool)Case.IsChecked ? new RandomGen(Option.IncludeCapitalAndSpecial) : new RandomGen(Option.Default);
            Stop.IsEnabled = true;
            Start.IsEnabled = false;
            for (int i = 0; i < wordsCount; i++)
            {
                text += rnd.Gen((int)DiffSlider.Value*3) + " ";
            }
            Text.Text = text;
            timer.Start();
            Case.IsEnabled = false;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Speed.Content = $"Speed : {count * 60} chars/min";
                count = 0;
            }));
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            char? q=null;
            foreach (Border item in ButtonGrid.Children)
            {
                if ((Key)Convert.ToInt32(item.Tag as string) == e.Key)
                {
                    item.Opacity = 0.3;
                }
                else if(e.Key==Key.System&&Convert.ToInt32(item.Tag)==(int)e.SystemKey)
                {
                    item.Opacity = 0.3;
                }
                if(item.Tag as string == "8")
                {
                    SwitchButtons();
                }
                else if (item.Tag as string == "116" || item.Tag as string == "117")
                {
                    SwitchButtons();
                }
                if (e.Key == Key.Space)
                    q = ' ';
                else if (e.Key != Key.Space && Convert.ToInt32(item.Tag) == (int)e.Key && Stop.IsEnabled && e.Key != Key.System && e.Key != Key.Capital && e.Key != Key.LeftAlt && e.Key != Key.RightAlt && e.Key != Key.LWin && e.Key != Key.RWin && e.Key != Key.Tab && e.Key != Key.Back && e.Key != Key.LeftShift && e.Key != Key.RightShift && e.Key != Key.Enter && e.Key != Key.LeftCtrl && e.Key != Key.RightCtrl)
                    q = ((item.Child as Label).Content as string)[0];    
            }
            if (Stop.IsEnabled&& e.Key!=Key.System&&e.Key!=Key.Capital&&e.Key!=Key.LeftAlt&&e.Key!=Key.RightAlt&&e.Key!=Key.LWin&&e.Key!=Key.RWin&&e.Key!=Key.Tab&&e.Key!=Key.Back&&e.Key!=Key.LeftShift&&e.Key!=Key.RightShift&&e.Key!=Key.Enter&&e.Key!=Key.LeftCtrl&&e.Key!=Key.RightCtrl)
            {
                count++;
            }
            if (q != null)
            {
                if (q == Text.Text[currtextind])
                {                    
                    currtextind++;
                    ChangeCurrInd();
                    if (currtextind == Text.Text.Length - 1)
                    {
                        if(MessageBox.Show($"Congratulations!\n Your mistakes count: {fails} ", "End",MessageBoxButton.OK)==MessageBoxResult.OK)
                        On_Stop(null, new RoutedEventArgs());
                    }
                }
                else
                {
                    FailsC.Content = (++fails).ToString();
                }
            }
        }
        
        private void ChangeCurrInd()
        {
            Selection.Value+=0.4;
        }

        private void SwitchButtons()
        {
            foreach (Border item in ButtonGrid.Children)
            {
                if (((item.Child as Label).Content as string).Length == 1 && char.IsLetter(((item.Child as Label).Content as string)[0]))
                {
                    if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                        ((item.Child as Label).Content) = ((item.Child as Label).Content as string).ToUpper();
                    else if(System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock)&&(System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                        ((item.Child as Label).Content) = ((item.Child as Label).Content as string).ToLower();
                    else ((item.Child as Label).Content) = ((item.Child as Label).Content as string).ToLower();
                }
                else
                {
                    switch (item.Tag as string)
                    {
                        case "35":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "1";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock)|| System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "!";
                                break;
                            }
                            (item.Child as Label).Content = "1"; break;
                        case "36":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "2";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "@";
                                break;
                            }
                            (item.Child as Label).Content = "2"; break;
                        case "37":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "3";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "#";
                                break;
                            }
                            (item.Child as Label).Content = "3"; break;
                        case "38":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "4";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "$";
                                break;
                            }
                            (item.Child as Label).Content = "4"; break;
                        case "39":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "5";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "%";
                                break;
                            }
                            (item.Child as Label).Content = "5"; break;
                        case "40":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "6";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "^";
                                break;
                            }
                            (item.Child as Label).Content = "6"; break;
                        case "41":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "7";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "&";
                                break;
                            }
                            (item.Child as Label).Content = "7"; break;
                        case "42":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "8";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "*";
                                break;
                            }
                            (item.Child as Label).Content = "8"; break;
                        case "43":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "9";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "(";
                                break;
                            }
                            (item.Child as Label).Content = "9"; break;
                        case "34":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "0";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = ")";
                                break;
                            }
                            (item.Child as Label).Content = "0"; break;
                        case "143":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "-";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "_";
                                break;
                            }
                            (item.Child as Label).Content = "-"; break;
                        case "141":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "=";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "+";
                                break;
                            }
                            (item.Child as Label).Content = "="; break;
                        case "149":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "[";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "{";
                                break;
                            }
                            (item.Child as Label).Content = "["; break;
                        case "151":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "]";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "}";
                                break;
                            }
                            (item.Child as Label).Content = "]"; break;
                        case "150":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "\\";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "|";
                                break;
                            }
                            (item.Child as Label).Content = "\\"; break;
                        case "140":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = ";";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = ":";
                                break;
                            }
                            (item.Child as Label).Content = ";"; break;
                        case "146":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "`";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "~";
                                break;
                            }

                            (item.Child as Label).Content = "`"; break;
                        case "152":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "'";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "\"";
                                break;
                            }
                            (item.Child as Label).Content = "'"; break;
                        case "142":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = ",";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "<";
                                break;
                            }
                            (item.Child as Label).Content = ","; break;
                        case "144":
                            if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = ".";
                            }
                            else if(System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = ">";
                                break;
                            }
                            (item.Child as Label).Content = "."; break;
                        case "145":
                            if(System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) && (System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift)))
                            {
                                (item.Child as Label).Content = "/";
                            }
                            else if (System.Windows.Input.Keyboard.IsKeyToggled(Key.CapsLock) || System.Windows.Input.Keyboard.IsKeyDown(Key.LeftShift) || System.Windows.Input.Keyboard.IsKeyDown(Key.RightShift))
                            {
                                (item.Child as Label).Content = "?";
                                break;
                            }
                            (item.Child as Label).Content = "/"; break;
                    }
                }
            
        
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (Border item in ButtonGrid.Children)
            {
                if ((Key)Convert.ToInt32(item.Tag as string) == e.Key)
                {
                    item.Opacity=1;
                }
                else if (e.Key == Key.System && Convert.ToInt32(item.Tag) == (int)e.SystemKey)
                {
                    item.Opacity = 1;
                }
                if (item.Tag as string == "8")
                {
                    SwitchButtons();
                }
                else if(item.Tag as string == "116" || item.Tag as string == "117")
                {
                    SwitchButtons();
                }
            }
        }
    }
}
