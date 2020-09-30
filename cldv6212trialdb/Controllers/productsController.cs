using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Amazon.IdentityManagement.Model;
using Azure.Storage.Blobs;
using cldv6212trialdb;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace cldv6212trialdb.Controllers
{
    public class productsController : Controller
    {
        //ImageService imageService = new ImageService();
        private cldv6212task2Entities db = new cldv6212task2Entities();
        private readonly IConfiguration _configuration;

        public productsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // GET: products
        public ActionResult Index()
        {
            return View(db.products.ToList());
        }

        // GET: products/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: products/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Index(List<IFormFile> files)
        {
            var filePaths = new List<string>();
            foreach (var formfile in files)
            {
                if (formfile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), formfile.FileName);
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formfile.CopyToAsync(stream);
                    }
                }
            }
            return View();
        }
        // POST: products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public  ActionResult Create([Bind(Include = "id,item_descr_and_name,item_price,item_image")] product product)
        //{
        //    if (ModelState.IsValid)
        //    {
                
        //        BlobServiceClient objBlobService = new BlobServiceClient(_configuration.GetConnectionString("AzureStorage"));
        //        int id = product.id;
        //        db.products.Add(product);
        //        db.SaveChanges();
        //        if (product.item_image != null)
        //        {
        //            string category = "car";
        //            var fileName = "car-image.jpg";
                    
        //            byte[] fileData = new byte[product.item_image.Length];
        //            string mimeType = product.item_image.ContentType;
                    //var imageUrl = await imageService.UploadImageAsync(photo);
                    //TempData["LatestImage"] = imageUrl.ToString();
                    //return RedirectToAction("LatestImage");

                    //    product.ImagePath = objBlobService.CreateBlobContainer(
                    //        category,
                    //        id,
                    //        fileName,
                    //        fileData,
                    //        mimeType); 
                    //}
        //            return RedirectToAction("Index");
        //    }

        //    return View(product);
        //}

        // GET: products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,item_descr_and_name,item_price,item_image")] product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: products/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product); 
        }

        // POST: products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //private async Task<string> UploadFileToBlobAsync(string category, int id, string strFileName, byte[] fileData, string fileMimeType)
        //{
        //    try
        //    {
        //        string strContainerName = "uploads";
        //        string fileName = category + "/" + id + "/" + strFileName;

        //        CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(AccessKey);
        //        CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        //        CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

        //        if (await cloudBlobContainer.CreateIfNotExistsAsync().ConfigureAwait(false))
        //        {
        //            await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }).ConfigureAwait(false);
        //        }

        //        if (fileName != null && fileData != null)
        //        {
        //            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
        //            cloudBlockBlob.Properties.ContentType = fileMimeType;
        //            await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length).ConfigureAwait(false);
        //            return cloudBlockBlob.Uri.AbsoluteUri;
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
