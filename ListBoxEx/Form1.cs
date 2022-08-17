using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using Gma.System.MouseKeyHook;


namespace ListBoxEx
{
    public partial class Form1 : Form
    {

        public List<string> _items = new List<string>(); // <-- Add this

        static bool isRunning = false;

        private IKeyboardMouseEvents globalHook;
        
        public string runKey="F2", stopKey="F3";

        Thread t1;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;


        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F3)
            {
                //MessageBox.Show(string.Format("{0} Key Pressed", e.KeyData.ToString()));
                isRunning = false;
                if (t1 != null)
                {
                    if (t1.IsAlive)
                    {
                        t1.Abort();
                        button5.Enabled = false;
                        isRunning = false;
                    }
                }
            }
            //
            /*
            isRunning = false;
            //ExcuteMacro();
            if (t1 != null)
            {
                if (t1.IsAlive)
                {
                    t1.Abort();
                    button5.Enabled = false;
                    isRunning = false;
                }
            }
            */
        }

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;

            listBox1.DoubleClick += new System.EventHandler(listBox1_DoubleClick);

            //
            globalHook = Hook.GlobalEvents();

            globalHook.KeyDown += GlobalHook_KeyDown;
        
            //
            button5.Enabled = false;

            _items.Add("(시작)"); // <-- Add these
                                //_items.Add("Two");
                                //_items.Add("Three");

            //
            //_items.Add("지연 3초");
            //_items.Add("마우스 위치(2538,14) 왼버튼 클릭");
            listBox1.DataSource = null;
            
            listBox1.DataSource = _items;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("F2 pressed");
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //

            //_items.Add("마우스 위치(2532,0) 왼버튼 클릭");
            
            listBox1.DataSource = null;
            listBox1.DataSource = _items;
            //
            MouseEdit me = new MouseEdit(this);
            me.Show();

            //refresh

            //_items.Add("마우스 위치(0,0) 왼버튼 클릭");
            listBox1.DataSource = null;
            listBox1.DataSource = _items;
            label1.Text = string.Format("{0:D}개 항목이 기록됨", listBox1.Items.Count-1);

        }

        private void ExcuteMacro()
        {
            //
            Point point;
            foreach (var item in listBox1.Items)
            {
                var request = item.ToString();
                // Create a Regex  
                
                if (Regex.IsMatch(request, "([0-9]*,[0-9]*)"))
                {
                    var match = Regex.Match(request, "([0-9]*,[0-9]*)");
                    string[] coords = match.ToString().Split(',');
                    //Console.WriteLine(coords[0]);
                    point = new Point(int.Parse(coords[0]), int.Parse(coords[1]));
                    Console.WriteLine(point.X);
                    Console.WriteLine(point.Y);
                    //
                    int X = point.X;
                    int Y = point.Y;
                    
                    SetCursorPos(X, Y);

                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    //System.Threading.Thread.Sleep(1000);
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                }
                else if (request.Contains("지연"))
                {
                    //MessageBox.Show("match");
                    var match = Regex.Match(request, @"\d+");
                    //Console.WriteLine(match);
                    int delaytime = int.Parse(match.ToString());
                    System.Threading.Thread.Sleep(delaytime*1000);
                    //MessageBox.Show(match.ToString());
                }

            }

            //
            /*
            t1 = new Thread(new ThreadStart(RunMacro));
            // start newly created thread
            t1.Start();
            */
            

        }
        void RunMacro()
        {
            //           
            int cm = Int16.Parse(textBox1.Text);
            //MessageBox.Show(cm.ToString());
            if (cm == 0)
            {
                isRunning = true;
                while (true)
                {
                    if (isRunning == false) break;
                    ExcuteMacro();
                    Thread.Sleep(1000);
                }
                
            }
            else
                {
                isRunning = true;
                    for (int i = cm; i > 0; i--)
                    {

                        //MessageBox.Show(string.Format(isRunning.ToString()));
                        if (isRunning == false) {
                           // MessageBox.Show(string.Format(isRunning.ToString()));
                            break; }
                        //Thread.Sleep(5000);
                        else { ExcuteMacro(); }
                    }
                }
            
            button5.Enabled = false;
        }

    private void button3_Click(object sender, EventArgs e)
        {
            //ExcuteMacro();

            isRunning = true;
            button5.Enabled = true;
            RunMacro();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Delay dd = new Delay(this);
            dd.Show();
            //_items.Add("마우스 위치(0,0) 왼버튼 클릭")
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                //MessageBox.Show(listBox1.SelectedItem.ToString());
                show_MouseEdit();
            }


        }

        private void show_MouseEdit()
        {
            if (listBox1.SelectedItem != null)
            {
                //MessageBox.Show(listBox1.SelectedItem.ToString());
                string request = listBox1.SelectedItem.ToString();
                if (Regex.IsMatch(request, "([0-9]*,[0-9]*)"))
                {
                    var match = Regex.Match(request, "([0-9]*,[0-9]*)");
                    string[] coords = match.ToString().Split(',');

                    //
                    int X = int.Parse(coords[0]);
                    int Y = int.Parse(coords[1]);

                    MouseEdit me = new MouseEdit(this);
                    me.textBox3.Text = X.ToString();
                    me.textBox4.Text = Y.ToString();
                    me.Show();
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            show_MouseEdit();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

            _items.RemoveAt(listBox1.SelectedIndex);

            listBox1.DataSource = null;
            listBox1.DataSource = _items;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (t1 !=null)
            {
                if (t1.IsAlive)
                {
                    t1.Abort();
                    button5.Enabled = false;
                }
            }
            
        }
      
    }
}
