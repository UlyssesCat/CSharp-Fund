using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Fund
{
    public class Stockrank
    {
        public string code;
        public string name;
        public string total;
    }
    public partial class Form9 : Form
    {
        String ID;
        Thread td;
        public Form9(string id)
        {
            InitializeComponent();
            ID = id;
            ThreadStart ts = new ThreadStart(GetIntroduction);
            td = new Thread(ts);
            td.SetApartmentState(ApartmentState.STA);
            td.Start();
        }
        void GetIntroduction()
        {
            string str2 = Read2();
            string str3 = Read3();
            List<Stockrank> qua2 = new List<Stockrank>();
            List<Stockrank> qua3 = new List<Stockrank>();
            List<Stockrank> result = new List<Stockrank>();
            string[] stock2 = str2.Split(',');
            string[] stock3 = str3.Split(',');
    
            for (int i = 0; i < (stock2.Length) /3; i++)
            {
                Stockrank temp = new Stockrank();
                temp.code = stock2[3 * i];
                temp.name = stock2[3 * i+1];
                temp.total = stock2[3 * i + 2];
                qua2.Add(temp);
            }
            for (int i = 0; i < (stock3.Length) /3; i++)
            {
                Stockrank temp = new Stockrank();
               temp.code = stock3[3 * i];
                temp.name = stock3[3 * i +1];
                temp.total = stock3[3 * i +2];
                qua3.Add(temp);
            }
            foreach (var element in qua3)
            {
                int count = qua2.Count;
                for (int i = 0; i < count; i++)
                {
                    if (element.code == qua2[i].code)
                    {
                        element.total = (Convert.ToDouble(qua2[i].total) - Convert.ToDouble(element.total)).ToString();

                        result.Add(element);
                        continue;
                    }
                }
            }

            var resultss =
                from element in result
                where Convert.ToDouble(element.total) >= 0
                orderby Convert.ToDouble(element.total) descending
                select element;
            List<Stockrank> results = new List<Stockrank>();
            List<Stockrank> last = new List<Stockrank>();
            foreach (var element in resultss)
                results.Add(element);
            var obj=results.Distinct();
            foreach (var element in obj)
                last.Add(element);
            for (int i = 0; i < 50; i++)
            {
                DataGridViewRow row = new DataGridViewRow();

                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows.Add(row);
                });
                dataGridView1.Rows[i].Cells[0].Value = i + 1;
                dataGridView1.Rows[i].Cells[1].Value = last[i].code;
                dataGridView1.Rows[i].Cells[2].Value = last[i].name;

                double d = Convert.ToDouble(last[i].total);
                d = Math.Round(d, 2);
                dataGridView1.Rows[i].Cells[3].Value = d;
            }
        }

        string Read2()
        {
            StreamReader sr = new StreamReader("..\\..\\stock\\result\\result7.txt", Encoding.Default);
            String data = sr.ReadToEnd();
            return data;
        }
        string Read3()
        {
            StreamReader sr = new StreamReader("..\\..\\stock\\result\\result6.txt", Encoding.Default);
            String data = sr.ReadToEnd();
            return data;
        }
    }
}
