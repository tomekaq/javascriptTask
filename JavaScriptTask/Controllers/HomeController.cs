using ServerFile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1;

namespace JavaScriptTask.Controllers
{
    public class HomeController : Controller
    {

        static Queue<string> requestQueue;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Download(string fileName)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                    response.ClearContent();
                    response.Clear();
                    response.ContentType = "text/plain";
                    response.AddHeader("Content-Disposition",
                                       "attachment; filename=" + fileName + ";");
                    response.TransmitFile(Server.MapPath("~/GenerateFile.txt"));
                    response.Flush();
                    response.End();

                }
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
            return null;
        }

        public JsonResult GenerateFile(ModelFile file)
        {
            int maxVal = int.Parse(file.MaxValue);
            int _amount = int.Parse(file.FileAmount);

            if (requestQueue == null)
            {
                requestQueue = new Queue<string>();
            }

            using (BCRandomStream rndstream = new BCRandomStream(maxVal + 1))
            {
                string fName = "GenerateFile" + file.Id + ".txt";
                string path = Server.MapPath("~/GenerateFile" + file.Id + ".txt");
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (StreamWriter writeStream = new StreamWriter(fileStream))
                    {
                        for (var i = 0; i < _amount; i++)
                            writeStream.WriteLine(rndstream.Read());
                    }
                    string json = "{success = true," + fName + " id =" + file.Id + " }";
                    requestQueue.Enqueue(json);
                    return Json(new { success = true, fName }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }


}


