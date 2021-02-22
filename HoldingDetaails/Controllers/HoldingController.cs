using HoldingDetaails.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using Newtonsoft.Json;

namespace HoldingDetaails.Controllers
{
    public class HoldingController : Controller
    {
        public HoldingController()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
         
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
        
      
        // GET: Holding
        public ActionResult Holding()
        {
            PublicTokenParam obj = new PublicTokenParam();
            obj.ClientId = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientId"];
            obj.Secret = System.Web.Configuration.WebConfigurationManager.AppSettings["Secret"];
            obj.InstitutionId = "ins_3";
            obj.InitialProducts = new string[] { "auth" };
            
            IRestResponse response1 = RestCall("https://sandbox.plaid.com/sandbox/public_token/create", obj);
            PublicTokenResponce publicToken= JsonConvert.DeserializeObject<PublicTokenResponce>(response1.Content);

            PublicTokenExchangeParam objExPrm = new PublicTokenExchangeParam();
            objExPrm.ClientId = obj.ClientId;
            objExPrm.Secret = obj.Secret;
            objExPrm.PublicToken = publicToken.PublicToken;
            
            IRestResponse response2 = RestCall("https://sandbox.plaid.com/item/public_token/exchange", objExPrm);
            PublicTokenExchangeResponce publicTokenEx = JsonConvert.DeserializeObject<PublicTokenExchangeResponce>(response2.Content);


            HoldingReqParam objHoldPrm = new HoldingReqParam();
            objHoldPrm.ClientId = obj.ClientId;
            objHoldPrm.Secret = obj.Secret;
            objHoldPrm.AccessToken = publicTokenEx.AccessToken;

            IRestResponse response3 = RestCall("https://sandbox.plaid.com/investments/holdings/get", objHoldPrm);
            HoldingResponce HoldingDtl = JsonConvert.DeserializeObject<HoldingResponce>(response3.Content);

            //----------------------------



            return View(HoldingDtl.holdings);
        }
    }
}