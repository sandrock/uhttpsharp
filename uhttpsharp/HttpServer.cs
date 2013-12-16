/*
 * Copyright (C) 2011 uhttpsharp project - http://github.com/raistlinthewiz/uhttpsharp
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.

 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.

 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */

namespace uhttpsharp
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Threading;
    using System.Diagnostics;

    public class HttpServer : IDisposable
    {
        public string Banner = string.Empty;
        private bool isDisposed;
        private TcpListener listener;
        private bool isActive;
        private Thread serverThread;

        public HttpServer(IPAddress address, int port)
        {
            this.Address = address;
            this.Port = port;
            this.Banner = string.Format("uhttpsharp {0}", Assembly.GetExecutingAssembly().GetName().Version);
            this.Router = new HttpRouter();
        }

        public IPAddress Address { get; set; }

        public int Port { get; set; }

        public HttpRouter Router { get; private set; }

        public void Start()
        {
            if (this.isActive)
                return;
            this.listener = new TcpListener(IPAddress.Loopback, Port);
            this.listener.Start();
            this.serverThread = new Thread(Listen)
            {
                IsBackground = true,
            };
            this.serverThread.Start();
        }

        public void Dispose()
        {
            this.Dispose(true, TimeSpan.FromSeconds(60D));
            GC.SuppressFinalize(this);
        }

        public void Dispose(TimeSpan timeout)
        {
            this.Dispose(true, timeout);
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return this.GetType().Name + " " + this.Address.ToString() + ":" + this.Port.ToString();
        }

        protected virtual void Dispose(bool disposing, TimeSpan timeout)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.isActive = false;

                    if (this.listener != null)
                    {
                        this.listener.Stop();
                        this.listener = null;
                    }

                    if (this.serverThread != null)
                    {
                        this.serverThread.Join(timeout);
                        this.serverThread = null;
                    }
                }

                this.isDisposed = true;
            }
        }

        private void Listen()
        {
            this.isActive = true;

            Trace.TraceInformation(this.ToString() + " is now listenning.");

            while (this.isActive)
            {
                var context = new HttpContext
                {
                    Server = this,
                };
                new HttpClient(context, listener.AcceptTcpClient());
            }

            Trace.TraceInformation(this.ToString() + " is not listenning anymore.");
        }
    }
}