﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TradeBot.Generated
{

    public class AppPreferences
    {

        [JsonProperty("clientId")]
        public int ClientId { get; set; }

        [JsonProperty("clientUrl")]
        public string ClientUrl { get; set; }

        [JsonProperty("clientPort")]
        public int ClientPort { get; set; }

        [JsonProperty("windowWidth")]
        public int WindowWidth { get; set; }

        [JsonProperty("windowHeight")]
        public int WindowHeight { get; set; }

        [JsonProperty("centerWindow")]
        public bool CenterWindow { get; set; }
    }

}
