using LdcardUDrv;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            T2();
            //Console.WriteLine("读卡程序已经启动，请将卡片放在读卡器上... ...");
            //T1 t1 = new T1();
            //while (true)
            //{
            //    var data = t1.ReadCard();
            //    Thread.Sleep(500);
            //}
            //Console.ReadLine();
        }


        static void T2()
        {
            while (true)
            {
                IntPtr mwh = WinApiHelper.FindWindow(null, "向日葵远程控制");  //主窗口句柄
                WinApiHelper.SetActiveWindow(mwh);

                WinApiHelper.ShowWindow(mwh, WindowState.SW_RESTORE);
                WinApiHelper.Click(mwh, 328, 335);
                Thread.Sleep(500);
                Bitmap data = WinApiHelper.CutControlBitmap(mwh);
                data.Save(DateTime.Now.ToFileTime()+".png");
                WinApiHelper.Click(mwh, 310, 335);
                Thread.Sleep(500);
                WinApiHelper.ShowWindow(mwh, WindowState.SW_MINIMIZE);
                WinApiHelper.Click(mwh, 1, 1);
                Thread.Sleep(1000*5);
            }
        }


    }
}
