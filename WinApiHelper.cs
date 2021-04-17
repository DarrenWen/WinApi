using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

    public class WinApiHelper
    {
        //寻找目标进程窗口       
        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("USER32.DLL", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);
        //设置进程窗口到最前       
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        //模拟键盘事件         
        [DllImport("USER32.DLL")]
        public static extern void keybd_event(Byte bVk, Byte bScan, Int32 dwFlags, Int32 dwExtraInfo);
        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="cButtons">0</param>
        /// <param name="dwExtraInfo">0</param>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(MouseEventFlag dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        /// <summary>
        /// 设置光标位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        /// <summary>
        /// 设置活动窗口
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetActiveWindow")]
        public static extern int SetActiveWindow(
    IntPtr hwnd
);
        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        public static extern int ShowWindow(
    IntPtr hwnd,
    WindowState nCmdShow
);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, int hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);

    /// <summary>
    /// 鼠标点击事件，在指定句柄范围内
    /// </summary>
    /// <param name="winCotPtr">句柄Id</param>
    /// <param name="xoffset">水平偏移</param>
    /// <param name="yoffset">垂直偏移</param>
    /// 向日葵默认偏移:325*330
    public static void Click(IntPtr winCotPtr,int xoffset=0,int yoffset=0)
    {
        SetWindowPos(winCotPtr,-1, 0, 0, 0, 0, 1 | 2);
        // 前置要点击的程序
        SetForegroundWindow(winCotPtr);
        RECT rECT = new RECT();
        GetWindowRect(winCotPtr, ref rECT);
        SetCursorPos(rECT.Left + xoffset, rECT.Top + yoffset);
        mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, 0);
        mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, 0);
    }

        #region 使用api32 根据窗体句柄进行截图

        /// <summary>
        /// 此方法仅仅适用于设计器画在pan上的
        /// </summary>
        /// <param name="cot"></param>
        /// <returns></returns>
        //public Bitmap CutControlBitmap(Control cot)
        //{
        //    Bitmap bmp = new Bitmap(cot.Width, cot.Height);
        //    cot.DrawToBitmap(bmp, cot.ClientRectangle);
        //    return bmp;
        //}


        ///截图
        public static Bitmap CutControlBitmap(IntPtr winCotPtr)
        {
            //1先获取控件的大小
            IntPtr hscrdc = GetWindowDC(winCotPtr);
            RECT rECT = new RECT();
            GetWindowRect(winCotPtr, ref rECT);
            IntPtr mapPtr = CreateCompatibleBitmap(hscrdc, rECT.Right - rECT.Left, rECT.Bottom - rECT.Top);
            IntPtr hmemdc = CreateCompatibleDC(hscrdc);
            SelectObject(hmemdc, mapPtr);
            PrintWindow(winCotPtr, hmemdc, 0);
            Bitmap bmp = Bitmap.FromHbitmap(mapPtr);
            DeleteObject(mapPtr);
            DeleteDC(hscrdc);
            DeleteDC(hmemdc);
            return bmp;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, UInt32 nFlags);

        [DllImport("gdi32.dll")]
        public static extern IntPtr DeleteObject(IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern int DeleteDC(IntPtr hdc);
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion
    }

////声明:
//SetWindowPos(
//  hWnd: HWND;
//{ 窗口句柄}
//hWndInsertAfter: HWND;
//{ 窗口的 Z 顺序}
//X, Y: Integer;
//{ 位置}
//cx, cy: Integer;
//{ 大小}
//uFlags: UINT           { 选项}
//): BOOL;

////hWndInsertAfter 参数可选值:
//HWND_TOP = 0;
//{ 在前面}
//HWND_BOTTOM = 1;
//{ 在后面}
//HWND_TOPMOST = HWND(-1);
//{ 在前面, 位于任何顶部窗口的前面}
//HWND_NOTOPMOST = HWND(-2);
//{ 在前面, 位于其他顶部窗口的后面}

////uFlags 参数可选值:
//SWP_NOSIZE = 1;
//{ 忽略 cx、cy, 保持大小}
//SWP_NOMOVE = 2;
//{ 忽略 X、Y, 不改变位置}
//SWP_NOZORDER = 4;
//{ 忽略 hWndInsertAfter, 保持 Z 顺序}
//SWP_NOREDRAW = 8;
//{ 不重绘}
//SWP_NOACTIVATE = $10;
//{ 不激活}
//SWP_FRAMECHANGED = $20;
//{ 强制发送 WM_NCCALCSIZE 消息, 一般只是在改变大小时才发送此消息}
//SWP_SHOWWINDOW = $40;
//{ 显示窗口}
//SWP_HIDEWINDOW = $80;
//{ 隐藏窗口}
//SWP_NOCOPYBITS = $100;
//{ 丢弃客户区}
//SWP_NOOWNERZORDER = $200;
//{ 忽略 hWndInsertAfter, 不改变 Z 序列的所有者}
//SWP_NOSENDCHANGING = $400;
//{ 不发出 WM_WINDOWPOSCHANGING 消息}
//SWP_DRAWFRAME = SWP_FRAMECHANGED;
//{ 画边框}
//SWP_NOREPOSITION = SWP_NOOWNERZORDER;
//{ }
//SWP_DEFERERASE = $2000;
//{ 防止产生 WM_SYNCPAINT 消息}
//SWP_ASYNCWINDOWPOS = $4000;
//{ 若调用进程不拥有窗口, 系统会向拥有窗口的线程发出需求}

public enum MouseEventFlag : uint
{
    Move = 0x0001,
    LeftDown = 0x0002,
    LeftUp = 0x0004,
    RightDown = 0x0008,
    RightUp = 0x0010,
    MiddleDown = 0x0020,
    MiddleUp = 0x0040,
    XDown = 0x0080,
    XUp = 0x0100,
    Wheel = 0x0800,
    VirtualDesk = 0x4000,
    Absolute = 0x8000
}

public enum WindowState : int
{
    //API 常數定義  
    SW_HIDE = 0,
    SW_NORMAL = 1, //正常弹出窗体  
    SW_SHOWMINIMIZED = 2,//激活窗口并将其最小化。
    SW_MAXIMIZE = 3, //最大化弹出窗体  
    SW_SHOWNOACTIVATE = 4,//以窗口最近一次的大小和状态显示窗口。激活窗口仍然维持激活状态。
    SW_SHOW = 5,//在窗口原来的位置以原来的尺寸激活和显示窗口。
    SW_MINIMIZE = 6,//最小化指定的窗口并且激活在Z序中的下一个顶层窗口。
    SW_SHOWMINNOACTIVE = 7,//以窗口最近一次的大小和状态显示窗口。激活窗口仍然维持激活状态
    SW_SHOWNA = 8,//以窗口原来的状态显示窗口。激活窗口仍然维持激活状态。
    SW_RESTORE = 9,//激活并显示窗口。如果窗口最小化或最大化，则系统将窗口恢复到原来的尺寸和位置。在恢复最小化窗口时，应用程序应该指定这个标志。
    SW_SHOWDEFAULT = 10//依据在STARTUPINFO结构中指定的SW_FLAG标志设定显示状态，STARTUPINFO 结构是由启动应用程序的程序传递给CreateProcess函数的。
}