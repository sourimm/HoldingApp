using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace HoldingDetails.Models
{
    public class HoldingResponce
    {
        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        public List<Holding> holdings = new List<Holding>();
    }

    public class Holding
    {
        [JsonProperty("account_id")]
        public string AccountId { get; set; }
        [JsonProperty("cost_basis")]
        public float CostBasis { get; set; }
        [JsonProperty("institution_price")]
        public float InstitutionPrice { get; set; }
        [JsonProperty("institution_price_as_of")]
        public string InstitutionPriceAsOf { get; set; }
        [JsonProperty("institution_value")]
        public float InstitutionValue { get; set; }
        [JsonProperty("iso_currency_code")]
        public string IsoCurrencyCode { get; set; }
        [JsonProperty("quantity")]
        public float Quantity { get; set; }
        [JsonProperty("security_id")]
        public string SecurityId { get; set; }
        [JsonProperty("unofficial_currency_code")]
        public string UnofficialCurrencyCode { get; set; }
    }
    public class PublicTokenParam
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }
        [JsonProperty("institution_id")]
        public string InstitutionId { get; set; }

        [JsonProperty("initial_products")]
        public string[] InitialProducts { get; set; }
    }

    public class PublicTokenResponce
    {
        [JsonProperty("public_token")]
        public string PublicToken { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }

    public class PublicTokenExchangeParam
    {
        [JsonProperty("public_token")]
        public string PublicToken { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }
    }

    public class PublicTokenExchangeResponce
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("item_id")]
        public string ItemId { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }

    public class HoldingReqParam
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }
    }

    public class LinkTokenParam
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        [JsonProperty("products")]
        public string[] Products { get; set; }

        [JsonProperty("country_codes")]
        public string[] CountryCodes { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }

    public class LinkTokenResponce
    {
        [JsonProperty("link_token")]
        public string LinkToken { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
    }

    public class User
    {
        [JsonProperty("client_user_id")]
        public string ClientUserId { get; set; }
    }
}