using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Installer
{
    public class InstallerProgram
    {
        private static string ConfigFile = "config.cfg";

        private static string GetMacAddress()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();
        }

        private static string GetMachineName()
        {
            return Environment.MachineName;
        }

        private static string GenerateHash(string str1, string str2)
        {
            var md5 = MD5.Create();

            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(String.Concat(str1, str2)));

            return Convert.ToBase64String(hash);
        }


        private static void CreateConfigFile(string hash)
        {
            FileStream fstream = new FileStream(ConfigFile, FileMode.OpenOrCreate);

            // Write in file
            byte[] array = Encoding.Default.GetBytes(hash);
            fstream.Write(array, 0, array.Length);

            fstream.Close();
        }

        static void Main(string[] args)
        {
            string MacAddress = GetMacAddress();
            string MachineName = GetMachineName();

            CreateConfigFile(GenerateHash(MacAddress, MachineName));
        }
    }
}
