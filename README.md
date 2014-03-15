# uhttpsharp

A very lightweight & simple embedded http server for c# 

## In this branch

A few changes I'm making for [another project](https://github.com/sandrock/MarkDownBrowser/):

* Allow multiple instances (on multiple TCP/IP ports)
  * Bye bye static stuff  
* Allow bit of IoC 
* Convert the HttpHandler class to a IHttpHandler interface
* Create a HttpContext class to simplify IHttpHandler

## License

Copyright (C) 2011 uhttpsharp project

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

