﻿using Newtonsoft.Json;

namespace HelpfulThings.Connect.Cryptowatch.DataModel
{
    public class Asset
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "fiat")]
        public bool IsFiatCurrency { get; set; }

        [JsonProperty(PropertyName = "markets")]
        public MarketsCollection Markets { get; set; }
    }
}
