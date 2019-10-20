using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SampleMvc
{
    class WebServer : IDisposable
    {
        private readonly TcpListener _tcpListener;
        private readonly List<IHandler> _handlers = new List<IHandler>();

        public WebServer()
        {
            _tcpListener = new TcpListener(IPAddress.Any, 80);
            _tcpListener.Start();
        }

        public WebServer Add(IHandler handler)
        {
            _handlers.Add(handler);
            return this;
        }

        public void Start()
        {
            while(true)
            {
                var client = _tcpListener.AcceptTcpClient();
                Task.Run(() => Process(client));
            }
        }

        private void Process(TcpClient client)
        {
            using (client)
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream);
                var rawRequest = new StringBuilder();
                string line;
                do
                {
                    line = reader.ReadLine();
                    rawRequest.AppendLine(line);
                }
                while (!string.IsNullOrEmpty(line));

                var context = new WebContext(rawRequest.ToString());
                foreach (var handler in _handlers)
                {
                    if (handler.Handle(context))
                        break;
                }

                var writer = new StreamWriter(stream);
                writer.WriteLine(context.Response);
                writer.Flush();
            }
        }

        public void Dispose()
        {
            _tcpListener.Stop();
        }
    }
}
