using HoldingDetails.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using Newtonsoft.Json;
using HoldingDetails.BL;

namespace HoldingDetails.Controllers
{
    public class HoldingController : Controller
    {
        static string ClientId = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientId"];
        static string Secret = System.Web.Configuration.WebConfigurationManager.AppSettings["Secret"];
        static string ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"];

        RestHelper helper = new RestHelper(ClientId, Secret, ApiUrl);

        public HoldingController()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
         
        }

        
      
        // GET: Holding
        public ActionResult Holding()
        {
            HoldingResponce holdingResponce = null;
            LinkTokenResponce linkResponse = null;
            List<HoldingDetails.Models.Holding> holdingList = null;
            string publicToken = string.Empty;

            linkResponse = helper.GetLinkToken();
            holdingResponce = helper.GetHoldings(ref publicToken);
            if(holdingResponce!=null && holdingResponce.holdings != null)
            {
                holdingList = holdingResponce.holdings;
            }
            //----------------------------

            ViewBag.publicToken = publicToken;
            ViewBag.ClientId = ClientId;
            ViewBag.Secret = Secret;
            ViewBag.ApiUrl = ApiUrl;

            if(linkResponse!=null)
            {
                ViewBag.LinkToken = linkResponse.LinkToken;
            }

            return View(holdingList);
        }
    }
}