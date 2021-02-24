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

        public ActionResult AddConnection()
        {
            Session.RemoveAll();
            return RedirectToAction("Index");
        }


        // GET: Holding
        public ActionResult Holding()
        {
            if (Session["LinkToken"] == null || Session["PublicToken"] == null)
            {
                return RedirectToAction("Index");
            }

            HoldingResponse holdingResponce = null;
            List<HoldingDetails.Models.Holding> holdingList = null;

            holdingResponce = helper.GetHoldings(Session["PublicToken"].ToString());
            if (holdingResponce != null && holdingResponce.holdings != null)
            {
                holdingList = holdingResponce.holdings;
            }
            //----------------------------

            ViewBag.publicToken = Session["PublicToken"];
            ViewBag.ClientId = ClientId;
            ViewBag.Secret = Secret;
            ViewBag.ApiUrl = ApiUrl;
            ViewBag.LinkToken = Session["LinkToken"];
            return View(holdingList);
        }

        public ActionResult Index()
        {
            if (Session["LinkToken"] != null && Session["PublicToken"] != null)
            {
                return RedirectToAction("Holding");
            }


            LinkTokenResponse linkResponse = null;
            linkResponse = helper.GetLinkToken();
            if (linkResponse != null)
            {
                ViewBag.LinkToken = linkResponse.LinkToken;
                Session["LinkToken"] = linkResponse.LinkToken;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(string publictoken)
        {
            Session["PublicToken"] = publictoken;
            return RedirectToAction("Holding");
        }
    }
}
