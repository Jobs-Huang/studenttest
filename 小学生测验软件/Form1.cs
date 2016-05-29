using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 小学生测验软件
{
    public partial class Form1 : Form
    {
        int question_num = 1;     //题号
        double score = 0;          //分数
        //int sum;
        //int chance = 3;        //每道题有三次机会
        int t = 60;
        save_formula save_f = new save_formula();
        //int sum;
        class save_formula      //保存类，纪录每道题的算式和正确与否
        {
            public int[] frist_save = new int [10];
            public int[] sec_save = new int[10];
            public char[] operater = new char[10];
            public int[] sum_save = new int[10];
            public string[] ft = new string[10];
            public int []chance = new int [10];
        }



        class Formula       //算式类，每道题都是该类的对象
        {
            public int first_number;
            public int second_number;
            public int sum_number;
            public string operater;       //加减法运算符
           

            public Formula()        //构造函数
            {

            }
            public void build_equation()        //生成符合要求的算式
            {
                Random ran = new Random();

                first_number = ran.Next(0, 50);
                second_number = ran.Next(0, 50);
                int t = ran.Next(1, 10);
                if (t % 2 == 0)
                {
                    operater = "+";
                }
                else
                {
                    operater = "-";
                }
                sum_number = t % 2 == 0 ? first_number + second_number : first_number - second_number;
                //sum = sum_number;
                //sum_number = first_number + second_number;
            }
        }

        void show_formula()     //显示算式
        {
            Formula f1 = new Formula();
            f1.build_equation();
            while (f1.sum_number > 50 || f1.sum_number < 0)
                f1.build_equation();
            label1.Text = f1.first_number + f1.operater + f1.second_number + "=";
            //textBox1.Text = Convert.ToString(f1.sum_number);
            //sum = f1.sum_number;
            save_f.frist_save[question_num - 1] = f1.first_number;
            save_f.sec_save[question_num - 1] = f1.second_number;
            save_f.operater[question_num - 1] = Convert.ToChar(f1.operater);
            save_f.sum_save[question_num - 1] = f1.sum_number;
            save_f.chance[question_num - 1] = 3;
         //   Console.WriteLine("{0}{1}{2}={3}",save_f.frist_save,save_f.operater,save_f.sec_save,save_f.sum_save);

        }
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Text = "第" + question_num + "题" + "（共10题）";
            show_formula();
            timer1.Start();
           // save_formula sf = new save_formula();
        }

        public void next()
        {
            if (question_num == 10)
            {
                result();
                button2.Text = "重新测试";
                button1.Visible = true;
            }
            else
            {
                
                textBox1.Clear();
              
                question_num++;
                save_f.chance[question_num-1]=3;     //重置三次机会
                show_formula();
                label3.Text = "第" + question_num + "题" + "（共10题）";
                button2.Text = "提交";
                textBox1.Focus();
                
                t = -5*question_num+ 65;        //时间随题号变化

            }
        }

        public void result()
        {
            if (score == 100)
                MessageBox.Show("你真是个天才", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                if (score >= 90 && score <= 99)
                    MessageBox.Show("不错哟", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (score >= 80 && score <= 89)
                    MessageBox.Show("非常棒", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (score >= 70 && score <= 79)
                    MessageBox.Show("还需努力哟", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (score >= 60 && score <= 69)
                    MessageBox.Show("要加油啦", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (score < 60)
                    MessageBox.Show("很遗憾，再来一次", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "提交")
            {
                if (textBox1.Text == "")
                    MessageBox.Show("请输入你的答案~~~", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    if (Convert.ToInt32(textBox1.Text) == save_f.sum_save[question_num - 1])
                {
                    double temp = 0.5 * save_f.chance[question_num-1] * save_f.chance[question_num-1] + 0.5 * save_f.chance[question_num-1] + 4;
                    score += temp;
                    MessageBox.Show("回答正确", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    label2.Text = "当前分数：" + score;
                    button2.Text = "下一题";
                    next();
                  //  save_f.ft[question_num-1] = "正确";
                    timer1.Start();

                }
                else
                {
                    save_f.chance[question_num-1]--;
                    if(save_f.chance[question_num-1] == 0)
                    {
                        MessageBox.Show("回答错误，本题答案是" + save_f.sum_save[question_num - 1] , "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        MessageBox.Show("输入错误，请重新输入，本题你还有" + save_f.chance[question_num-1] + "次机会。", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    textBox1.Focus();
                  //  save_f.ft[question_num-1] ="错误";
                }
                if (save_f.chance[question_num-1] < 1)
                {
                 //   button2.Text = "下一题";
                    next();
                 //   save_f.ft[question_num-1] = "错误";

                }

                


            }
            else if (button2.Text == "重新测试")
            {
                if (MessageBox.Show("要重新测试吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    question_num = 1;     //题号
                    score = 0;          //分数
                    for (int i = 0; i < 10; i++)
                    {
                        save_f.chance[i] = 3;
                    }
                    t = 60;
                    //save_formula save_f = new save_formula();
                    label3.Text = "第" + question_num + "题" + "（共10题）";
                    label2.Text = "当前分数：" + score;
                    show_formula();
                    timer1.Start();
                    button2.Text = "提交";
                    listBox1.Visible = false;
                    textBox1.Visible = true;
                    button1.Visible = false;
                    textBox1.Text = "";
                    textBox1.Focus();
                }
              
            }
        }



        private void timer1_Tick(object sender, EventArgs e)
        {

            label4.Text = "本题剩余时间："+Convert.ToString(t--);
            if (t < 0)
            {
                timer1.Stop();
                MessageBox.Show("本题时间到！进入下一题，按确定继续。");
                next();
                timer1.Start();
                
            }
            else
            {
                if (question_num < 9)
                {
                    
                }
                else
                {
                    timer1.Stop();
                }
            }


        }



        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)  //按下enter，自动显示答案
        {
         //   Console.WriteLine("{0}", sum);
            if (e.KeyChar == 13)
                textBox1.Text = Convert.ToString(save_f.sum_save[question_num - 1]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Visible = !listBox1.Visible;
            textBox1.Visible = !textBox1.Visible;
            listBox1.Items.Clear();
            for(int i=0;i<10;i++)
            {
                if(save_f.chance[i]==3)
                {
                    save_f.ft[i] = "第一次答对";
                }
                if (save_f.chance[i] ==2)
                {
                    save_f.ft[i] = "第二次答对";
                }
                if (save_f.chance[i] == 1)
                {
                    save_f.ft[i] = "第三次答对";
                }
                if (save_f.chance[i] <= 0)
                {
                    save_f.ft[i] = "错误";
                }
            }
            for(int i=0;i<10;i++)
            {
                listBox1.Items.Add("第"+Convert.ToString(i +1)+"题  "+Convert.ToString(save_f.frist_save[i]) + Convert.ToString(save_f.operater[i]) + Convert.ToString(save_f.sec_save[i]) + "=" + Convert.ToString(save_f.sum_save[i])+"   "+Convert.ToString(save_f.ft[i]));
            }
               // infor = Convert.ToString(save_f.frist_save[i]) + Convert.ToString(save_f.operater[i]) + Convert.ToString(save_f.sec_save[i]) + "=" + Convert.ToString(save_f.sum_save[i]) + "\n";
        }
    }
}
