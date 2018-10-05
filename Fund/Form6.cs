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

namespace Fund
{


    public class StockContent
    {
        public string name;
        public string code;
        public string num;
        
    }
   


    public partial class Form6 : Form
    {
        List<StockContent> stock2 = new List<StockContent>();
        List<StockContent> stock3 = new List<StockContent>();








        string ID;
        Thread td;
        //初始化
        public Form6(string id)
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
            string url = @"http://fund.eastmoney.com/f10/FundArchivesDatas.aspx?type=jjcc&code=" + ID + "&topline=10&year=&month=9,6&rt=0.28937255050441113";

            string data = GetContent(url);
            //如果没有表格内容，则返回
            //string的contain方法是判断字符串是否有一段特定的字符
            if (!data.Contains("<tbody>"))
                return;
            if (!data.Contains("市场"))
            {
                data = data.Substring(data.IndexOf("<tbody>") + 7);
                data = data.Substring(0, data.IndexOf("</tbody>"));

                int index = 0;
                string tmp;
                //处理字符串，获取需要的信息
                while (data.Contains("<tr>"))
                {
                    StockContent temp = new StockContent();
                   

                    data = data.Substring(data.IndexOf("<td>") + 4);
                    tmp = data.Substring(0, data.IndexOf("<"));
               

                    data = data.Substring(data.IndexOf("<td>") + 4);
                    tmp = data.Substring(data.IndexOf(">") + 1);
                    //tmp = tmp.Substring(0, tmp.IndexOf("</a"));
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    
                        temp.code = tmp;
                   

                    data = data.Substring(data.IndexOf("<td"));
                    data = data.Substring(data.IndexOf(">") + 1);
                    tmp = data.Substring(data.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                   
                        temp.name = tmp;
                   

                    //跳过无用数据
                    data = data.Substring(data.IndexOf("<td") + 3);
                    data = data.Substring(data.IndexOf("<td") + 3);
                    data = data.Substring(data.IndexOf("<td") + 3);


                    data = data.Substring(data.IndexOf("<td") + 3);
                    tmp = data.Substring(data.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                 

                    data = data.Substring(data.IndexOf("<td") + 3);
                    tmp = data.Substring(data.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    
                        temp.num = tmp;
                   

                    data = data.Substring(data.IndexOf("<td"));
                    tmp = data.Substring(data.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                 
                    index++;
                    stock3.Add(temp);
                }
                
            }
           
           
            string url2 = @"http://fund.eastmoney.com/f10/FundArchivesDatas.aspx?type=jjcc&code=" + ID + "&topline=10&year=&month=9,6&rt=0.28937255050441113";

            string data2 = GetContent(url2);
            //如果没有表格内容，则返回
            //string的contain方法是判断字符串是否有一段特定的字符
            if (!data2.Contains("<tbody>"))
                return;
            if (!data2.Contains("市场"))
            {
                data2 = data2.Substring(data2.IndexOf("<tbody>") + 7);
                data2 = data2.Substring(data2.IndexOf("<tbody>") + 7);
                data2 = data2.Substring(0, data2.IndexOf("</tbody>"));
                int index = 0;
                string tmp;
                //处理字符串，获取需要的信息
                while (data2.Contains("<tr>"))
                {
                    StockContent temp = new StockContent();
                    //DataGridViewRow row = new DataGridViewRow();

                    data2 = data2.Substring(data2.IndexOf("<td>") + 4);
                    tmp = data2.Substring(0, data2.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        //dataGridView1.Rows.Add(row);
                        //dataGridView1.Rows[index].Cells[0].Value = tmp;
                    });

                    data2 = data2.Substring(data2.IndexOf("<td>") + 4);
                    tmp = data2.Substring(data2.IndexOf(">") + 1);
                    //tmp = tmp.Substring(0, tmp.IndexOf("</a"));
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        //dataGridView1.Rows[index].Cells[1].Value = tmp;
                        temp.code = tmp;

                    });

                    data2 = data2.Substring(data2.IndexOf("<td"));
                    data2 = data2.Substring(data2.IndexOf(">") + 1);
                    tmp = data2.Substring(data2.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        //dataGridView1.Rows[index].Cells[2].Value = tmp;
                        temp.name = tmp;
                    });

                    //跳过无用数据
                    data2 = data2.Substring(data2.IndexOf("<td") + 3);
                    data2 = data2.Substring(data2.IndexOf("<td") + 3);
                    //data2 = data2.Substring(data2.IndexOf("<td") + 3);


                    //data2 = data2.Substring(data2.IndexOf("<td") + 3);
                    tmp = data2.Substring(data2.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        //dataGridView1.Rows[index].Cells[3].Value = tmp;
                    });

                    data2 = data2.Substring(data2.IndexOf("<td") + 3);
                    tmp = data2.Substring(data2.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        //dataGridView1.Rows[index].Cells[4].Value = tmp;
                        temp.num = tmp;
                    });

                    data2 = data2.Substring(data2.IndexOf("<td"));
                    tmp = data2.Substring(data2.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        //dataGridView1.Rows[index].Cells[5].Value = tmp;
                    });
                    index++;
                    stock2.Add(temp);
                }
            }
            List<StockContent> result = new List<StockContent>();
            foreach (var element in stock3)
            {
                int count = stock2.Count;
                for (int i = 0; i < count; i++)
                {
                    if (element.name == stock2[i].name)
                    {
                        element.num = (Convert.ToDouble(stock2[i].num)- Convert.ToDouble(element.num)).ToString();
                        result.Add(element);
                        stock2.RemoveAt(i);
                        break;
                    }
                  
                }

            }
            foreach (var element in stock2)
            {
                element.num =  Convert.ToDouble(element.num).ToString();
                result.Add(element);
            }
            var results =
                from element in result
                where Convert.ToDouble(element.num) > 0
                orderby Convert.ToDouble(element.num) descending
                select element;

            int j = 0;
            foreach (var element in results)
            {

                DataGridViewRow row = new DataGridViewRow();
                this.Invoke((EventHandler)delegate
                {
                    //dataGridView1.Rows.Add(row);
                    //dataGridView1.Rows[index].Cells[0].Value = tmp;
                
                dataGridView1.Rows.Add(row);
                });
                dataGridView1.Rows[j].Cells[0].Value = j+1;
                dataGridView1.Rows[j].Cells[1].Value = element.code;
                dataGridView1.Rows[j].Cells[2].Value = element.name;
                dataGridView1.Rows[j].Cells[3].Value = element.num;
                j++;
            }
        }

        //获取网页内容
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
                    MessageBox.Show("请检查你的计算机是否已连接上互联网。", "提示");
                }
            }
            return html;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
