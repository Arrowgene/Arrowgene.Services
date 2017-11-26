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

namespace Arrowgene.Services.Logging
{
    public static class LogProvider
    {
        private static readonly Dictionary<string, ILogger> Loggers = new Dictionary<string, ILogger>();
        private static readonly object Lock = new object();
        private static ILogger _producer = new Logger("Producer");

        public static event EventHandler<LogWriteEventArgs> LogWrite;

        /// <summary>
        /// Provide an implementation of ILogger.
        /// All logging will be handled by the provided implementation.
        /// </summary>
        /// <param name="logger"></param>
        public static void SetProducer(ILogger logger)
        {
            _producer = logger;
        }

        public static ILogger GetLogger(object instance)
        {
            return GetLogger(instance.GetType());
        }

        public static ILogger GetLogger(Type type)
        {
            return GetLogger(type.FullName, type.Name);
        }

        public static ILogger GetLogger(string identity, string zone = null)
        {
            ILogger logger;
            lock (Lock)
            {
                if (!Loggers.TryGetValue(identity, out logger))
                {
                    logger = _producer.Produce(identity, zone);
                    logger.LogWrite += LoggerOnLogWrite;
                    Loggers.Add(identity, logger);
                }
            }
            return logger;
        }

        private static void LoggerOnLogWrite(object sender, LogWriteEventArgs writeEventArgs)
        {
            EventHandler<LogWriteEventArgs> logWrite = LogWrite;
            if (logWrite != null)
            {
                logWrite(sender, writeEventArgs);
            }
        }
    }
}