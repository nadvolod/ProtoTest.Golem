﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TestContext = NUnit.Framework.TestContext;

namespace Golem.Core
{
    /// <summary>
    ///     Random methods that don't belong anywhere else
    /// </summary>
    public class Common
    {
        /// <summary>
        ///     Return a psuedo-random string based on the current timestamp
        /// </summary>
        /// <returns></returns>
        public static string GetRandomString()
        {
            return DateTime.Now.ToString("ddHHmmssZ");
        }

        public static void KillProcess(string name)
        {
            var runningProcesses = Process.GetProcesses();
            foreach (var process in runningProcesses)
            {
                try
                {
                    if (process.ProcessName == name)
                    {
                        Log("Killing Process : " + name);
                        process.Kill();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public static Process ExecuteBatchFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Assert.Fail("Could not find batch file : " + filePath);
            }

            return Process.Start(filePath);
        }

        public static Process ExecuteDosCommand(string command, bool waitToFinish = true)
        {
            Log("Executing DOS Command: " + command);
            var CMDprocess = new Process();
            var StartInfo = new ProcessStartInfo();
            StartInfo.FileName = "cmd"; //starts cmd window
            StartInfo.Arguments = "/c \"" + command + "\"";
            //StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            // StartInfo.CreateNoWindow = true;
            StartInfo.RedirectStandardInput = true;
            StartInfo.RedirectStandardOutput = true;
            StartInfo.UseShellExecute = false; //required to redirect
            CMDprocess.StartInfo = StartInfo;
            CMDprocess.Start();
            var SR = CMDprocess.StandardOutput;
            var SW = CMDprocess.StandardInput;
            CMDprocess.Start();
            var line = "";

            while ((line != null) && (waitToFinish))
            {
                line = SR.ReadLine();
                Log(line);
            }

            SW.Close();
            SR.Close();
            return CMDprocess;
        }

        public static void Log(string msg)
        {
            Core.Log.Message(msg);
        }

        public static bool IsTruthy(string truth)
        {
            switch (truth)
            {
                case "1":
                case "true":
                case "True":
                    return true;
                case "0":
                case "false":
                case "False":
                default:
                    return false;
            }
        }

        public static string GetCurrentTestName()
        {
            string TestName = "Test";
            try
            {
                TestName = TestContext.CurrentContext.Test.FullName;
                if (string.IsNullOrEmpty(TestName))
                {
                    TestName = NUnit.Framework.TestContext.CurrentContext.Test.Name;
                }
            }
            catch (Exception e)
            {
                TestName = "Test";
            }
            TestName = Regex.Replace(TestName, "[^a-zA-Z0-9%._]", string.Empty);
            return TestName;
        }

        public static string GetShortTestName(int length)
        {
            var name = GetCurrentTestName();
            name = name.Replace("/", "_");
            name = name.Replace(":", "_");
            name = name.Replace("\\", "_");
            name = name.Replace("\"", "");
            name = name.Replace(" ", "");
            if (name.Length > length)
            {
                name = name.Substring((name.Length - length), length);
            }

            return name;
        }

        public string GetValueFromXmlFile(string filepath, string xpath)
        {
            var configFile = new XmlDocument();
            configFile.Load(filepath);
            return configFile.SelectSingleNode(xpath).Value ?? "";
        }

        public string GetConfigValue(string fileName, string xpath)
        {
            var configFile = new XmlDocument();
            configFile.Load(AppDomain.CurrentDomain.BaseDirectory + fileName);
            return configFile.SelectSingleNode(xpath).Value ?? "";
        }

        public static TestStatus GetTestOutcome()
        {
            if (TestBase.testData.VerificationErrors.Count != 0)
            {
                return TestStatus.Failed;
            }    
            return TestContext.CurrentContext.Result.Outcome.Status;
        }

        public static Image ScaleImage(Image image, double scale = .5)
        {
            var newWidth = (int) (image.Width*scale);
            var newHeight = (int) (image.Height*scale);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public static Image ResizeImage(Image image, int newWidth, int newHeight)
        {
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public static void Delay(int delayMs)
        {
            if (delayMs > 0)
            {
                Thread.Sleep(delayMs);
            }
        }

        /// <summary>
        ///     Create a dummy file with some ASCII
        /// </summary>
        /// <param name="filepath">File path and name to create</param>
        public static void CreateDummyFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                using (var fs = File.Create(filepath))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }

                    fs.Close();
                }
            }
        }

        public static string GetCodeDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "");
        }

        public static void LogImage(Image image)
        {
           
        }

        public static Size GetSizeFromResolution(string resolution)
        {
            var dimensions = resolution.Split('x');
            return new Size(int.Parse(dimensions[0]), int.Parse(dimensions[1]));
        }
    }
}