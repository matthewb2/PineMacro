using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Gma.System.MouseKeyHook;


namespace ListBoxEx
{
    public partial class MouseEdit : Form
    {
        public Form1 owner;
        public MouseEdit(Form1 form)
        {
            InitializeComponent();
            this.Load += MouseEdit_Load;

            owner = form;
            //this.KeyDown += new KeyEventHandler(MouseEdit_KeyDown);
            textBox4.KeyDown += new KeyEventHandler(textBox4_KeyDown);

            Hook.GlobalEvents().MouseMove += (sender, e) =>
            {
                //Console.WriteLine(e.Location.X);
                textBox1.Text = e.Location.X.ToString();
                textBox2.Text = e.Location.Y.ToString();
            };
            


        }

        private void MouseEdit_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("F2 pressed");
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(MouseEdit_KeyDown);
        }

        void MouseEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "F12")
            {
                //MessageBox.Show("F12 pressed");
                //ExcuteMacro();
                textBox3.Text = textBox1.Text;
                textBox4.Text = textBox2.Text;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.Application.Exit();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //owner._items.Add("마우스");
            //_items.Add("마우스 위치(0,0) 왼버튼 클릭");
            owner.runKey = comboBox2.Text.Replace(" Key", "");
            owner.stopKey = comboBox3.Text.Replace(" Key", "");

            owner._items.Add(string.Format("마우스 위치({0:D},{1:D}) 왼버튼 클릭", textBox3.Text.ToString(), textBox4.Text.ToString()));
            owner.listBox1.DataSource = null;
            owner.listBox1.DataSource = owner._items;
            owner.label1.Text = string.Format("{0:D}개 항목이 기록됨", owner.listBox1.Items.Count - 1);
            this.Close();
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //MessageBox.Show("Hello World");
                button1_Click(this, new EventArgs());
            }
        }
    }
}
