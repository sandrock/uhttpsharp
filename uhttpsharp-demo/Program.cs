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

using System;
using System.Net.Sockets;
using uhttpsharp;
using System.Net;

namespace uhttpsharpdemo
{
    internal static class Program
    {
        private static void Main()
        {
            ConsoleTraceListener.Bind();

            HttpServer server = null;
            for (var port = 8000; port <= 65535; ++port)
            {
                server = new HttpServer(IPAddress.Loopback, port);
                try
                {
                    server.Start();
                }
                catch (SocketException)
                {
                    continue;
                }
                break;
            }

            Console.WriteLine("Hit return to exit");
            Console.ReadLine();

            Console.WriteLine("Stopping...");
            server.Dispose(); // TODO: this should be waiting for requests to end
            Console.WriteLine("Stopped.");
        }
    }
}