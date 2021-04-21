using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LetUsDo
{
    class KB
    {
        public KB()
        { }

        public void process_Keyboard(byte[] data)
        {
            String str = Encoding.ASCII.GetString(data);
            String[] keyboard_string = str.Split(':');
            if (keyboard_string[0].CompareTo("KB") == 0)
            {
                try
                {
                    // SendKeys.Send(mouse_string[1]);
                    if (keyboard_string[1].CompareTo("add") == 0)
                    {
                        SendKeys.SendWait("{+}");
                        goto abc;
                    }
                    if (keyboard_string[1].CompareTo("cart") == 0)
                    {
                        SendKeys.SendWait("{^}");
                        goto abc;
                    }
                    if (keyboard_string[1].CompareTo("per") == 0)
                    {
                        SendKeys.SendWait("{%}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("tilde") == 0)
                    {
                        SendKeys.SendWait("{~}");
                        goto abc;
                    }
                    if (keyboard_string[1].CompareTo("left_b") == 0)
                    {
                        SendKeys.SendWait("{(}");
                        goto abc;
                    }
                    if (keyboard_string[1].CompareTo("right_b") == 0)
                    {
                        SendKeys.SendWait("{)}");
                        goto abc;
                    }
                    if (keyboard_string[1].CompareTo("left_b1") == 0)
                    {
                        SendKeys.SendWait("{{}");
                        goto abc;
                    }
                    if (keyboard_string[1].CompareTo("right_b1") == 0)
                    {
                        SendKeys.SendWait("{}}");
                        goto abc;
                    }
                    if (keyboard_string[1].CompareTo("left_b2") == 0)
                    {
                        SendKeys.SendWait("{[}");
                        goto abc;
                    }
                    if (keyboard_string[1].CompareTo("right_b2") == 0)
                    {
                        SendKeys.SendWait("{]}");
                        goto abc;
                    }
                    if (keyboard_string[1].CompareTo("UP") == 0)
                    {
                        SendKeys.SendWait("{UP}");
                        goto abc;
                    }
                    ////////
                    if (keyboard_string[1].CompareTo("DOWN") == 0)
                    {
                        SendKeys.SendWait("{DOWN}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("LEFT") == 0)
                    {
                        SendKeys.SendWait("{LEFT}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("RIGHT") == 0)
                    {
                        SendKeys.SendWait("{RIGHT}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("Pause") == 0)
                    {
                        SendKeys.SendWait("{BREAK}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("PageUp") == 0)
                    {
                        SendKeys.SendWait("{PGUP}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("PageDown") == 0)
                    {
                        SendKeys.SendWait("{PGDN}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("Scroll") == 0)
                    {
                        SendKeys.SendWait("{SCROLLLOCK}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("Home") == 0)
                    {
                        SendKeys.SendWait("{HOME}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("End") == 0)
                    {
                        SendKeys.SendWait("{END}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("PrintScreen") == 0)
                    {
                        SendKeys.SendWait("{PRTSC}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("Insert") == 0)
                    {
                        SendKeys.SendWait("{INS}");
                        goto abc;
                    }

                    if (keyboard_string[1].CompareTo("Delete") == 0)
                    {
                        SendKeys.SendWait("{DELETE}");
                        goto abc;
                    }


                    ///
                    SendKeys.SendWait(keyboard_string[1]);
                abc: ;
                }
                catch (Exception ex)
                { }
            }

        }
    }
}
