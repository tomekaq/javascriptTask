using ServerFile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1;

namespace JavaScriptTask.Controllers
{
    public class HomeController : Controller
    {

        static Queue<ModelFile> requestQueue;
        static List<Task> tasks;
        static List<string> downloadList;

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
                    response.TransmitFile(Server.MapPath("~/" + fileName));
                    response.Flush();
                    response.End();
                }

            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
            return Json(new { success = true, fileName }, JsonRequestBehavior.AllowGet); ;
        }

        public string GenerateFile(ModelFile file)
        {
            try
            {
                int maxVal = int.Parse(file.MaxValue);
                Int64 _amount = Int64.Parse(file.FileAmount);

                using (BCRandomStream rndstream = new BCRandomStream(maxVal + 1))
                {
                    string fName = "GenerateFile" + file.Id + ".txt";
                    string path = Server.MapPath("~/" + fName);
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
            catch
            {
                return null;
            }
        }

        public JsonResult RequestQueue(ModelFile file)
        {
            if (requestQueue == null)
            {
                requestQueue = new Queue<ModelFile>();
            }
            if (tasks == null)
            {
                tasks = new List<Task>();
            }
            if (downloadList == null)
            {
                downloadList = new List<string>();
            }
            requestQueue.Enqueue(file);
            var t = Task.Run(() =>
            {
                var ts = GenerateFile(file);
                downloadList.Add(ts);
            });
            tasks.Add(t);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MyRequests(string file)
        {
            try
            {
                if (downloadList.Contains(file))
                {
                    var filePath = downloadList.First();
                    return Json(new { success = true, filePath }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { success = false });
            }
            catch (Exception e)
            {
                var glg = e.Message;
            }
            return null;
        }

        public JsonResult SendDownload(string file)
        {
            try
            {
                downloadList.Remove(file);
                System.IO.File.Delete(Server.MapPath("~/" + file));
                return Json(new { success = true });
            }
            catch
            {
                return null;
            }

        }
    }
}


