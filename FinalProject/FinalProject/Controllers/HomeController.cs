using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        //TODO: pull all 6 stats
        //TODO: pull sprites

        public ActionResult Index()
        {
            HttpWebRequest WR = WebRequest.CreateHttp("https://pokeapi.co/api/v2/pokemon/6/");
            WR.UserAgent = ".NET Framework Test Client";

            HttpWebResponse Response;

            try
            {
                Response = (HttpWebResponse)WR.GetResponse();
            }
            catch (WebException e)
            {
                ViewBag.Error = "Exception";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                ViewBag.Error = Response.StatusCode;
                ViewBag.ErrorDescription = Response.StatusDescription;
                return View();
            }

            StreamReader reader = new StreamReader(Response.GetResponseStream());
            string PokemonData = reader.ReadToEnd();

            try
            {
                JObject JsonData = JObject.Parse(PokemonData);
                ViewBag.Name = JsonData["forms"][0];
                string name = ViewBag.Name.name;
                ViewBag.NameProp = UppercaseFirst(name);

                ViewBag.Speed = JsonData["stats"][0];
                ViewBag.SpecialDef = JsonData["stats"][1];
                ViewBag.SpecialAttack = JsonData["stats"][2];
                ViewBag.Def = JsonData["stats"][3];
                ViewBag.Attack = JsonData["stats"][4];
                ViewBag.HP = JsonData["stats"][5];
                ViewBag.Image = JsonData["sprites"];

                string specialAtt = (string)JsonData["stats"][2]["base_stat"];
                string att = (string)JsonData["stats"][4]["base_stat"];
                string speed = (string)JsonData["stats"][0]["base_stat"];
                string specialDef = (string)JsonData["stats"][1]["base_stat"];
                string def = (string)JsonData["stats"][3]["base_stat"];

                double SA = StatConverter(specialAtt);
                double A = (int.Parse(att))/3;
                double S = (int.Parse(speed))/3;
                double SD = (int.Parse(specialDef))/3;
                double D = (int.Parse(def))/3;


                ViewBag.ThreePoint = SA;
                ViewBag.FieldGoal = A;
                ViewBag.Paint = S;
                ViewBag.Steal = SD;
                ViewBag.Block = D;
            }
            catch (Exception e)
            {
                ViewBag.Error = "JSON Issue";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        static int StatConverter(string x)
        {
            return (int.Parse(x)) / 3;
        }
    }
}