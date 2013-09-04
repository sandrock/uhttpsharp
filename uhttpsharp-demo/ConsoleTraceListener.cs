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

namespace uhttpsharpdemo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Logs tracing event to the <see cref="Console"/> with coloration.
    /// </summary>
    public class ConsoleTraceListener : TraceListener
    {
        private readonly object syncRoot = new object();

        public static ConsoleTraceListener Bind()
        {
            var listener = Trace.Listeners.OfType<ConsoleTraceListener>().FirstOrDefault();
            
            if (listener == null)
            {
                Trace.Listeners.Add(listener = new ConsoleTraceListener());
            }

            return listener;
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            base.TraceEvent(eventCache, source, eventType, id, message);

            lock (this.syncRoot)
            {
                var foreg = Console.ForegroundColor;
                var backg = Console.BackgroundColor;

                switch (eventType)
                {
                    case TraceEventType.Critical:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Red;
                        break;

                    case TraceEventType.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.BackgroundColor = ConsoleColor.White;
                        break;

                    case TraceEventType.Verbose:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.BackgroundColor = ConsoleColor.White;
                        break;

                    case TraceEventType.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;

                    case TraceEventType.Information:
                    case TraceEventType.Resume:
                    case TraceEventType.Start:
                    case TraceEventType.Stop:
                    case TraceEventType.Suspend:
                    case TraceEventType.Transfer:
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;
                }

                Console.Write(eventType.ToString().Substring(0, 4).ToUpperInvariant() + " ");

                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;

                Console.WriteLine(message);

                Console.ForegroundColor = foreg;
                Console.BackgroundColor = backg; 
            }
        }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }
    }
}
