using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using HoldingDetails.Models;

namespace HoldingDetails.BL
{
    public class RestHelper
    {
        readonly string ClientId;
        readonly string Secret;
        readonly string ApiUrl;

        public RestHelper(string ClientId,string Secret,string ApiUrl)
        {
            this.ClientId = ClientId;
            this.Secret = Secret;
            this.ApiUrl = ApiUrl;
        }

        public IRestResponse RestCall(string Url, object Parameter)
        {
            var client = new RestClient(Url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(Parameter), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;
        }

        public LinkTokenResponse GetLinkToken()
        {
            LinkTokenResponse linkTokenResponse = null;

            try
            {
                LinkTokenParam obj = new LinkTokenParam();
                obj.ClientId = ClientId;
                obj.Secret = Secret;
                obj.ClientName = "Plaid App";
                obj.User = new User {ClientUserId = "test_user" };
                obj.Products = new string[] { "auth","investments" };
                obj.CountryCodes = new string[] { "GB","US" };
                obj.Language = "en";

                IRestResponse response = RestCall(string.Format("{0}/link/token/create", ApiUrl), obj);
                linkTokenResponse = JsonConvert.DeserializeObject<LinkTokenResponse>(response.Content);
            }
            catch(Exception ex)
            {

            }

            return linkTokenResponse;
        }

        public HoldingResponse GetHoldings(string public_Token)
        {
            HoldingResponse HoldingDtl = null;

            try
            {
                /*
                PublicTokenParam obj = new PublicTokenParam();
                obj.ClientId = ClientId;
                obj.Secret = Secret;
                obj.InstitutionId = "ins_3";
                obj.InitialProducts = new string[] { "auth" };
                
                IRestResponse response1 = RestCall(string.Format("{0}/sandbox/public_token/create", ApiUrl), obj);
                PublicTokenResponse publicToken = JsonConvert.DeserializeObject<PublicTokenResponse>(response1.Content);
                */
                PublicTokenExchangeParam objExPrm = new PublicTokenExchangeParam();
                objExPrm.ClientId = ClientId;
                objExPrm.Secret = Secret;
                objExPrm.PublicToken = public_Token;
                

                IRestResponse response2 = RestCall(string.Format("{0}/item/public_token/exchange", ApiUrl), objExPrm);
                PublicTokenExchangeResponse publicTokenEx = JsonConvert.DeserializeObject<PublicTokenExchangeResponse>(response2.Content);


                HoldingReqParam objHoldPrm = new HoldingReqParam();
                objHoldPrm.ClientId = ClientId;
                objHoldPrm.Secret = Secret;
                objHoldPrm.AccessToken = publicTokenEx.AccessToken;

                IRestResponse response3 = RestCall(string.Format("{0}/investments/holdings/get", ApiUrl), objHoldPrm);
                HoldingDtl = JsonConvert.DeserializeObject<HoldingResponse>(response3.Content);
            }
            catch (Exception ex)
            {

            }

            return HoldingDtl;

            //----------------------------
        }


    }
}
