using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Maze
{




    public partial class Form2 : Form
    {

        int Num; //方形迷宫阶数
        PictureBox[,] pb; // PictureBox控件  pb是二维PictureBox类型数组的引用
        Node[,] mymaze;//Pos类,mymaze是二维Pos类型数组的引用
        //int start_r, start_c;//起点位
        string CurrentFile;//当下打开的文件
       

        public Form2()
        {


            InitializeComponent();
            label2.Text = "没有打开任何地图文件，\r\n可以编辑后直接保存创\r\n建新地图文件";

            
            CurrentFile = "";
            Num = 20; //赋值   400个节点
            pb = new PictureBox[Num, Num];//动态创建二维PictureBox数组
            mymaze = new Node[Num, Num];//动态创建二维Node数组

            for (int i = 0; i < Num; i++) //循环初始化控件
            {
                for (int j = 0; j < Num; j++)
                {
                    mymaze[i, j] = new Node();
                    pb[i, j] = new System.Windows.Forms.PictureBox();//pb[i, j]也是引用
                    pb[i, j].Location = new Point(j * 24, i * 24); //左上角定位
                    pb[i, j].Size = new System.Drawing.Size(24, 24);//长宽大小
                    pb[i, j].Click += new EventHandler(image_click);//给每个控件加一个单击事件

                     if (i == 0 || i == Num - 1 || j == 0 || j == Num - 1)
                     {
                        pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\wall1.png");//载入图片位置
                        mymaze[i, j].flag = '1';
                     
                     }
                     
                    else if (i==1&&j==1)
                     {
                         pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\s.png");//载入图片位置
                         mymaze[i, j].flag = 'S';
                     }

                     else if (i == Num - 2 && j == Num - 2)
                     
                    {
                        pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\f.png");//载入图片位置
                        mymaze[i, j].flag = 'F';
                    }

                     else
                     {
                        pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\black.png");//载入图片位置
                        mymaze[i, j].flag = '0';
                     }
                       
                    this.Controls.Add(pb[i, j]);//把控件添加到当前窗体
                }

            }

        }

        //单击事件
        private void image_click(object sender, EventArgs e)
        {
            PictureBox pi = (PictureBox)sender;
            Point p = pi.Location;
            int x = p.X / 24;
            int y = p.Y / 24;

            //判断是不是在编辑范围内
            if (y == 0 || y == Num - 1||x==0|| x==Num-1) return;
            if (x == 1 && y == 1) return;
            if (x == Num-2 && y == Num-2) return;
            
            if(mymaze[y, x].flag == '0')
            {
//MessageBox.Show(mymaze[y, x].flag.ToString());
             pb[y, x].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\wall1.png");//载入图片位置
             mymaze[y, x].flag = '1';
             
            }

            else // mymaze[y, x].flag == '1'
            {

                pb[y, x].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\black.png");//载入图片位置
                mymaze[y, x].flag = '0';
                
            }

          //  


        }
        
        //退出按钮
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
   
        //打开
        private void button3_Click(object sender, EventArgs e)
        {



            //如果点确定按钮
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {



            string resultFile = "";//得到的文件名

            //  openFileDialog的初始目录
            DirectoryInfo info = new DirectoryInfo(Application.StartupPath);
            String path = info.Parent.Parent.FullName + "\\map";
            openFileDialog1.InitialDirectory = (path);




                resultFile = openFileDialog1.FileName;
                CurrentFile = resultFile;
                //获取文件名，不带路径
                string fileName = resultFile.Substring(resultFile.LastIndexOf("\\") + 1);
                // 显示文件名
                label2.Text = "地图文件：" + fileName;

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

                        }

                        if (bydata[j] == 'F')
                        {
                            pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\f.png");
                            mymaze[i, j] = new Node();
                            mymaze[i, j].flag = 'F';
                        }

                        if (bydata[j] == '0')
                        {
                            pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\black.png");
                            mymaze[i, j] = new Node();
                            mymaze[i, j].flag = '0';
                        }

                        if (bydata[j] == '1')
                        {
                            pb[i, j].Image = Image.FromFile(Application.StartupPath + @"\..\..\pic\wall1.png");
                            mymaze[i, j] = new Node();
                            mymaze[i, j].flag = '1';
                        }

                    }
                }

                File.Close();//关闭文件
            }

        }
        // 保存
        private void button5_Click(object sender, EventArgs e)
        {
            if (CurrentFile != "") //有打开过文件的情况
            {
                //直接保存到CurrentFile
                FileStream File = new FileStream(CurrentFile, FileMode.Create);
                File.Seek(0, SeekOrigin.Begin);//定位到文件起点
                byte[] bydata = new byte[Num + 2];//2是因为回车符两个字节

                for (int i = 0; i < Num; i++)  //循环写文件
                {
                    for (int j = 0; j < Num; j++)
                    {
                        bydata[j] = (byte)mymaze[i, j].flag;

                    }
                    bydata[Num] = (byte)'\r';//第Num个
                    bydata[Num + 1] = (byte)'\n';//第Num+1个
                    File.Write(bydata, 0, Num + 2);
                }
                File.Close();//关闭文件
                MessageBox.Show("保存成功");



            }
            else //没有打开过文件的情况
            {

                saveFileDialog1.Filter = "地图文件（*.txt）|*.txt";
                //  saveFileDialog的初始目录
                DirectoryInfo info = new DirectoryInfo(Application.StartupPath);
                String path = info.Parent.Parent.FullName +"\\map";
                saveFileDialog1.InitialDirectory = (path);
                
                //如果点保存
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string localFilePath = saveFileDialog1.FileName.ToString(); //获得文件路径
                    CurrentFile = localFilePath;

                    //获取文件名，不带路径
                   string   fileName = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);
                   // 显示文件名
                    label2.Text = "地图文件：" + fileName;
                   

                    //如果文件不存在新建，如果文件存在清空
                    FileStream File = new FileStream(CurrentFile,FileMode.Create);
                    File.Seek(0, SeekOrigin.Begin);//定位到文件起点
                    byte[] bydata = new byte[Num + 2];//2是因为回车符两个字节

                    for (int i = 0; i < Num; i++)  //循环写文件
                    {
                        for (int j = 0; j < Num; j++)
                        {
                            bydata[j] =(byte)mymaze[i, j].flag;
                           
                        }
                        bydata[Num] = (byte)'\r';//第Num个
                        bydata[Num + 1] = (byte)'\n';//第Num+1个
                          File.Write(bydata, 0, Num + 2);
                    }
                    File.Close();//关闭文件
                    MessageBox.Show("保存成功");

                }

            }

        }


        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }


    }
}
