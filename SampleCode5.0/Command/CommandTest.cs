using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyModel.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode5._0
{
    [TestClass]
    public class CommandTest
    {
        public string TestFile = "rip.jpg";
        [TestMethod]
        public void CLIOptionTest()
        {
            string[] args = new[] { "-p", Path.Combine(Environment.CurrentDirectory, TestFile), "-d", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), TestFile), "-o" };

            Parser.Default.ParseArguments<CLIOption>(args).WithParsed(cliOption =>
            {
                File.Copy(cliOption.Path, cliOption.DestPath, cliOption.Overwrite);
            }).WithNotParsed(e =>
            {
                Assert.Fail();
            });


        }
    }
}
