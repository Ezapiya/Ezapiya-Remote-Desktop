using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Net;

namespace LetUsDo
{
    public partial class Blackscreen : Form
    {

        public Image bitmap_image = null;
        private int icount;

        #region  veriable and import

        public const int WS_OVERLAPPED = 0x0;
        public const int WS_THICKFRAME = 0x40000;
        public const int WS_BORDER = 0x800000;
        public const int WS_POPUP = unchecked((int)0x80000000);
        public const int WS_CHILD = 0x40000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_CLIPCHILDREN = 0x2000000;
        public const int WS_EX_TOPMOST = 0x8;
        public const int WS_EX_LAYERED = 0x80000;
        public const int WS_EX_TRANSPARENT = 0x20;
        public const int WS_EX_TOOLWINDOW = 0x80;
        public const int LWA_ALPHA = 0x0;
        public const int MS_SHOWMAGNIFIEDCURSOR = 0x10001;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW")]
        public extern static IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        public const int SWP_NOREDRAW = 0x8;
        public const int SWP_FRAMECHANGED = 0x20;
        public const int SWP_NOCOPYBITS = 0x100;
        public const int SWP_NOOWNERZORDER = 0x200;
        public const int SWP_NOSENDCHANGING = 0x400;
        public const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;
        public const int SWP_DEFERERASE = 0x2000;
        public const int SWP_ASYNCWINDOWPOS = 0x4000;
        public const int SWP_HIDEWINDOW = 0x80;

        [DllImport("User32.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        public extern static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        public delegate IntPtr WindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct WNDCLASSEX
        {
            public uint cbSize;
            public uint style;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public WindowProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [DllImport("User32.dll", SetLastError = true)]
        public extern static short RegisterClassEx([In()] ref WNDCLASSEX lpwcx);

        [DllImport("User32.dll", SetLastError = true)]
        public extern static IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true)]
        public extern static bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("User32.dll", SetLastError = true)]
        public extern static IntPtr GetDesktopWindow();


        [DllImport("Magnification.dll")]
        public extern static bool MagInitialize();

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("Magnification.dll")]
        public extern static bool MagSetWindowSource(IntPtr hWnd, IntPtr rect);

        [DllImport("Magnification.dll")]
        public extern static bool MagSetWindowSource(IntPtr hWnd, [In(), MarshalAs(UnmanagedType.Struct)] RECT rect);

        [DllImport("Magnification.dll")]
        public extern static bool MagSetWindowFilterList(IntPtr hwnd, int dwFilterMode, int count, ref IntPtr pHWND);

        [DllImport("Magnification.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public extern static bool MagSetImageScalingCallback(IntPtr hwnd, MagImageScalingCallback MagImageScalingCallback);


        public delegate IntPtr MagImageScalingCallback(IntPtr hwnd, IntPtr srcdata, ref MAGIMAGEHEADER srcheader, IntPtr destdata, MAGIMAGEHEADER destheader, IntPtr unclipped, IntPtr clipped, IntPtr dirty);

        [StructLayout(LayoutKind.Sequential)]
        public struct MAGIMAGEHEADER
        {
            public int width;
            public int height;
            public Guid format;
            public int stride;
            public int offset;
            public int cbSize;
        }


        public static Guid GUID_WICPixelFormat32bppRGBA = new Guid("F5C7AD2D-6A8D-43DD-A7A8-A29935261AE9");

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            [MarshalAs(UnmanagedType.I4)]
            public int biSize;
            [MarshalAs(UnmanagedType.I4)]
            public int biWidth;
            [MarshalAs(UnmanagedType.I4)]
            public int biHeight;
            [MarshalAs(UnmanagedType.I2)]
            public short biPlanes;
            [MarshalAs(UnmanagedType.I2)]
            public short biBitCount;
            [MarshalAs(UnmanagedType.I4)]
            public int biCompression;
            [MarshalAs(UnmanagedType.I4)]
            public int biSizeImage;
            [MarshalAs(UnmanagedType.I4)]
            public int biXPelsPerMeter;
            [MarshalAs(UnmanagedType.I4)]
            public int biYPelsPerMeter;
            [MarshalAs(UnmanagedType.I4)]
            public int biClrUsed;
            [MarshalAs(UnmanagedType.I4)]
            public int biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            [MarshalAs(UnmanagedType.Struct, SizeConst = 40)]
            public BITMAPINFOHEADER bmiHeader;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public int[] bmiColors;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO_FLAT
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public byte[] bmiColors;
        }

        public const int BI_RGB = 0;
        public const int BI_RLE8 = 1;
        public const int BI_RLE4 = 2;
        public const int BI_BITFIELDS = 3;
        public const int BI_JPEG = 4;
        public const int BI_PNG = 5;

        public const int DIB_RGB_COLORS = 0;
        public const int DIB_PAL_COLORS = 1;

        public const int CBM_INIT = 0x4;

        [DllImport("User32.dll", SetLastError = true)]
        private extern static IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32.dll", SetLastError = true)]
        private extern static IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("User32.dll", SetLastError = true)]
        private extern static IntPtr ReleaseDC(IntPtr hWnd, IntPtr hdc);

