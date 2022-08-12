using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListBoxEx
{
    public partial class Delay : Form
    {
        public Form1 owner;
        public Delay(Form1 parent)
        {
            owner = parent;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            owner._items.Add(string.Format("지연 {0:D} 초", textBox1.Text.ToString()));
            owner.listBox1.DataSource = null;
            owner.listBox1.DataSource = owner._items;
            owner.label1.Text = string.Format("{0:D}개 항목이 기록됨", owner.listBox1.Items.Count - 1);
            this.Close();
        }
    }
}
