using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode5._0
{
    
    [TestClass]
    public class IPCTest
    {
        public TestContext TestContext { get; set; }
        string PipeName => "testpipe";
        [TestMethod]
        public void NamedPipeTest()
        {
            
        }

        async void PipeServer()
        {
            await Task.Factory.StartNew(() =>
            {
                NamedPipeServerStream pipeServer = new NamedPipeServerStream(PipeName);

                pipeServer.WaitForConnection();



            });

            TestContext.WriteLine("end pipe server");
        }
    }
}
