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
        public TestContext TestContext { get; set; }
        public string TestFile = "rip.jpg";
        [TestMethod]
        public void SingleCLIOptionTest()
        {
            string[] args = new[] { "-p", Path.Combine(Environment.CurrentDirectory, TestFile), "-d", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), TestFile), "-o" };

            Parser.Default.ParseArguments<CopyOption>(args).WithParsed(cliOption =>
            {
                File.Copy(cliOption.Path, cliOption.DestPath, cliOption.Overwrite);
            }).WithNotParsed(e =>
            {
                Assert.Fail();
            });
        }

        [TestMethod]
        public void MultiCLIOptionTest()
        {
            // copy
            string[] args = new[] { "copy", "-p", Path.Combine(Environment.CurrentDirectory, TestFile), "-d", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), TestFile), "-o" };

            RunMultiCLIOptions(args);

            // delete
            args = new[] { "delete", "-p", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), TestFile), "-b", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{TestFile}.bak") };

            RunMultiCLIOptions(args);
        }

        private void RunMultiCLIOptions(string[] args)
        {
            Parser.Default.ParseArguments<CopyOption, DeleteOption>(args).MapResult(
                            (CopyOption opts) =>
                            {
                                TestContext.WriteLine($"file copy {opts.Path} to {opts.DestPath}");
                                File.Copy(opts.Path, opts.DestPath, opts.Overwrite);
                                return true;
                            },
                            (DeleteOption opts) =>
                            {
                                if (!string.IsNullOrEmpty(opts.BakupPath))
                                {
                                    TestContext.WriteLine($"file backup {opts.Path} to {opts.BakupPath}");
                                    File.Copy(opts.Path, opts.BakupPath, true);
                                }

                                TestContext.WriteLine($"file delete {opts.Path}");
                                File.Delete(opts.Path);
                                return true;
                            },
                            errors => false);
        }
    }
}
