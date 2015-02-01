﻿/*
 *  Copyright 2015 Sebastian Heinz <sebastian.heinz.gt@googlemail.com>
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */
namespace MarrySocket.MServer
{
    using MarrySocket.MBase;
    using MarrySocket.MExtra;
    using MarrySocket.MExtra.Logging;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    public class ClientSocket : BaseSocket
    {
        private IPEndPoint remoteIpEndPoint;
        private Logger serverLog;

        public ClientSocket(Socket socket, Logger serverLog)
        {
            this.Id = Maid.Random.Next(9999);
            this.serverLog = serverLog;
            this.IsAlive = true;
            this.Socket = socket;
            this.remoteIpEndPoint = socket.RemoteEndPoint as IPEndPoint;
            this.LastPing = DateTime.Now;
            this.IsBusy = false;
        }

        internal bool IsBusy { get; set; }
        internal bool IsAlive { get; set; }
        public int Id { get; private set; }
        public DateTime LastPing { get; private set; }
        public string Ip { get { return ((IPEndPoint)this.Socket.RemoteEndPoint).Address.ToString(); } }

        public void Close()
        {
            this.Disconnect();
        }

        protected override void Disconnect()
        {
            base.Disconnect();
            this.IsAlive = false;
        }
    
        protected override void Error(string error)
        {
            this.serverLog.Write(error, LogType.ERROR);
        }
    }
}
