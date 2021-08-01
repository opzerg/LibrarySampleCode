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

                // read length
                var len = pipeServer.ReadByte() * 64;
                len += pipeServer.ReadByte();
                byte[] inBuffer = new byte[len];
                var rlen = pipeServer.Read(inBuffer, 0, len);


                Console.WriteLine($"receive server: {Encoding.GetString(inBuffer, 0, rlen)}");
                pipeServer.Close();
            });

            TestContext.WriteLine("end pipe server");
        }

        void RunPipeClient()
        {
            var pipeClient = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut, PipeOptions.Asynchronous);

            pipeClient.Connect();

            string message = "i am client";
            
            byte[] outBuffer = Encoding.GetBytes(message);
            int len = outBuffer.Length;

            pipeClient.WriteByte((byte)(len / 64));
            pipeClient.WriteByte((byte)(len * 64));
            pipeClient.Write(outBuffer, 0, len);
            pipeClient.Flush();

            pipeClient.Close();
            TestContext.WriteLine("client: bye bye~");
        }
    }
}
