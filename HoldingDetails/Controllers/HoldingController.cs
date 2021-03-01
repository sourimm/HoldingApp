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
using System.Linq;

namespace HoldingDetails.Controllers
{
    public class HoldingController : Controller
    {
        static string ClientId = System.Web.Configuration.WebConfigurationManager.AppSettings["ClientId"];
        static string Secret = System.Web.Configuration.WebConfigurationManager.AppSettings["Secret"];
        static string ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl"];
        PlaidEntities DB = new PlaidEntities();

        RestHelper helper = new RestHelper(ClientId, Secret, ApiUrl);

        public HoldingController()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        }

        

        // GET: Holding
        public ActionResult Holding()
        {
            if (Session["LinkToken"] == null || Session["PublicToken"] == null)
            {
                return RedirectToAction("Connection");
            }

            HoldingResponse holdingResponce = null;
            List<HoldingDetails.Models.Holding> holdingList = null;

            holdingResponce = helper.GetHoldings(Session["PublicToken"].ToString());
            if (holdingResponce != null)
            {
                if (!string.IsNullOrEmpty(holdingResponce.ErrorMessage))
                {
                    TempData["error"] = holdingResponce.ErrorMessage;
                }
                if (holdingResponce.holdings != null)
                {
                    holdingList = holdingResponce.holdings;
                }
            }


            //----------------------------

            ViewBag.publicToken = Session["PublicToken"];
            ViewBag.ClientId = ClientId;
            ViewBag.Secret = Secret;
            ViewBag.ApiUrl = ApiUrl;
            ViewBag.LinkToken = Session["LinkToken"];
            return View(holdingList);
        }
        
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(tblUser loginDtl)
        {
            using (PlaidEntities DB = new PlaidEntities())
            {
                loginDtl = DB.tblUsers.Where(x => x.LoginId == loginDtl.LoginId && x.Password == loginDtl.Password).FirstOrDefault();
                if (loginDtl != null)
                {
                    Session["LoginDtl"] = loginDtl;
                    return RedirectToAction("Connection");
                }

            }
            return View();
        }
        public ActionResult Connection()
        {
            if (Session["LoginDtl"] != null)
            {
                tblUser loginDtl = (tblUser)Session["LoginDtl"];
            }

            if (Session["LinkToken"] != null)
            {
                ViewBag.LinkToken = Session["LinkToken"];
            }
            else
            {
                LinkTokenResponse linkResponse = null;
                linkResponse = helper.GetLinkToken();
                if (linkResponse != null)
                {
                    ViewBag.LinkToken = linkResponse.LinkToken;
                    Session["LinkToken"] = linkResponse.LinkToken;
                }
            }
            using (PlaidEntities DB = new PlaidEntities())
            {
                List<tblInstance> Instances = DB.tblInstances.Select(x => x).ToList<tblInstance>();
                return View(Instances);
            }
        }
        [HttpPost]
        public ActionResult Connection(string ConnectionDtl,string Action)
        {
            if (!string.IsNullOrEmpty(Action) && Session["LoginDtl"] != null)
            {
                ActionClass obj = JsonConvert.DeserializeObject<ActionClass>(Action);
                if (obj.Action.Equals("Go"))
                {
                    using (PlaidEntities DB = new PlaidEntities())
                    {
                        string publictoken = DB.tblInstances.Where(x => x.ConnectionId == obj.Id).Select(x => x.PublicToken).FirstOrDefault().ToString();
                        Session["PublicToken"] = publictoken.Trim();
                        return RedirectToAction("Holding");
                    }
                }
                else if (obj.Action.Equals("Delete"))
                {
                    using (PlaidEntities DB = new PlaidEntities())
                    {
                        tblInstance RemoveInstances = DB.tblInstances.Where(x => x.ConnectionId == obj.Id).Select(x => x).FirstOrDefault();
                        if (RemoveInstances != null)
                        {
                            DB.tblInstances.Remove(RemoveInstances);
                            DB.SaveChanges();
                        }
                        List<tblInstance> Instances = DB.tblInstances.Select(x => x).ToList<tblInstance>();
                        return View(Instances);
                    }
                }
                else if(obj.Action.Equals("Save"))
                {
                    using (PlaidEntities DB = new PlaidEntities())
                    {
                        var result = DB.tblInstances.SingleOrDefault(b => b.ConnectionId == obj.Id);
                        if (result != null)
                        {
                            result.InstanceName = obj.InstanceName;
                            DB.SaveChanges();
                        }
                        List<tblInstance> Instances = DB.tblInstances.Select(x => x).ToList<tblInstance>();
                        return View(Instances);
                    }
                }
                else
                    return View();


            }
            else if (!string.IsNullOrEmpty(ConnectionDtl) && Session["LoginDtl"] != null)
            {
                tblInstance obj = JsonConvert.DeserializeObject<tblInstance>(ConnectionDtl);
                tblUser loginDtl = (tblUser)Session["LoginDtl"];
                obj.UserId = loginDtl.Id;
                using (PlaidEntities DB = new PlaidEntities())
                {
                    DB.tblInstances.Add(obj);
                    DB.SaveChanges();
                }
            }
            using (PlaidEntities DB = new PlaidEntities())
            {
                List<tblInstance> Instances = DB.tblInstances.Select(x => x).ToList<tblInstance>();
                return View(Instances);
            }
        }
    }
}
