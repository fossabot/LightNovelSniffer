﻿// Copyright (c) 20011 Oleksandr Tymoshenko <gonzo@bluezbox.com>
// All rights reserved.

// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.

// THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
// OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
// OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
// SUCH DAMAGE.

using System.Xml.Linq;

namespace LightNovelSniffer.Libs.DotNetEpub
{
    class Manifest
    {
        XElement _element;
        internal Manifest()
        {
            _element = new XElement(LightNovelSniffer.Libs.DotNetEpub.Document.OpfNS + "manifest");
        }

        internal void AddItem(string id, string href, string type)
        {
            XElement item = new XElement(LightNovelSniffer.Libs.DotNetEpub.Document.OpfNS + "item");
            item.SetAttributeValue("id", id);
            item.SetAttributeValue("href", href);
            item.SetAttributeValue("media-type", type);
            _element.Add(item);
        }


        internal XElement ToElement()
        {
            return _element;
        }
    }
}
