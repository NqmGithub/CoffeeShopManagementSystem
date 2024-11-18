using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.Helpers
{
    public class SendEmailHelper
    {
        public static bool SendEmailOTP(string to, string otp)
        {
            string Subject = "no-reply";
            string Content = $"{otp} is your otp";
            string cc = "";
            string bcc = "";
            string file = "";


            string output = "";
            string error = "";
            bool started = false;
            try
            {
                Process process = new Process();

                string fullpath = System.IO.Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "CoffeeShopManagement.Business", "Helpers", "EmailSending", "mailCPL.exe");

                process.StartInfo.FileName = fullpath;

                process.StartInfo.Arguments = $"\"{to}\" \"{Subject}\" \"{Content}\" \"{file}\" \"{cc}\" \"{bcc}\"";

                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = System.IO.Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "CoffeeShopManagement.Business", "Helpers", "EmailSending");

                started = process.Start();

                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();

                process.WaitForExit();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + " " + error);
                Console.WriteLine();
            }

            return output == "Email(s) sent\r\n";
        }

        public static bool SendEmailResetPassword(string to, string newPass)
        {
            string Subject = "no-reply";
            string Content = $"{newPass} is your reset password";
            string cc = "";
            string bcc = "";
            string file = "";


            string output = "";
            string error = "";
            bool started = false;
            try
            {
                Process process = new Process();

                string fullpath = System.IO.Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "CoffeeShopManagement.Business", "Helpers", "EmailSending", "mailCPL.exe");

                process.StartInfo.FileName = fullpath;

                process.StartInfo.Arguments = $"\"{to}\" \"{Subject}\" \"{Content}\" \"{file}\" \"{cc}\" \"{bcc}\"";

                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = System.IO.Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "CoffeeShopManagement.Business", "Helpers", "EmailSending");

                started = process.Start();

                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();

                process.WaitForExit();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + " " + error);
                Console.WriteLine();
            }

            return output == "Email(s) sent\r\n";
        }

    }
}
