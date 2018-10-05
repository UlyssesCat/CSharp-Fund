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
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace Fund
{
    public partial class Form1 : Form
    {
        Thread td;
        //初始化页数
        int page = 1;
        //初始化开始界面，并把页数转到第一页

        List<string> fundsID = new List<string>();
        List<Stock> stocks = new List<Stock>();//第三季度
        List<Stock> stocks_ = new List<Stock>();//第二季度

        public Form1()
        {
            InitializeComponent();
            ThreadStart ts = new ThreadStart(GetIntroduction);
            td = new Thread(ts);
            td.SetApartmentState(ApartmentState.STA);
            td.Start();
        }

        void GetIntroduction()
        {
            turnTo(1);
        }

        //获取网页的内容，代码来自老师给的股票
        string GetContent(string url)
        {
            string html = "";
            // 发送查询请求
            WebRequest request = WebRequest.Create(url);
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
                // 获得流
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                html = sr.ReadToEnd();
                response.Close();
            }
            catch (Exception ex)
            {
                // 本机没有联网
                if (ex.GetType().ToString().Equals("System.Net.WebException"))
                {
                    MessageBox.Show("请检查你的计算机是否已连接上互联网。\n" + url, "提示");
                }
            }
            return html;
        }
        //转到特定页
        bool isturn = false;
        string turnTo(int pi)
        {
            if (isturn)
                return "";
            isturn = true;
            //清空之前内容
            this.dataGridView1.Rows.Clear();
            string url = "http://fund.eastmoney.com/data/rankhandler.aspx?op=ph&dt=kf&ft=all&rs=&gs=0&sc=zzf&st=desc&sd=2015-10-29&ed=2016-10-29&qdii=&tabSubtype=,,,,,&pi="
                + pi + "&pn=50&dx=1&v=0.10850418109563731";

            string data = GetContent(url);
            //正则表达式，提取每两个引号之间内容
            Regex re = new Regex("(?<=\").*?(?=\")", RegexOptions.None);

            //用正则表达式提取内容
            MatchCollection mc = re.Matches(data);

            int index = 0;

            //跳过单数项
            int pass = 0;

            foreach (Match funds in mc)
            {
                if (pass % 2 == 1)
                {
                    pass++;
                    continue;
                }

                string fund = funds.Value;
                //把逗号之间的内容提取出来放进string数组里
                string[] all = Regex.Split(fund, ",", RegexOptions.IgnoreCase);
                //新建一行
                DataGridViewRow row = new DataGridViewRow();


                //之后的代码都是把string数组的内容放进每一行里
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows.Add(row);
                    dataGridView1.Rows[index].Cells[0].Value = all[0];
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[1].Value = all[1].Substring(0, (all[1].Length > 6 ? 6 : all[1].Length));
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[2].Value = all[3].Length == 0 ? "---" : all[3].Substring(5);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[3].Value = all[4].Length == 0 ? "---" : all[4];
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[4].Value = all[5].Length == 0 ? "---" : all[5];
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[5].Value = getPecent(all[6]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[6].Value = getPecent(all[7]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[7].Value = getPecent(all[8]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[8].Value = getPecent(all[9]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[9].Value = getPecent(all[10]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[10].Value = getPecent(all[11]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[11].Value = getPecent(all[12]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[12].Value = getPecent(all[13]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[13].Value = getPecent(all[14]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[14].Value = getPecent(all[15]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[15].Value = all[20];
                });
                //换行
                index++;
                pass++;
            }
            //改变页码
            page = pi;
            label1.Text = page + "/55";
            isturn = false;
            //清空文字框内容
            textBox1.Text = "";
            return url;

        }

        //获得两位有效数字的字符串百分数
        string getPecent(string temp)
        {
            if (temp.Length == 0)
            {
                return "---";
            }
            double d = Math.Round(Convert.ToDouble(temp), 2);
            temp = d.ToString();
            if (Convert.ToInt32(d) - d == 0)
                temp += ".00%";
            else if (Convert.ToInt32(d * 10) - d * 10 == 0)
                temp += "0%";
            else
                temp += "%";
            return temp;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //下一页
        private void button2_Click(object sender, EventArgs e)
        {
            if (page == 55)
                return;
            turnTo(page + 1);
        }
        //上一页
        private void button1_Click(object sender, EventArgs e)
        {
            if (page == 1)
                return;
            turnTo(page - 1);
        }
        //转到特定页
        private void button3_Click(object sender, EventArgs e)
        {
            //使用try catch通过文字框内容转到特定页，并抛出错误输出
            try
            {
                int a = Convert.ToInt32(textBox1.Text);
                if (!(a > 0 && a < 56))
                    throw new Exception();
                turnTo(a);
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入错误，请检查您输入的数字（1-55）。", "提示");
                textBox1.Text = "";
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button5.Enabled = true;
            }
        }

        //输入基金代码查看某只基金详情
        private void button5_Click(object sender, EventArgs e)
        {
            string str = textBox2.Text;

            //通过try catch把输入的基金代码转到网页，若网页不存在，则抛出错误，
            //同时把输入的错误基金代号的格式抛出
            try
            {


                FileStream fs = new FileStream("..\\..\\stock\\result\\code.txt", FileMode.Open, FileAccess.Read);
                List<string> list = new List<string>();
                StreamReader sr = new StreamReader(fs);
                //使用StreamReader类来读取文件 
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                // 从数据流中读取每一行，直到文件的最后一行
                string tmp = sr.ReadLine();
                while (tmp != null)
                {
                    list.Add(tmp);
                    tmp = sr.ReadLine();
                }
                //关闭此StreamReader对象 
                sr.Close();
                fs.Close();




                if (!(list.Contains(str)))
                {
                    throw new Exception();
                }
                string x = GetContent(@"http://fund.eastmoney.com/" + str + ".html");
                x = x.Substring(x.IndexOf("<title>") + 7);
                x = x.Substring(0, x.IndexOf("</title>"));

                Form2 f = new Form2(str);
                f.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入错误，不存编号为" + str + "的基金。", "提示");
                textBox1.Text = "";
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button5.Enabled = true;
            }
        }


        //查看某只基金详情
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            Form2 f = new Form2(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            for (int i = 1; i < 56; i++)
            {
                this.Invoke((EventHandler)delegate
                {
                    string url = "http://fund.eastmoney.com/data/rankhandler.aspx?op=ph&dt=kf&ft=all&rs=&gs=0&sc=zzf&st=desc&sd=2015-10-29&ed=2016-10-29&qdii=&tabSubtype=,,,,,&pi="
                        + i + "&pn=50&dx=1&v=0.10850418109563731";

                    string data1 = GetContent(url);
                    //正则表达式，提取每两个引号之间内容
                    Regex re = new Regex("(?<=\").*?(?=\")", RegexOptions.None);

                    //用正则表达式提取内容
                    MatchCollection mc = re.Matches(data1);


                    FileStream fs1 = new FileStream("..\\..\\stock\\result\\code.txt", FileMode.Create);
                    StreamWriter sw1 = new StreamWriter(fs1);

                    // 使用StreamWriter来往文件中写入内容 

                    foreach (Match funds in mc)
                    {
                        string fund = funds.Value;
                        //把逗号之间的内容提取出来放进string数组里
                        string[] all = Regex.Split(fund, ",", RegexOptions.IgnoreCase);
                        if (all[0].Length == 0)
                            continue;
                        fundsID.Add(all[0]);
                    }

                    for (int k = 0; k < fundsID.Count; k++) sw1.WriteLine(fundsID[k]);
                    //关闭此文件t 
                    sw1.Flush();
                    sw1.Close();
                    fs1.Close();

                });
            }
            string url2;
            string data;
            foreach (string ID in fundsID)
            {
                url2 = @"http://fund.eastmoney.com/f10/FundArchivesDatas.aspx?type=jjcc&code=" + ID + "&topline=10&year=&month=9,6&rt=0.28937255050441113";

                data = GetContent(url2);
                //如果没有表格内容，则返回
                //string的contain方法是判断字符串是否有一段特定的字符
                if (!data.Contains("<tbody>"))
                    continue;
                if (!data.Contains("市场"))
                {
                    data = data.Substring(data.IndexOf("<tbody>") + 7);
                    data = data.Substring(0, data.IndexOf("</tbody>"));

                    string tmp;
                    //处理字符串，获取需要的信息
                    while (data.Contains("<tr>"))
                    {
                        Stock s = new Stock();
                        DataGridViewRow row = new DataGridViewRow();

                        data = data.Substring(data.IndexOf("<td>") + 4);
                        data = data.Substring(data.IndexOf("<td>") + 4);
                        tmp = data.Substring(data.IndexOf(">") + 1);
                        //tmp = tmp.Substring(0, tmp.IndexOf("</a"));
                        tmp = tmp.Substring(0, tmp.IndexOf("<"));//股票代码
                        if (tmp == "") continue;
                        s.code = tmp;

                        data = data.Substring(data.IndexOf("<td"));
                        data = data.Substring(data.IndexOf(">") + 1);
                        tmp = data.Substring(data.IndexOf(">") + 1);
                        tmp = tmp.Substring(0, tmp.IndexOf("<"));//股票名称
                        if (tmp == "") continue;
                        s.name = tmp;

                        //跳过无用数据
                        data = data.Substring(data.IndexOf("<td") + 3);
                        data = data.Substring(data.IndexOf("<td") + 3);
                        data = data.Substring(data.IndexOf("<td") + 3);
                        data = data.Substring(data.IndexOf("<td") + 3);
                        data = data.Substring(data.IndexOf("<td") + 3);
                        data = data.Substring(data.IndexOf("<td"));
                        tmp = data.Substring(data.IndexOf(">") + 1);
                        tmp = tmp.Substring(0, tmp.IndexOf("<"));
                        if (tmp == "") continue;
                        s.sum = Convert.ToDouble(tmp);
                        stocks.Add(s);
                    }
                    //以上是爬第三季度，以下是爬第二季度
                    data = GetContent(url2);
                    data = data.Substring(data.IndexOf("<tbody>") + 7);
                    data = data.Substring(data.IndexOf("<tbody>") + 7);
                    data = data.Substring(0, data.IndexOf("</tbody>"));
                    //处理字符串，获取需要的信息
                    while (data.Contains("<tr>"))
                    {
                        Stock s = new Stock();
                        data = data.Substring(data.IndexOf("<td>") + 4);
                        data = data.Substring(data.IndexOf("<td>") + 4);
                        tmp = data.Substring(data.IndexOf(">") + 1);
                        //tmp = tmp.Substring(0, tmp.IndexOf("</a"));
                        tmp = tmp.Substring(0, tmp.IndexOf("<"));
                        if (tmp == "") continue;
                        s.code = tmp;

                        data = data.Substring(data.IndexOf("<td"));
                        data = data.Substring(data.IndexOf(">") + 1);
                        tmp = data.Substring(data.IndexOf(">") + 1);
                        tmp = tmp.Substring(0, tmp.IndexOf("<"));
                        if (tmp == "") continue;
                        s.name = tmp;

                        //跳过无用数据
                        data = data.Substring(data.IndexOf("<td") + 3);
                        data = data.Substring(data.IndexOf("<td") + 3);
                        data = data.Substring(data.IndexOf("<td") + 3);
                        data = data.Substring(data.IndexOf("<td"));
                        tmp = data.Substring(data.IndexOf(">") + 1);
                        tmp = tmp.Substring(0, tmp.IndexOf("<"));
                        if (tmp == "") continue;
                        s.sum = Convert.ToDouble(tmp);
                        stocks_.Add(s);
                    }
                }
            }
            List<Stock> tmp1 = new List<Stock>();
            bool IN;
            foreach (Stock st in stocks)
            {
                IN = false;
                for (int i = 0; i < tmp1.Count; i++)
                {
                    if (tmp1.ElementAt(i).sum < st.sum)
                    {
                        tmp1.Insert(i, st);
                        IN = true;
                        break;
                    }
                }
                if (!IN)
                {
                    tmp1.Add(st);
                }
            }
            List<Stock> tmp2 = new List<Stock>();
            foreach (Stock st in stocks_)
            {
                IN = false;
                for (int i = 0; i < tmp2.Count; i++)
                {
                    if (tmp2.ElementAt(i).sum < st.sum)
                    {
                        tmp2.Insert(i, st);
                        IN = true;
                        break;
                    }
                }
                if (!IN)
                {
                    tmp2.Add(st);
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 500; i++)//tmp1.Count
            {
                sb.Append(tmp1.ElementAt(i).code + "," + tmp1.ElementAt(i).name + "," + tmp1.ElementAt(i).sum + ",");
            }

            string result = sb.ToString();

            FileStream fs = new FileStream("..\\..\\stock\\result\\result6.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(result);
            //关闭此文件t 
            sw.Flush();
            sw.Close();
            fs.Close();
            StringBuilder sb_ = new StringBuilder();
            for (int i = 0; i < 500; i++)//tmp2.Count
            {
                sb_.Append(tmp2.ElementAt(i).code + "," + tmp2.ElementAt(i).name + "," + tmp2.ElementAt(i).sum + ",");
            }

            string result_ = sb_.ToString();

            FileStream fs_ = new FileStream("..\\..\\stock\\result\\result7.txt", FileMode.Create);//2
            StreamWriter sw_ = new StreamWriter(fs_);
            sw_.WriteLine(result_);
            //关闭此文件t 
            sw_.Flush();
            sw_.Close();
            fs_.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

    public class Stock
    {
        public string name { get; set; }
        public double sum { get; set; }
        public string code { get; set; }//股票代码

    }
}