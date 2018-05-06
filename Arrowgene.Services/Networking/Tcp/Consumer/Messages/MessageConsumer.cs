﻿/*
 * MIT License
 * 
 * Copyright (c) 2018 Sebastian Heinz <sebastian.heinz.gt@googlemail.com>
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */


using System;
using System.Collections.Generic;
using Arrowgene.Services.Networking.Tcp.Consumer.GenericConsumption;

namespace Arrowgene.Services.Networking.Tcp.Consumer.Messages
{
    public class MessageConsumer : GenericConsumer<Message>, IMessageSerializer
    {
        private readonly Dictionary<int, IMessageHandle> _handles;

        public MessageConsumer()
        {
            _handles = new Dictionary<int, IMessageHandle>();
        }

        public void AddHandle(IMessageHandle handle)
        {
            if (_handles.ContainsKey(handle.Id))
            {
                throw new Exception(string.Format("Handle for id: {0} already defined.", handle.Id));
            }

            handle.SetMessageSerializer(this);
            _handles.Add(handle.Id, handle);
        }

        protected override void OnReceivedGeneric(ITcpSocket socket, Message message)
        {
            base.OnReceivedGeneric(socket, message);
            if (_handles.ContainsKey(message.Id))
            {
                _handles[message.Id].Process(message, socket);
            }
        }
    }
}