        [DllImport("Gdi32.dll", SetLastError = true)]
        public extern static bool DeleteDC(IntPtr hDC);

        [DllImport("Gdi32.dll", SetLastError = true)]
        public extern static IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("Gdi32.dll", SetLastError = true)]
        private extern static IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("Gdi32.dll", SetLastError = true)]
        public extern static bool DeleteObject([In()] IntPtr hObject);

        public const int SRCCOPY = 0xCC0020;

        [DllImport("Gdi32.dll", SetLastError = true)]
        public extern static bool BitBlt(IntPtr hDCDest, int XOriginDest, int YOriginDest, int WidthDest, int HeightDest, IntPtr hDCSrc, int XOriginScr, int YOriginSrc, int dwRop);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public extern static int StretchDIBits(IntPtr hdc, int XDest, int YDest, int nDestWidth, int nDestHeight, int XSrc, int YSrc, int nSrcWidth, int nSrcHeight, IntPtr lpBits, ref BITMAPINFO_FLAT lpBitsInfo, int iUsage, int dwRop);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public extern static IntPtr CreateDIBitmap(IntPtr hdc, ref BITMAPINFOHEADER lpbmih, int fdwInit, IntPtr lpbInit, ref BITMAPINFO lpBitsInfo, UInt32 fuUsage);

        public const int MW_FILTERMODE_EXCLUDE = 0;
        public const int MW_FILTERMODE_INCLUDE = 1;
        #endregion


        #region MagImageScaling

        public static IntPtr MagImageScaling(IntPtr hwnd, IntPtr srcdata, ref MAGIMAGEHEADER srcheader, IntPtr destdata, MAGIMAGEHEADER destheader, IntPtr unclipped, IntPtr clipped, IntPtr dirty)
        {
            /* Graphics g = Graphics.FromHwnd(IntPtr.Zero);
             IntPtr desktop = g.GetHdc();
             int LogicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenWidth);
             int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenHeight);
             int PhysicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenWidth);
             int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenHeight);
             */

            BITMAPINFO lpbmi = new BITMAPINFO();
            lpbmi.bmiHeader.biSize = Marshal.SizeOf(lpbmi.bmiHeader);

            lpbmi.bmiHeader.biHeight = Screen.PrimaryScreen.Bounds.Height;
            lpbmi.bmiHeader.biWidth = Screen.PrimaryScreen.Bounds.Width;
            lpbmi.bmiHeader.biSizeImage = (int)(Screen.PrimaryScreen.Bounds.Height * Screen.PrimaryScreen.Bounds.Width);

            /*  lpbmi.bmiHeader.biHeight = PhysicalScreenHeight;
              lpbmi.bmiHeader.biWidth = PhysicalScreenWidth;
              lpbmi.bmiHeader.biSizeImage = (int)(PhysicalScreenHeight * PhysicalScreenWidth);*/


            lpbmi.bmiHeader.biPlanes = 1;
            lpbmi.bmiHeader.biBitCount = 32;
            lpbmi.bmiHeader.biCompression = BI_RGB;
            IntPtr hDC = GetWindowDC(hwnd);


            hBitmap = CreateDIBitmap(hDC, ref lpbmi.bmiHeader, CBM_INIT, srcdata, ref lpbmi, (UInt32)DIB_RGB_COLORS);
            bCallbackDone = true;
            DeleteDC(hDC);

            //INSTANT C# NOTE: Inserted the following 'return' since all code paths must return a value in C#:
            return System.IntPtr.Zero;
        }


        private IntPtr hWndMag = IntPtr.Zero;
        private IntPtr hWndHost = IntPtr.Zero;
        //Private Shared wndProc As WndProc = New WndProc(AddressOf HostWndProc)
        private int nWidth = 0;
        private int nHeight = 0;
        private string sFilename = string.Empty;
        private System.Drawing.Imaging.ImageFormat imageformat;
        private static bool bCallbackDone = false;
        private static IntPtr hBitmap = IntPtr.Zero;

        public static IntPtr HostWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            return DefWindowProc(hWnd, msg, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DLGTEMPLATE
        {
            public int style;
            public int dwExtendedStyle;
            public ushort cdit;
            public short x;
            public short y;
            public short cx;
            public short cy;
        }
        //
        private int InitialStyle;
        private decimal PercentVisible;

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowLongA", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        public static extern int GetWindowLong(System.IntPtr hwnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetWindowLongA", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        public static extern int SetWindowLong(System.IntPtr hwnd, int nIndex, int dwNewLong);










        private struct KBDLLHOOKSTRUCT
        {
            public Keys key;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr extra;
        }
        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "SetWindowPos", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "FindWindowA", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int FindWindow(string lpClassName, string lpWindowName);
        // private const int SWP_HIDEWINDOW1 = 0x80;
        private const int SWP_SHOWWINDOW = 0x40;
        private int taskBar;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private extern static IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private extern static bool UnhookWindowsHookEx(IntPtr hook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private extern static IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wp, IntPtr lp);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private extern static IntPtr GetModuleHandle(string name);
        private IntPtr ptrHook;
        private LowLevelKeyboardProc objKeyboardProcess;


        public bool allowToExit = false;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private extern static IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public extern static short GetAsyncKeyState(System.Windows.Forms.Keys vKey);




        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            LogicalScreenWidth = 8,
            LogicalScreenHeight = 10,
            PhysicalScreenHeight = 117,
            PhysicalScreenWidth = 118,
            LOGPIXELSY = 90,
        }
        #endregion




        public Blackscreen()
        {
            try
            {
                ProcessModule objCurrentModule = Process.GetCurrentProcess().MainModule;
                objKeyboardProcess = new LowLevelKeyboardProc(captureKey);
                ptrHook = SetWindowsHookEx(13, objKeyboardProcess, GetModuleHandle(objCurrentModule.ModuleName), 0);

            }
            catch (Exception ex)
            {
            }

            InitializeComponent();
        }

        private IntPtr captureKey(int nCode, IntPtr wp, IntPtr lp)
        {
            try
            {
                if (nCode >= 0)
                {
                    KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lp, typeof(KBDLLHOOKSTRUCT));
                    if (objKeyInfo.key == Keys.Alt)
                    {
                        return (IntPtr)1;
                    }

                    if (objKeyInfo.key == Keys.RWin || objKeyInfo.key == Keys.LWin)
                    {
                        Debug.Write(SetWindowPos(taskBar, (int)0L, (int)0L, (int)0L, (int)0L, (int)0L, SWP_SHOWWINDOW));
                        this.ShowInTaskbar = true;
                        this.ShowIcon = true;
                        this.WindowState = FormWindowState.Minimized;
                    }
                    if (objKeyInfo.key == Keys.ControlKey)
                    {
                        return (IntPtr)1;
                    }
                    if (objKeyInfo.key == Keys.Escape)
                    {
                        return (IntPtr)1;
                    }

                }
                return CallNextHookEx(ptrHook, nCode, wp, lp);
            }
            catch (Exception ex)
            {

            }
            //INSTANT C# NOTE: Inserted the following 'return' since all code paths must return a value in C#:
            return System.IntPtr.Zero;
        }


        //INSTANT C# NOTE: These were formerly VB static local variables:
        private WindowProc Form1_Load__wndProc = new WindowProc(HostWndProc);
        private MagImageScalingCallback Form1_Load_MagImageScalingProc = new MagImageScalingCallback(MagImageScaling);


        private void Blackscreen_Load(object sender, EventArgs e)
        {
            /* Graphics g = Graphics.FromHwnd(IntPtr.Zero);
             IntPtr desktop = g.GetHdc();
             int LogicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenWidth);
             int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.LogicalScreenHeight);
             int PhysicalScreenWidth = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenWidth);
             int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.PhysicalScreenHeight);

            

             nWidth = PhysicalScreenWidth;
             nHeight = PhysicalScreenHeight;*/

            nWidth = Screen.PrimaryScreen.Bounds.Width;
            nHeight = Screen.PrimaryScreen.Bounds.Height;


            //INSTANT C# NOTE: VB local static variable moved to class level:
            //		Static _wndProc As WindowProc = new WindowProc(AddressOf HostWndProc)
            string sWindowClassName = "MagnifierHost";
            WNDCLASSEX wcex = new WNDCLASSEX();
            wcex.cbSize = Convert.ToUInt32(Marshal.SizeOf(wcex));
            wcex.lpfnWndProc = Form1_Load__wndProc;
            wcex.hInstance = IntPtr.Zero;
            wcex.lpszClassName = sWindowClassName;
            wcex.hCursor = IntPtr.Zero;
            wcex.hbrBackground = IntPtr.Zero;
            wcex.style = 0;
            wcex.cbClsExtra = 0;
            wcex.cbWndExtra = 0;
            wcex.hIcon = IntPtr.Zero;
            wcex.lpszMenuName = null;
            wcex.hIconSm = IntPtr.Zero;
            if (RegisterClassEx(ref wcex) != 0)
            {
                hWndHost = CreateWindowEx(WS_EX_TOPMOST | WS_EX_LAYERED | WS_EX_TRANSPARENT, sWindowClassName, "Host Window", WS_POPUP | WS_CLIPCHILDREN, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, wcex.hInstance, IntPtr.Zero);

                // hWndHost = CreateWindowEx(WS_EX_TOPMOST | WS_EX_LAYERED, sWindowClassName, "Host Window", WS_POPUP | WS_CLIPCHILDREN, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, wcex.hInstance, IntPtr.Zero);


                if (hWndHost != IntPtr.Zero)
                {
                    /////////////////////////////////////
                    SetWindowPos(hWndHost, IntPtr.Zero, 0, 0, nWidth, nHeight, SWP_HIDEWINDOW);
                    bool bRet = SetLayeredWindowAttributes(hWndHost, 0, 0xFF, LWA_ALPHA);
                    if (MagInitialize())
                    {
                        hWndMag = CreateWindowEx(0, "Magnifier", "MagnifierWindow", WS_CHILD | MS_SHOWMAGNIFIEDCURSOR | WS_VISIBLE, 0, 0, nWidth, nHeight, hWndHost, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                        if (hWndMag != IntPtr.Zero)
                        {
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                        //INSTANT C# NOTE: VB local static variable moved to class level:
                        //					Static MagImageScalingProc As MagImageScalingCallback = new MagImageScalingCallback(AddressOf MagImageScaling)
                        if (MagSetImageScalingCallback(hWndMag, Form1_Load_MagImageScalingProc) != false)
                        {

                        }
                    }
                }
            }

            icount = 0;

            //'''

            InitialStyle = GetWindowLong(this.Handle, -20);
            PercentVisible = 1;
            SetWindowLong(this.Handle, -20, InitialStyle | 0x80000 | 0x20);
            //SetLayeredWindowAttributes(this.Handle, 0, 255 * PercentVisible, 0x2);

            SetLayeredWindowAttributes(this.Handle, 0, 255 * 1, 0x2);

            //Me.BackColor = Color.Green
            this.TopMost = true;
            ///



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IntPtr pWnd = this.Handle;
            if (MagSetWindowFilterList(hWndMag, MW_FILTERMODE_EXCLUDE, 1, ref pWnd) != false)
            {
                //this.Hide();
                RECT sourceRect = new RECT();
                sourceRect.left = 0;
                sourceRect.top = 0;
                sourceRect.right = nWidth;
                sourceRect.bottom = nHeight;


                // sourceRect.right = 500;
                // sourceRect.bottom = 500;

                bCallbackDone = false;
                if (MagSetWindowSource(hWndMag, sourceRect) == true)
                {

                    Cursor = Cursors.WaitCursor;
                    while (bCallbackDone == false)
                    {
                    }
                    Cursor = Cursors.Default;
                    //Cursor.Hide();
                    Image img = Image.FromHbitmap(hBitmap);
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    // img.Save("C:\\temp\\" + Convert.ToString(this.icount) + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                    bitmap_image = img;
                    DeleteObject(hBitmap);
                    this.icount = this.icount + 1;
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!(GetForegroundWindow() == this.Handle))
            {
                this.TopMost = true;
            }
            this.TopMost = true;
            this.Left = 0;
            this.Top = 0;
           this.WindowState = FormWindowState.Maximized;


            SetWindowLong(this.Handle, -20, InitialStyle | 0x80000 | 0x20);
            SetLayeredWindowAttributes(this.Handle, 0, 255 * 1, 0x2);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
