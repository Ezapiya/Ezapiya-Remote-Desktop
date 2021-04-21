using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetUsDo
{
    class Data_Class
    {
        public byte[] Add_Payload(String command, String data)
        {
            byte[] b_data = Encoding.ASCII.GetBytes(data);
            byte[] byte_data = new byte[b_data.Length + 5];
            if (command.CompareTo("IMG") == 0)
                byte_data[0] = (byte)0;
            if (command.CompareTo("STR") == 0)
                byte_data[0] = (byte)1;
            if (command.CompareTo("MOUSE") == 0)
                byte_data[0] = (byte)3;
            if (command.CompareTo("KEYBOARD") == 0)
                byte_data[0] = (byte)4;
            if (command.CompareTo("SYSTEM") == 0)
                byte_data[0] = (byte)5;
            if (command.CompareTo("PROCESS") == 0)
                byte_data[0] = (byte)6;
            if (command.CompareTo("COMMAND") == 0)
                byte_data[0] = (byte)7;
            if (command.CompareTo("CHAT") == 0)
                byte_data[0] = (byte)8;
            if (command.CompareTo("HARTBIT") == 0)
                byte_data[0] = (byte)9;
            if (command.CompareTo("FILE_SYSTEM") == 0)
                byte_data[0] = (byte)10;
            if (command.CompareTo("CLIPBOARD") == 0)
                byte_data[0] = (byte)11;

            if (command.CompareTo("FILE_UPLOAD") == 0)
                byte_data[0] = (byte)12;
            if (command.CompareTo("FILE_UPLOAD_COMPLIT") == 0)
                byte_data[0] = (byte)13;
            if (command.CompareTo("FILE_DOWNLOAD") == 0)
                byte_data[0] = (byte)14;

            if (command.CompareTo("FILE_DOWNLOAD_COMPLIT") == 0)
                byte_data[0] = (byte)15;




            

         
            if (command.CompareTo("LOGIN_NAME") == 0)
                byte_data[0] = (byte)18;
            if (command.CompareTo("BLACK_SCREEN") == 0)
                byte_data[0] = (byte)19;
            if (command.CompareTo("BLACK_SCREEN_ING") == 0)
                byte_data[0] = (byte)20;
            if (command.CompareTo("BLACK_SCREEN_IMAGE_LIST") == 0)
                byte_data[0] = (byte)21;
            if (command.CompareTo("STARTUP_PATH") == 0)
                byte_data[0] = (byte)22;
            if (command.CompareTo("BLACK_SCREEN_IMAGE_NAME") == 0)
                byte_data[0] = (byte)23;
            if (command.CompareTo("BLACK_SCREEN_IMAGE_RECIVED") == 0)
                byte_data[0] = (byte)24;
            
            

            if (command.CompareTo("Exit") == 0)
                byte_data[0] = (byte)25;
            if (command.CompareTo("DESKTOP_AS_BLACK_SCREEN") == 0)
                byte_data[0] = (byte)26;

            if (command.CompareTo("NEW_IMG") == 0)
                byte_data[0] = (byte)27;

            if (command.CompareTo("IMG_RTP") == 0)
                byte_data[0] = (byte)28;
            if (command.CompareTo("MIC") == 0)
                byte_data[0] = (byte)29;

            if (command.CompareTo("START_MIC") == 0)
                byte_data[0] = (byte)30;

            if (command.CompareTo("RESET_CONNECTION") == 0)
                byte_data[0] = (byte)31;

            if (command.CompareTo("NO_IMAGE_CHANGE") == 0)
                byte_data[0] = (byte)32;

            if (command.CompareTo("SHOW_DRAWING") == 0)
                byte_data[0] = (byte)41;
            if (command.CompareTo("NEW_DRAWING") == 0)
                byte_data[0] = (byte)42;
            if (command.CompareTo("REMOVE_DRAWING") == 0)
                byte_data[0] = (byte)43;

            if (command.CompareTo("FT_INFO") == 0)
                byte_data[0] = (byte)60;
            if (command.CompareTo("FT_DATA") == 0)
                byte_data[0] = (byte)61;


            Int32 data_len = b_data.Length + 5;
            byte[] len_byte = BitConverter.GetBytes(data_len);
            byte_data[1] = len_byte[0];
            byte_data[2] = len_byte[1];
            byte_data[3] = len_byte[2];
            byte_data[4] = len_byte[3];
            Array.Copy(b_data, 0, byte_data, 5, b_data.Length);
            return byte_data;
        }
        public byte[] Add_Payload(String command, byte[] data)
        {

            byte[] byte_data = new byte[data.Length + 5];
            if (command.CompareTo("IMG") == 0)
                byte_data[0] = (byte)0;
            if (command.CompareTo("STR") == 0)
                byte_data[0] = (byte)1;
            if (command.CompareTo("MOUSE") == 0)
                byte_data[0] = (byte)3;
            if (command.CompareTo("KEYBOARD") == 0)
                byte_data[0] = (byte)4;
            if (command.CompareTo("SYSTEM") == 0)
                byte_data[0] = (byte)5;
            if (command.CompareTo("PROCESS") == 0)
                byte_data[0] = (byte)6;
            if (command.CompareTo("COMMAND") == 0)
                byte_data[0] = (byte)7;
            if (command.CompareTo("CHAT") == 0)
                byte_data[0] = (byte)8;
            if (command.CompareTo("HARTBIT") == 0)
                byte_data[0] = (byte)9;
            if (command.CompareTo("FILE_SYSTEM") == 0)
                byte_data[0] = (byte)10;
            if (command.CompareTo("CLIPBOARD") == 0)
                byte_data[0] = (byte)11;
            if (command.CompareTo("FILE_UPLOAD") == 0)
                byte_data[0] = (byte)12;
            if (command.CompareTo("FILE_UPLOAD_COMPLIT") == 0)
                byte_data[0] = (byte)13;
            if (command.CompareTo("FILE_DOWNLOAD") == 0)
                byte_data[0] = (byte)14;
            if (command.CompareTo("FILE_DOWNLOAD_COMPLIT") == 0)
                byte_data[0] = (byte)15;


            if (command.CompareTo("LOGIN_NAME") == 0)
                byte_data[0] = (byte)18;
            if (command.CompareTo("BLACK_SCREEN") == 0)
                byte_data[0] = (byte)19;
            if (command.CompareTo("BLACK_SCREEN_ING") == 0)
                byte_data[0] = (byte)20;
            if (command.CompareTo("BLACK_SCREEN_IMAGE_LIST") == 0)
                byte_data[0] = (byte)21;
            if (command.CompareTo("STARTUP_PATH") == 0)
                byte_data[0] = (byte)22;
            if (command.CompareTo("BLACK_SCREEN_IMAGE_NAME") == 0)
                byte_data[0] = (byte)23;
            if (command.CompareTo("BLACK_SCREEN_IMAGE_RECIVED") == 0)
                byte_data[0] = (byte)24;


            
            
            if (command.CompareTo("Exit") == 0)
                byte_data[0] = (byte)25;
            if (command.CompareTo("DESKTOP_AS_BLACK_SCREEN") == 0)
                byte_data[0] = (byte)26;

            if (command.CompareTo("NEW_IMG") == 0)
                byte_data[0] = (byte)27;

            if (command.CompareTo("IMG_RTP") == 0)
                byte_data[0] = (byte)28;
            if (command.CompareTo("MIC") == 0)
                byte_data[0] = (byte)29;

            if (command.CompareTo("START_MIC") == 0)
                byte_data[0] = (byte)30;

            if (command.CompareTo("RESET_CONNECTION") == 0)
                byte_data[0] = (byte)31;

            if (command.CompareTo("NO_IMAGE_CHANGE") == 0)
                byte_data[0] = (byte)32;


            if (command.CompareTo("SHOW_DRAWING") == 0)
                byte_data[0] = (byte)41;
            if (command.CompareTo("NEW_DRAWING") == 0)
                byte_data[0] = (byte)42;
            if (command.CompareTo("REMOVE_DRAWING") == 0)
                byte_data[0] = (byte)43;

            if (command.CompareTo("FT_INFO") == 0)
                byte_data[0] = (byte)60;
            if (command.CompareTo("FT_DATA") == 0)
                byte_data[0] = (byte)61;

            Int32 data_len = data.Length + 5;
            byte[] len_byte = BitConverter.GetBytes(data_len);
            byte_data[1] = len_byte[0];
            byte_data[2] = len_byte[1];
            byte_data[3] = len_byte[2];
            byte_data[4] = len_byte[3];
            Array.Copy(data, 0, byte_data, 5, data.Length);
            return byte_data;
        }
        public byte[] Get_Payload(byte[] data, ref String command)
        {
            if ((int)data[0] == 0)
                command = "IMG";
            if ((int)data[0] == 1)
                command = "STR";
            if ((int)data[0] == 3)
                command = "MOUSE";
            if ((int)data[0] == 4)
                command = "KEYBOARD";
            if ((int)data[0] == 5)
                command = "SYSTEM";
            if ((int)data[0] == 6)
                command = "PROCESS";
            if ((int)data[0] == 7)
                command = "COMMAND";
            if ((int)data[0] == 8)
                command = "CHAT";
            if ((int)data[0] == 10)
                command = "FILE_SYSTEM";
            if ((int)data[0] == 11)
                command = "CLIPBOARD";

            if ((int)data[0] == 12)
                command = "FILE_UPLOAD";
            if ((int)data[0] == 13)
                command = "FILE_UPLOAD_COMPLIT";
            if ((int)data[0] == 14)
                command = "FILE_DOWNLOAD";
            if ((int)data[0] == 15)
                command = "FILE_DOWNLOAD_COMPLIT";


            if ((int)data[0] == 18)
                command = "LOGIN_NAME";
            if ((int)data[0] == 19)
                command = "BLACK_SCREEN";
            if ((int)data[0] == 20)
                command = "BLACK_SCREEN_ING";
            if ((int)data[0] == 21)
                command = "BLACK_SCREEN_IMAGE_LIST";
            if ((int)data[0] == 22)
                command = "STARTUP_PATH";
            if ((int)data[0] == 23)
                command = "BLACK_SCREEN_IMAGE_NAME";
            if ((int)data[0] == 24)
                command = "BLACK_SCREEN_IMAGE_RECIVED";



            if ((int)data[0] == 25)
                command = "Exit";
            if ((int)data[0] == 26)
                command = "DESKTOP_AS_BLACK_SCREEN";
            if ((int)data[0] == 27)
                command = "NEW_IMG";
            if ((int)data[0] == 28)
                command = "IMG_RTP";

            if ((int)data[0] == 29)
                command = "MIC";
            if ((int)data[0] == 30)
                command = "START_MIC";


            if ((int)data[0] == 31)
                command = "RESET_CONNECTION";

            if ((int)data[0] == 32)
                command = "NO_IMAGE_CHANGE";


            if ((int)data[0] == 41)
                command = "SHOW_DRAWING";
            if ((int)data[0] == 42)
                command = "NEW_DRAWING";
            if ((int)data[0] == 43)
                command = "REMOVE_DRAWING";

            if ((int)data[0] == 50)
                command = "P2P_ADDRESS";

            if ((int)data[0] == 60)
                command = "FT_INFO";
            if ((int)data[0] == 61)
                command = "FT_DATA";


            byte[] byte_data = new byte[data.Length - 5];
            Array.Copy(data, 5, byte_data, 0, data.Length - 5);
            return byte_data;
        }
    }
}
