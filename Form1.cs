using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace langgen
{
    public partial class Form1 : Form
    {
        int wait = 30;
        List<ns2grid> nsh = new List<ns2grid>();
        List<string> locale = new List<string>();
        List<string[]> locale_t = new List<string[]>();
        List<grids> hq = new List<grids>();
        int selected = 0;

        public Form1(bool buildIndex)
        {
            InitializeComponent();
            textBox1.ReadOnly = true;
            this.DoubleBuffered = true;

            if (buildIndex)
            {
                loadTexts();

                for (int i = 0; i < locale.Count; i++)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        loadGrid(i, j, locale_t[i]);
                        Console.WriteLine("Processed {0} for n={1}", locale[i], j);
                    }
                }
                //for (int i = 4; i <= 8; i++ )
                {
                   // loadGrid(locale.IndexOf("Svenska"), i, locale_t[locale.IndexOf("Svenska")]);
                }
                serialize();
            }
            else
            {
                IFormatter formatter = new BinaryFormatter();

                Stream stream = new FileStream("data1.bin",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                hq = (List<grids>)formatter.Deserialize(stream);
                stream.Close();

                Stream stream2 = new FileStream("data2.bin",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                nsh = (List<ns2grid>)formatter.Deserialize(stream2);
                stream2.Close();

                Stream stream3 = new FileStream("data3.bin",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                locale = (List<string>)formatter.Deserialize(stream3);
                stream3.Close();

                Stream stream4 = new FileStream("data4.bin",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                locale_t = (List<string[]>)formatter.Deserialize(stream4);
                stream4.Close();

                foreach (string g in locale)
                    comboBox1.Items.Add(g);
            }
            comboBox1.SelectedIndex = 0;

        }



        void loadGrid(int a, int b, string[] texts)
        {
            if (b == 1)
            {
                hq.Add(new s1grid(a, b));
            }
            else if (b == 2)
            {
                hq.Add(new ns2grid(a, b));
            }
            else
            {
                hq.Add(new ngrid(a, b));
            }
            foreach (string y in texts)
                hq[hq.Count - 1].appendMap(y);

            hq[hq.Count - 1].computeMap();

            if (b == 2)
            {
                nsh.Add((ns2grid)hq[hq.Count - 1]);
            }
        }


        void loadTexts()
        {
            string[] c = Directory.GetDirectories(Environment.CurrentDirectory + "\\Corpus");

            foreach (string y in c)
            {
                string[] a = Directory.GetFiles(y);

                if (a.Length != 0)
                {
                    string[] jj = y.Split('\\');

                    locale.Add(jj[jj.Length - 1]);
                    comboBox1.Items.Add(jj[jj.Length - 1]);

                    string[] r = new string[a.Length];

                    for (int i = 0; i < r.Length; i++)
                    {
                        r[i] = loadText(a[i]);
                    }

                    locale_t.Add(r);
                }
            }
        }

        public void append(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(append), new object[] { value });
                return;
            }
            textBox1.Text += value;
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            running = !running;

            if (running)
                new Thread(wout2).Start();

        }

        static volatile bool running = false;

        void wout2()
        {
            int n = (int)numericUpDown1.Value, wcount = 0;
            string wlong = "";

            if (n <= 2)
            {
                int last = 1;

                while (running)
                {
                    last = hq[selected].getNextInt(last);

                    if (n == 1)
                        wlong += s1grid.chars[last].ToString();
                    else
                        wlong += s1grid.chars[last / s1grid.chars.Length].ToString() +
                    s1grid.chars[last % s1grid.chars.Length].ToString();

                    if (wcount == 10)
                    {
                        wcount = 0;
                        append(wlong);
                        wlong = "";
                        Thread.Sleep(wait * 2);
                    }
                    wcount++;
                }
            }
            else
            {
                string s = hq[selected].getFirst();

                while (running)
                {
                    s = hq[selected].getNext(s);
                    append(s);
                    Thread.Sleep(wait);
                }
            }
        }

        static string loadText(string path)
        {
            string ret = string.Empty;

            StreamReader r = new StreamReader(path, Encoding.UTF8);

            while (!r.EndOfStream)
            {
                ret += r.ReadLine();
            }
            r.Dispose();

            return ret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lid();
        }


        void lid()
        {
            string rn = textBox2.Text;
            textBox1.Clear();

            textBox1.Text += String.Format("        Input Query: \"{0}\"{1}      == Probability index =={1}{1}", rn, Environment.NewLine);


            SortedDictionary<double, string> ty = new SortedDictionary<double, string>();
            double inc = 0.00001;

            List<grids> nnn = new List<grids>();
            foreach (grids u in hq)
                if (u.getn() == (int)numericUpDown1.Value)
                    nnn.Add(u);

            for (int p = 0; p < locale.Count; p++)
            {
                try
                {
                    ty.Add(fkts.pred_c(rn, nnn[p]), locale[p]);
                }
                catch
                {
                    ty.Add(inc, locale[p]);
                    inc += 0.00001;
                }
            }

            for (int i = 0; i < locale.Count; i++)
            {
                //textBox1.Text += string.Format("{0}: {1}{2}", locale[i], avg_p2(rn, nsh[i]), Environment.NewLine);
            }
            List<string> fixorder = new List<string>();

            foreach (KeyValuePair<double, string> y in ty)
            {
                fixorder.Add(string.Format("        {0}: {1:0.000}{2}", y.Value, y.Key, Environment.NewLine));
            }
            for (int i = fixorder.Count - 1; i >= 0; i--)
            {
                textBox1.Text += fixorder[i];

                if (i == fixorder.Count - 1)
                    textBox1.Text += Environment.NewLine;
            }

            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text += String.Format("Ceasar {0}{1}", fkts.chiffer(textBox2.Text, nsh[0]), Environment.NewLine);
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 29; i++)
                textBox1.Text += "Shift " + i.ToString() + " : " + fkts.nshift(textBox2.Text, -i) + Environment.NewLine;
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }





        void serialize()
        {
            var formatter = new BinaryFormatter();
            Stream stream = new FileStream("data1.bin",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, hq);
            stream.Close();

            Stream stream2 = new FileStream("data2.bin",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream2, nsh);
            stream2.Close();
            Stream stream3 = new FileStream("data3.bin",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream3, locale);
            stream3.Close();
            Stream stream4 = new FileStream("data4.bin",
                                     FileMode.Create,
                                     FileAccess.Write, FileShare.None);
            formatter.Serialize(stream4, locale_t);
            stream4.Close();
        }



        private void button4_Click(object sender, EventArgs e)
        {
            loadGrid((int)comboBox1.SelectedIndex, (int)numericUpDown1.Value, locale_t[(int)comboBox1.SelectedIndex]);
            button4.Enabled = false;
            button6.Enabled = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            bool checkl = checkLoaded();

            button6.Enabled = checkl;
            button4.Enabled = !checkl;

            changeSelection();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool checkl = checkLoaded();

            button6.Enabled = checkl;
            button4.Enabled = !checkl;

            changeSelection();
        }
        void changeSelection()
        {
            for (int i = 0; i < hq.Count; i++)
            {
                if (hq[i].getlang() == (int)comboBox1.SelectedIndex
                    && hq[i].getn() == (int)numericUpDown1.Value)
                    selected = i;
            }
        }

        bool checkLoaded()
        {
            foreach (grids s in hq)
            {
                if (s.getlang() == (int)comboBox1.SelectedIndex
                    && s.getn() == (int)numericUpDown1.Value)
                    return true;
            }
            return false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialize();
            MessageBox.Show("Serialized!");
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lid();
                textBox2.Clear();
                textBox2.SelectionStart = 0;
                e.SuppressKeyPress = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public interface grids
    {
        void appendMap(string s);
        void computeMap();
        int getNextInt(int i);
        string getNext(string i);
        int getn();
        int getlang();
        string getFirst();
    }
}
