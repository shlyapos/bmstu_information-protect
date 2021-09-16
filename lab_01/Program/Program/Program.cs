using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace Program
{
    public class Program
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

        private static string GetHashFromConfigFile()
        {
            FileStream fstream = File.OpenRead(ConfigFile);

            byte[] array = new byte[fstream.Length];
            fstream.Read(array, 0, array.Length);

            return Encoding.Default.GetString(array);
        }

        private static bool CompareHash(string str1, string str2)
        {
            return String.Compare(str1, str2) == 0;
        }


        static void Main(string[] args)
        {
            string MacAddress = GetMacAddress();
            string MachineName = GetMachineName();

            if (File.Exists(ConfigFile) && CompareHash(GenerateHash(MacAddress, MachineName), GetHashFromConfigFile()))
            {
                Console.WriteLine("// OK, you can use me!");
                Console.WriteLine("// I also know your computer name: " + GetMachineName());
            }
            else
            {
                System.Diagnostics.Process.Start("https://raw.githubusercontent.com/shlyapos/bmstu_information-protect/main/lab_01/src/b72519d7-55fd-4720-85f7-9138d55c3e49_fs.jpg");
                Console.WriteLine("// Слышь, купи");
            }

            Console.WriteLine("// Enter any key for exit...");
            Console.ReadKey();
        }
    }
}
