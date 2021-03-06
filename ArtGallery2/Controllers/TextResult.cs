﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace ArtGallery2.Controllers
{
    public class TextResult: IHttpActionResult
    {
        string _value;
        HttpRequestMessage _request;

        public TextResult( string value, HttpRequestMessage request )
        {
            _value = value;
            _request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync( CancellationToken cancellationToken )
        {
            var response = new HttpResponseMessage() {
                Content = new StringContent( _value ),
                RequestMessage = _request
            };
            return Task.FromResult( response );
        }
    }
}