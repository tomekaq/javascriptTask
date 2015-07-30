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
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Download(ModelFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GenerateFile(file.MaxValue, file.FileAmount);

                    var fileName = "GenerateFile.txt";

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
            catch
            {
                return View("Download");
            }

            return View();

        }

        public FileStream GenerateFile(string inputParam, string amount)
        {
            int maxVal = int.Parse(inputParam);
            int _amount = int.Parse(amount);

            using (BCRandomStream rndstream = new BCRandomStream(maxVal))
            {
                string path = Server.MapPath("~/GenerateFile.txt");
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (StreamWriter writeStream = new StreamWriter(fileStream))
                    {
                        for (var i = 0; i < _amount; i++)
                            writeStream.WriteLine(rndstream.Read());
                    }
                    return fileStream;

                }
            }
        }
    }
}


