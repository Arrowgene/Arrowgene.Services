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
namespace Arrowgene.Services.Network.MarrySocket.MClient
{
    using System;

    /// <summary>
    /// TODO SUMMARY
    /// </summary>
    public class ReceivedPacketEventArgs : EventArgs
    {
        /// <summary>
        /// TODO SUMMARY
        /// </summary>
        public ReceivedPacketEventArgs(int packetId, ServerSocket serverSocket, object myObject)
        {
            this.ServerSocket = serverSocket;
            this.PacketId = packetId;
            this.MyObject = myObject;
        }

        /// <summary>
        /// TODO SUMMARY
        /// </summary>
        public int PacketId { get; private set; }

        /// <summary>
        /// TODO SUMMARY
        /// </summary>
        public ServerSocket ServerSocket { get; private set; }

        /// <summary>
        /// TODO SUMMARY
        /// </summary>
        public object MyObject { get; private set; }
    }
}