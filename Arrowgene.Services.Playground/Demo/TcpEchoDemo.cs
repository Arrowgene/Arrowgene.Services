﻿using System;
using System.Net;
using System.Threading;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;
namespace Arrowgene.Services.Playground.Demo
{
    public class TcpEchoDemo
    {
        private volatile bool _isRunning;
        private BlockingQueueConsumer _consumer;
        private Thread _consumerThread;
        private AsyncEventServer _server;

        public TcpEchoDemo()
        {
            LogProvider.GlobalLogWrite += LogProviderOnLogWrite;

            Start();
            Console.WriteLine("Demo: Press any key to exit...");
            Console.ReadKey();
            Stop();
        }

        private void LogProviderOnLogWrite(object sender, LogWriteEventArgs logWriteEventArgs)
        {
            Console.WriteLine(logWriteEventArgs.Log);
        }

        public void Start()
        {
            
            AsyncEventSettings settings = new AsyncEventSettings();
            _consumer = new BlockingQueueConsumer();
            _server = new AsyncEventServer(IPAddress.IPv6Any, 2345, _consumer,settings);
            _server.Start();
            _consumerThread = new Thread(HandleEvents);
            _consumerThread.Name = "ConsumerThread";
            _consumerThread.Start();
        }

        public void Stop()
        {
            _server.Stop();
            _isRunning = false;
            if (_consumerThread != null && Thread.CurrentThread != _consumerThread && !_consumerThread.Join(1000))
            {
                _consumerThread.Interrupt();
            }
        }

        private void HandleEvents()
        {
            _isRunning = true;
            ClientEvent clientEvent = null;
            while (_isRunning)
            {
                Console.WriteLine(string.Format("Demo: Client Events: {0}", _consumer.ClientEvents.Count));
                try
                {
                    clientEvent = _consumer.ClientEvents.Take();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    _isRunning = false;
                }

                if (clientEvent != null)
                {
                    switch (clientEvent.ClientEventType)
                    {
                        case ClientEventType.Connected:
                            Console.WriteLine(string.Format("Demo: Server: Client Connected ({0})", clientEvent.Socket));
                            break;
                        case ClientEventType.Disconnected:
                            Console.WriteLine(string.Format("Demo: Server: Client Disconnected ({0})", clientEvent.Socket));
                            break;
                        case ClientEventType.ReceivedData:
                            byte[] data = clientEvent.Data;
                            Console.WriteLine(string.Format("Demo: Server: received packet Size:{0}", data.Length));
                            clientEvent.Socket.Send(data);
                            break;
                    }
                }
            }

            Console.WriteLine("Demo: Handler exited");
        }
    }
}