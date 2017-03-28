using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Maze
{

    class Node //节点类
    {
        public char flag; // 标识位
        public int r; //行坐标
        public int c; //列坐标
        public int father_r; //父节点的行坐标
        public int father_c; //父节点的列坐标



    }


    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
