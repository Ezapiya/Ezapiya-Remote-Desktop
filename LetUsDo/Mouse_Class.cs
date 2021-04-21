using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace LetUsDo
{
    class Mouse_Class
    {
        [Flags]
        public enum MouseEventFlags : uint
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
            WHEEL = 0x00000800,
            XDOWN = 0x00000080,
            XUP = 0x00000100
        }
        public int orignal_width=800;
        public int orignal_height=600;
        public enum MouseEventDataXButtons : uint
        {
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002
        }
        [DllImport("user32.dll")]
       // static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);
          static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);
        public Mouse_Class()
        { }
        public void mouse_move(int x_point, int y_point)
        {
            Cursor.Position = new Point(x_point, y_point);
        }
        public void mouse_wheel_up(int x_point, int y_point)
        {
            Cursor.Position = new Point(x_point, y_point);

            Point pt = Cursor.Position;
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);

            mouse_event((uint)MouseEventFlags.WHEEL, x, y, 120, 0);

        }
        public void mouse_wheel_down(int x_point, int y_point)
        {
            Cursor.Position = new Point(x_point, y_point);
            Point pt = Cursor.Position;
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);

            mouse_event((uint)MouseEventFlags.WHEEL, x, y, -120, 0);

        }
        public void mouse_click(int x_point, int y_point)
        {
            Cursor.Position = new Point(x_point, y_point);

            Point pt = Cursor.Position;
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                    MouseEventFlags.LEFTDOWN | MouseEventFlags.LEFTUP),
                x, y, 0, 0);

        }
        public void mouse_right_click(int x_point, int y_point)
        {
            Cursor.Position = new Point(x_point, y_point);
            Point pt = Cursor.Position;
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                    MouseEventFlags.RIGHTDOWN | MouseEventFlags.RIGHTUP),
                x, y, 0, 0);
        }
        public void mouse_double_click(int x_point, int y_point)
        {
            Cursor.Position = new Point(x_point, y_point);
            Point pt = Cursor.Position;
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                    MouseEventFlags.LEFTDOWN | MouseEventFlags.LEFTUP),
                x, y, 0, 0);
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                    MouseEventFlags.LEFTDOWN | MouseEventFlags.LEFTUP),
                x, y, 0, 0);
        }
        public void mouse_left_down(int x_point, int y_point)
        {
            Cursor.Position = new Point(x_point, y_point);
            Point pt = Cursor.Position;
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                    MouseEventFlags.LEFTDOWN),
                x, y, 0, 0);
        }
        public void mouse_left_up(int x_point, int y_point)
        {
            Cursor.Position = new Point(x_point, y_point);
            Point pt = Cursor.Position;
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                    MouseEventFlags.LEFTUP),
                x, y, 0, 0);
        }
        public void mouse_right_down(int x_point, int y_point)
        {
            Cursor.Position = new Point(x_point, y_point);
            Point pt = Cursor.Position;
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                    MouseEventFlags.RIGHTDOWN),
                x, y, 0, 0);
        }
        public void mouse_right_up(int x_point, int y_point)
        {
            Cursor.Position = new Point(x_point, y_point);
            Point pt = Cursor.Position;
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                    MouseEventFlags.RIGHTUP),
                x, y, 0, 0);

        }


        public void process_mouse(int mouse_event, int x_point, int y_point)
        {
            x_point = get_actual_position(Globle_data.LogicalScreenWidth, Globle_data.PhysicalScreenWidth, x_point);
            y_point = get_actual_position(Globle_data.LogicalScreenHeight, Globle_data.PhysicalScreenHeight, y_point);
            switch (mouse_event)
            {
                case 1:
                    //left click
                    mouse_click(x_point, y_point);
                    break;
                case 2:
                    // right click
                    mouse_right_click(x_point, y_point);
                    break;
                case 3:
                    // mouse move
                    mouse_move(x_point, y_point);
                    break;
                case 4:
                    // left mouse down
                    mouse_left_down(x_point, y_point);
                    break;
                case 5:
                    // right mouse down
                    mouse_right_down(x_point, y_point);
                    break;
                case 6:
                    // left mouse down
                    mouse_left_up(x_point, y_point);
                    break;
                case 7:
                    // right mouse down
                    mouse_right_up(x_point, y_point);
                    break;
                case 8:

                    mouse_wheel_up(x_point, y_point);
                    break;
                case 9:

                    mouse_wheel_down(x_point, y_point);
                    break;
            }

            //
        }
        public int get_actual_position(int orignal, int pic, int m)
        {
            int actual = 0;
            actual = (orignal * m) / pic;
            return actual;
        }
    }
}
