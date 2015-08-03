﻿namespace ArrowgeneServices.Networking.Proxy
{
    using ArrowgeneServices.Provider;
    public class ProxyPacket 
    {
        public enum TrafficType { CLIENT, SERVER };
        public TrafficType Traffic { get; set; }
        public ByteBuffer Payload { get; set; }

        public ProxyPacket(ByteBuffer payload)
        {
            this.Payload = payload;
        }
    }
}