using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using ArtGallery2.Models.Inventory;

namespace ArtGallery2.Controllers.Inventory
{
    public class FormController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static FormController()
        {
            HttpClientHandler handler = new HttpClientHandler() {
                AllowAutoRedirect = false
            };
            client = new HttpClient( handler );

            // Change this to match your own local port number.
            client.BaseAddress = new Uri( "https://localhost:44338/api/" );
            
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue( "application/json" ) );

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
        }

        // GET: Form
        public ActionResult Index()
        {
            return View();
        }

        // GET: Form/list
        public ActionResult List()
        {
            string url = "FormData/getForms";
            HttpResponseMessage response = client.GetAsync( url ).Result;
            if( response.IsSuccessStatusCode ) {
                IEnumerable<FormDto> forms = response.Content.ReadAsAsync<IEnumerable<FormDto>>().Result;
                return View( forms );
            } else {
                return RedirectToAction( "Error" );
            }
        }

        // GET: Form/details/5
        public ActionResult Details(int id)
        {
            string url = "FormData/findForm/" + id;
            HttpResponseMessage response = client.GetAsync( url ).Result;
            if( response.IsSuccessStatusCode ) {
                FormDto form = response.Content.ReadAsAsync<FormDto>().Result;
                return View( form );
            } else {
                return RedirectToAction( "Error" );
            }
        }

        // GET: Form/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Form/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create( Form form )
        {
            string url = "FormData/addForm";
            Debug.WriteLine( jss.Serialize( form ) );
            HttpContent content = new StringContent( jss.Serialize( form ) );
            content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );
            HttpResponseMessage response = client.PostAsync( url, content ).Result;

            if( response.IsSuccessStatusCode ) {

                int formId = 0;
                try {
                    formId = response.Content.ReadAsAsync<int>().Result;
                    return RedirectToAction( "Details", new {
                        id = formId
                    } );
                } catch( Exception e ) {
                    Debug.WriteLine( e );
                    return RedirectToAction( "List" );
                }


            } else {
                return RedirectToAction( "Error" );
            }
        }

        [HttpGet]
        public ActionResult Edit( int id )
        {
            string url = "FormData/findForm/" + id;
            HttpResponseMessage response = client.GetAsync( url ).Result;
            if( response.IsSuccessStatusCode ) {
                //Put data into data transfer object
                FormDto formDto = response.Content.ReadAsAsync<FormDto>().Result;
                return View( formDto );
            } else {
                return RedirectToAction( "Error" );
            }
        }

        // POST: Form/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit( int id, Form form )
        {
            string url = "FormData/updateForm/" + id;
            HttpContent content = new StringContent( jss.Serialize( form ) );
            content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );
            HttpResponseMessage response = client.PostAsync( url, content ).Result;
            if( response.IsSuccessStatusCode ) {
                return RedirectToAction( "Details", new {
                    id = id
                } );
            } else {
                return RedirectToAction( "Error" );
            }
        }

        // GET: Form/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // GET: Form/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm( int id )
        {
            string url = "FormData/findForm/" + id;
            HttpResponseMessage response = client.GetAsync( url ).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if( response.IsSuccessStatusCode ) {
                //Put data into player data transfer object
                FormDto formDto = response.Content.ReadAsAsync<FormDto>().Result;
                return View( formDto );
            } else {
                return RedirectToAction( "Error" );
            }
        }

        // POST: Form/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "FormData/deleteForm/" + id;

            HttpContent content = new StringContent( "" );
            HttpResponseMessage response = client.PostAsync( url, content ).Result;

            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if( response.IsSuccessStatusCode ) {
                return RedirectToAction( "List" );
            } else {
                return RedirectToAction( "Error" );
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
