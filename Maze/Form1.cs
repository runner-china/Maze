using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace Maze
{

    public partial class Form1 : Form
    {
        //int test;
        bool Loadmap;//是否载入地图的标记
        int Num; //方形迷宫阶数
        PictureBox[,] pb; // PictureBox控件  pb是二维PictureBox类型数组的引用
        Node[,] mymaze;//mymaze是二维Node类型数组的引用
        Node MazeStart;//记录起点位
        int mytimer;//计时器
        bool first;//载入某张地图后第一次运行搜索算法



        public Form1()
        {

            
            InitializeComponent();

            //timer1.Enabled= false;//定时器控件
            Loadmap = false;
           // first = true;

            Num = 20; //赋值   400个节点
            pb = new PictureBox[Num, Num];//动态创建二维PictureBox数组
            mymaze = new Node[Num, Num];//动态创建二维Node数组
            MazeStart = new Node();
            mytimer = 200;
           


            for (int i = 0; i < Num; i++) //循环初始化控件
            {
                for (int j = 0; j < Num; j++)
                {
                    pb[i, j] = new System.Windows.Forms.PictureBox();//pb[i, j]也是引用
                    pb[i, j].Location = new Point(j * 24, i * 24); //左上角定位
                    pb[i, j].Size = new System.Drawing.Size(24, 24);//长宽大小
                    // this.Controls.Add(pb[i, j]);//把控件添加到当前窗体
                }

            }
           
        }





        //载入迷宫按钮
        private void button1_Click(object sender, EventArgs e)
        {



            //如果点确定按钮
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {


            Loadmap = true;
            first = true;
            
            string resultFile = "";//得到的文件名

            //  openFileDialog的初始目录
            DirectoryInfo info = new DirectoryInfo(Application.StartupPath);
            String path = info.Parent.Parent.FullName + "\\map";
            openFileDialog1.InitialDirectory = (path);



                pictureBox_black.Hide();//隐藏背景黑色图
                resultFile = openFileDialog1.FileName;

                

                byte[] bydata = new byte[Num + 2];//2是因为回车符两个字节
                FileStream File = new FileStream(resultFile, FileMode.Open);//打开文件
                File.Seek(0, SeekOrigin.Begin);//定位到文件起点
                for (int i = 0; i < Num; i++)  //循环读取文件
                {
                    File.Read(bydata, 0, Num + 2);

                    for (int j = 0; j < Num; j++)
                    {
                        if (bydata[j] == 'S')//起点标识符
                        {
                            //载入对应的图片
                            pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\s.png");
                            mymaze[i, j] = new Node();// mymaze[i, j] 也是引用
                            mymaze[i, j].flag = 'S';//打标识符
                            mymaze[i, j].r = i;
                            mymaze[i, j].c = j;
                            mymaze[i, j].father_r = 0;
                            mymaze[i, j].father_c = 0;
                            MazeStart.r = i;//标识起点位
                            MazeStart.c = j;
      
                            
                        }

                        if (bydata[j] == 'F')
                        {
                            pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\f.png");
                            mymaze[i, j] = new Node();
                            mymaze[i, j].flag = 'F';
                            mymaze[i, j].r = i;
                            mymaze[i, j].c = j;
                            mymaze[i, j].father_r = 0;
                            mymaze[i, j].father_c = 0;
                        }

                        if (bydata[j] == '0')
                        {
                            pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\black.png");
                            mymaze[i, j] = new Node();
                            mymaze[i, j].flag = '0';
                            mymaze[i, j].r = i;
                            mymaze[i, j].c = j;
                            mymaze[i, j].father_r = 0;
                            mymaze[i, j].father_c = 0;
                        }

                        if (bydata[j] == '1')
                        {
                            pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\wall1.png");
                            mymaze[i, j] = new Node();
                            mymaze[i, j].flag = '1';
                            mymaze[i, j].r = i;
                            mymaze[i, j].c = j;
                            mymaze[i, j].father_r = 0;
                            mymaze[i, j].father_c = 0;
                        }
                        this.Controls.Add(pb[i, j]);//把控件添加到当前窗体
                    }
                }

                File.Close();//关闭文件
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("test");
        }



        //深度优先搜索
        private void button2_Click(object sender, EventArgs e)
        {

            if(Loadmap==false)
            {

                MessageBox.Show("请先载入地图文件", "请先载入地图文件");
                return;

            }
            //不是第一次运行情况时候的预处理。
            if(first!=true)
            {
                for (int i = 0; i < Num;i++ )
                {

                    for(int j=0;j<Num;j++)
                    {

                        mymaze[i, j].father_r = 0;
                        mymaze[i, j].father_c = 0;
                        if (mymaze[i, j].flag == '2' || mymaze[i, j].flag == '3')
                        {
                            mymaze[i, j].flag = '0';
                            pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\black.png");
                        }
                        if (mymaze[i, j].flag == '1')
                        {

                            pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\wall1.png");
                        }
                        if (mymaze[i, j].flag == 'S')
                        {
                            
                            pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\s.png");
                        }
                        if (mymaze[i, j].flag == 'F')
                        {

                            pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\f.png");
                        }


                    }



                }



                  
            }


              first = false;

            
            Stack OpenStack = new Stack();//初始化
            OpenStack.Push(mymaze[MazeStart.r, MazeStart.c]);//迷宫起点入栈
            
            //用于存放子节点的数据结构
            Node[] sons=new Node[4];
            int sons_len = 0;
            
            //bool sflag = true;//起点标记
            Node temp;
            while(true)
            {

                 temp = (Node)OpenStack.Pop();

                //起点时候
                if (temp.flag=='S')
                {
   
                    //不必修改flag

                }
                // 非起点时候
                else{  

                    //temp.flag = '3';
                    mymaze[temp.r, temp.c].flag = '3';


                    //画上一个路面的箭头（起点任然画起点）
                    if (mymaze[temp.father_r, temp.father_c].flag=='S')
                        pb[temp.father_r, temp.father_c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\s.png");

                    else if ((temp.c - temp.father_c == 1) && (temp.r - temp.father_r == 0))
                        pb[temp.father_r, temp.father_c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\right2.png");

                    else if ((temp.c - temp.father_c == 0) && (temp.r - temp.father_r == 1))
                        pb[temp.father_r, temp.father_c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\down2.png");

                    else if ((temp.c - temp.father_c == -1) && (temp.r - temp.father_r == 0))
                        pb[temp.father_r, temp.father_c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\left2.png");
                   
                    else if ((temp.c - temp.father_c == 0) && (temp.r - temp.father_r == -1))
                        pb[temp.father_r, temp.father_c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\up2.png");


                }

                //东南西北的顺序探测
                ////////////////////////////////////
                pb[temp.r, temp.c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\east.png");


                switch (mymaze[temp.r, temp.c + 1].flag)//东探索
                {
                    case 'F':
                        mymaze[temp.r, temp.c + 1].father_r = temp.r;
                        mymaze[temp.r, temp.c + 1].father_c = temp.c;
                        Form3 f3 = new Form3();
                        f3.Show();
                        //MessageBox.Show("找到终点"); 
                        return;
                    case '0':
                        mymaze[temp.r, temp.c + 1].father_r = temp.r;
                        mymaze[temp.r, temp.c + 1].father_c = temp.c;
                        mymaze[temp.r, temp.c + 1].flag = '2';
                        sons[sons_len] = mymaze[temp.r, temp.c + 1];
                        sons_len++;
                        pb[temp.r, temp.c + 1].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\gray.png");

                        //MessageBox.Show(sons_len.ToString());
                        break;
                    default: break;

                }
                int n = Environment.TickCount;//延时
                while (Environment.TickCount - n < mytimer) Application.DoEvents();
                ////////////////////////////////////
                pb[temp.r, temp.c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\south.png");

                switch (mymaze[temp.r + 1, temp.c].flag)//南探索
                {
                    case 'F':
                        mymaze[temp.r + 1, temp.c].father_r = temp.r;
                        mymaze[temp.r + 1, temp.c].father_c = temp.c;
                        Form3 f3 = new Form3();
                        f3.Show();
                        //MessageBox.Show("找到终点"); 
                        return;
                    case '0':
                        mymaze[temp.r + 1, temp.c].father_r = temp.r;
                        mymaze[temp.r + 1, temp.c].father_c = temp.c;
                        mymaze[temp.r + 1, temp.c].flag = '2';
                        sons[sons_len] = mymaze[temp.r + 1, temp.c];
                        sons_len++;
                        pb[temp.r + 1, temp.c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\gray.png");

                        //MessageBox.Show(sons[0].r.ToString());
                        break;
                    default: break;

                }

                n = Environment.TickCount;//延时
                while (Environment.TickCount - n < mytimer) Application.DoEvents();

                ////////////////////////////////////
                pb[temp.r, temp.c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\west.png");


                switch (mymaze[temp.r, temp.c - 1].flag)//西探索
                {
                    case 'F':
                        mymaze[temp.r, temp.c - 1].father_r = temp.r;
                        mymaze[temp.r, temp.c - 1].father_c = temp.c;
                        Form3 f3 = new Form3();
                        f3.Show();
                        //MessageBox.Show("找到终点"); 
                        return;
                    case '0':
                        mymaze[temp.r, temp.c - 1].father_r = temp.r;
                        mymaze[temp.r, temp.c - 1].father_c = temp.c;
                        mymaze[temp.r, temp.c - 1].flag = '2';
                        sons[sons_len] = mymaze[temp.r, temp.c - 1];
                        sons_len++;
                        pb[temp.r, temp.c - 1].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\gray.png");

                        //MessageBox.Show(sons[0].r.ToString());
                        break;
                    default: break;

                }

                n = Environment.TickCount;//延时
                while (Environment.TickCount - n < mytimer) Application.DoEvents();

                ////////////////////////////////////
                pb[temp.r, temp.c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\north.png");


                switch (mymaze[temp.r - 1, temp.c].flag)//北探索
                {
                    case 'F':
                        mymaze[temp.r - 1, temp.c].father_r = temp.r;
                        mymaze[temp.r - 1, temp.c].father_c = temp.c;
                        Form3 f3 = new Form3();
                        f3.Show();
                        //MessageBox.Show("成功找到迷宫终点！"); 
                        return;
                    case '0':
                        mymaze[temp.r - 1, temp.c].father_r = temp.r;
                        mymaze[temp.r - 1, temp.c].father_c = temp.c;
                        mymaze[temp.r - 1, temp.c].flag = '2';
                        sons[sons_len] = mymaze[temp.r - 1, temp.c];
                        sons_len++;
                        pb[temp.r - 1, temp.c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\gray.png");

                        //MessageBox.Show(sons[0].r.ToString());
                        break;
                    default: break;

                }
                n = Environment.TickCount;//延时
                while (Environment.TickCount - n < mytimer) Application.DoEvents();
                ////////////////////////////////////
                
                    if((sons_len==0)&&(OpenStack.Count==0))
                    {
                        //MessageBox.Show("找不到迷宫终点，失败！"); 
                        Form4 f4 = new Form4();
                        f4.Show();
                        return;

                    }

                   if(sons_len==0)
                   {
                        Node temp2=(Node)OpenStack.Peek();
                        //MessageBox.Show(temp2.r.ToString(), temp2.c.ToString());
                        while (true)
                        {
                        if ((temp2.father_r == temp.r) && (temp2.father_c== temp.c)) break;
                         


                        if (temp.flag == 'S')
                        {
                            pb[temp.r, temp.c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\s.png");
                        }
                        else
                        {
                            pb[temp.r, temp.c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\blue.png");
                        }

                             temp = mymaze[temp.father_r, temp.father_c];//注意temp是一个引用

                            /* int r= temp.father_r;
                             int c= temp.father_c;
                             temp.r = r;
                             temp.c = c;
                             temp.flag = mymaze[r, c].flag; //MessageBox.Show(temp.flag.ToString());
                             temp.father_r = mymaze[r, c].father_r;
                             temp.father_c = mymaze[r, c].father_c;
                             */

                             pb[temp.r, temp.c].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\south.png");
                             n = Environment.TickCount;//延时
                             while (Environment.TickCount - n < mytimer) Application.DoEvents();
                        }

                   }
                   ////////////////////////////////////

                   if (sons_len != 0)
               {

                   for (int i = sons_len - 1; i >= 0;i--)
                   {
                       //北西南东的顺序入堆栈
                       OpenStack.Push(sons[i]);

                   }

                   sons_len = 0;
               }

              

            }//大while循环 
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
    
        }


        private void button3_Click(object sender, EventArgs e)
        {

        }

        //关闭程序按钮
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //地图编辑器按钮
        private void button4_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

 
        //迷宫文本格式按钮
        private void button6_Click(object sender, EventArgs e)
        {

            if (Loadmap == false)
            {

                MessageBox.Show("请先载入地图文件", "请先载入地图文件");
                return;

            }
           
            string str = "";
            for (int i = 0; i < Num; i++)
            {

                for (int j = 0; j < Num; j++)
                {
                    str += mymaze[i, j].flag.ToString();
                    str += " ";
                }
                str += '\n';
            }
            MessageBox.Show(str);
  
        }


    }
}
