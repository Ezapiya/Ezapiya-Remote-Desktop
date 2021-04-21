using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Windows;

namespace LetUsDo
{
   public static class HardwareInfo
    {

       public static String GetHDDSerialNo()
       {
           ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
           ManagementObjectCollection mcol = mangnmt.GetInstances();
           string result = "";
           foreach (ManagementObject strt in mcol)
           {
               result += Convert.ToString(strt["VolumeSerialNumber"]);
           }
           return result;
       }
    }
}
