﻿using NSSLServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;

namespace NSSLServer.Sources
{
    class OutpanProductSource : IProductSource
    {
        public bool islocal { get; } = false;
        public long Total { get; set; } = 0;

        public async static Task AddProduct(string name, string gtin)
        {
            // TODO Implement Adding Products to Outpan
        }
        async Task<BasicProduct> IProductSource.FindProductByCode(string code)
        {
            string url = "https://" + $"api.outpan.com/v2/products/{code}?apikey=1e0dea2842c3bd80559b9ef0a8df187b";
            var request = WebRequest.Create(url);
            Stream responseStream = (await  request.GetResponseAsync()).GetResponseStream();
            using (StreamReader sr = new StreamReader(responseStream))
            {
                JObject o = JObject.Parse(sr.ReadToEnd());
                var name = o["name"].ToString();
                var gtin = o["gtin"].ToString();

                if (string.IsNullOrWhiteSpace(name))
                    return null;

                LocalCacheProductSource.AddProduct(name, gtin);

                BasicProduct p = new BasicProduct { Name = name, Gtin = gtin};
               
                return p;
            }
        }

        async Task<Paged<BasicProduct>> IProductSource.FindProductsByName(string name, int i)
        {
            return new Paged<BasicProduct>();
        }
    }
}