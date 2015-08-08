using ServerFile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebApplication1;

namespace JavaScriptTask.Controllers
{
    public class HomeController : Controller
    {

        static Queue<ModelFile> requestQueue;
        static Queue<JsonResult> responseQueue;

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Download(string fileName)
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
            return Json(new { success = true, fileName }, JsonRequestBehavior.AllowGet);;
        }

        public string GenerateFile(ModelFile file)
        {
            int maxVal = int.Parse(file.MaxValue);
            Int64 _amount = Int64.Parse(file.FileAmount);

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
                   
                    
                    return fName;
                }
            }
        }
        public JsonResult DownloadQueue()
        {
            if (responseQueue.Count != 0)
            {
                return Json(responseQueue.First());
            } 
            return null;
        }

        public void GenerateQueue()
        {
            try
            {
                if (responseQueue == null)
                {
                    responseQueue = new Queue<JsonResult>();
                }

                if (requestQueue.Count != 0)
                {
                    var peek = requestQueue.First();
                    var t = GenerateFile(peek);
                    var down = Download(t); 
                    requestQueue.Dequeue();
                    responseQueue.Enqueue(down);                    
                }
                else
                {
                  Thread.Sleep(5000);
                }
              
            }
            catch(Exception e)
            {
            }
        }

        public ActionResult RequestQueue(ModelFile file){

            if (requestQueue == null)
            {
                requestQueue = new Queue<ModelFile>();
            }
            requestQueue.Enqueue(file);

            return Json(new{success = true});
        }
    }


}


