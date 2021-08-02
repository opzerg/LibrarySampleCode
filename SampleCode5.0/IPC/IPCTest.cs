using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyModel;
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
        Encoding Encoding = Encoding.UTF8;
        string PipeName => "testpipe";
        [TestMethod]
        public void NamedPipeTest()
        {
            RunPipeServer();
            RunPipeClient();
        }

        async void RunPipeServer()
        {
            await Task.Factory.StartNew(() =>
            {
                NamedPipeServerStream pipeServer = new NamedPipeServerStream(PipeName);
                pipeServer.WaitForConnection();

                NamedPipeString namedPipeString = new NamedPipeString(pipeServer);
                TestContext.WriteLine($"receive server: {namedPipeString.Read()}");


                pipeServer.Close();
            });

            TestContext.WriteLine("end pipe server");
        }

        void RunPipeClient()
        {
            var pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut, PipeOptions.Asynchronous);

            pipeClient.Connect();

            string message = "Hello World!!!";
            NamedPipeString namedPipeString = new NamedPipeString(pipeClient);
            namedPipeString.Write(message);
            TestContext.WriteLine("client: bye bye~");

            
        }
    }
}
