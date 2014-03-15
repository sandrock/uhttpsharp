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
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace uhttpsharp
{
    public sealed class HttpRouter
    {
        private readonly Dictionary<string, IHttpHandler> _handlers = new Dictionary<string, IHttpHandler>();

        internal HttpRouter()
        {
            RegisterHandlers();
        }

        private HttpResponse DefaultError(HttpContext context)
        {
            return HttpResponse.CreateWithMessage(context, HttpResponseCode.NotFound, "Not Found");
        }

        private HttpResponse DefaultIndex(HttpContext context)
        {
            return HttpResponse.CreateWithMessage(context, HttpResponseCode.Ok, "Welcome to uhttpsharp!");
        }

        public HttpResponse Route(HttpContext context)
        {
            var request = context.Request;
            var function = request.Parameters.Function;
            return
                RouteToFunction(context, function) ??
                RouteToFunction(context, "*") ??
                (string.IsNullOrEmpty(function) ? (RouteToFunction(context, "") ?? DefaultIndex(context)) : null) ??
                RouteToFunction(context, "404") ??
                DefaultError(context);
        }

        private HttpResponse RouteToFunction(HttpContext context, string function)
        {
            var request = context.Request;
            IHttpHandler handler;

            if (_handlers.TryGetValue(function, out handler))
            {
                context.Response = handler.Handle(context);
                return context.Response;
            }

            return null;
        }

        private void RegisterHandlers()
        {
            foreach (var type in Assembly.GetEntryAssembly().GetTypes())
            {
                if (type.IsSubclassOf(typeof(HttpRequestHandler)))
                {
                    try
                    {
                        var attributes = type.GetCustomAttributes(typeof(HttpRequestHandlerAttributes), true);
                        if (attributes.Length > 0)
                        {
                            var handler = (HttpRequestHandler)Activator.CreateInstance(type);
                            _handlers.Add(((HttpRequestHandlerAttributes)attributes[0]).Function, handler);
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(string.Format("Exception during activating the IHttpRequestHandler: {0} - {1}", type, ex));
                    }
                }
            }
        }
    }
}