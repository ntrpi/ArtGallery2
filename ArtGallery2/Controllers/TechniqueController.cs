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

namespace ArtGallery2.Controllers
{
    public class TechniqueController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static TechniqueController()
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

        // GET: Technique
        public ActionResult Index()
        {
            return RedirectToAction( "List" );
        }

        // GET: Technique/list
        public ActionResult List()
        {
            string url = "TechniqueData/getTechniques";
            HttpResponseMessage response = client.GetAsync( url ).Result;
            if( response.IsSuccessStatusCode ) {
                IEnumerable<TechniqueDto> techniques = response.Content.ReadAsAsync<IEnumerable<TechniqueDto>>().Result;
                return View( techniques );
            } else {
                return RedirectToAction( "Error" );
            }
        }



        // GET: Technique/Details/5
        public ActionResult Details( int id )
        {
            string url = "TechniqueData/findTechnique/" + id;
            HttpResponseMessage response = client.GetAsync( url ).Result;
            if( response.IsSuccessStatusCode ) {
                TechniqueDto technique = response.Content.ReadAsAsync<TechniqueDto>().Result;
                return View( technique );
            } else {
                return RedirectToAction( "Error" );
            }
        }

        // GET: Technique/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Technique/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create( Technique technique )
        {
            string url = "TechniqueData/addTechnique";
            Debug.WriteLine( jss.Serialize( technique ) );
            HttpContent content = new StringContent( jss.Serialize( technique ) );
            content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );
            HttpResponseMessage response = client.PostAsync( url, content ).Result;

            if( response.IsSuccessStatusCode ) {

                int techniqueId = 0;
                try {
                    techniqueId = response.Content.ReadAsAsync<int>().Result;
                    return RedirectToAction( "Details", new {
                        id = techniqueId
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
            string url = "TechniqueData/findTechnique/" + id;
            HttpResponseMessage response = client.GetAsync( url ).Result;
            if( response.IsSuccessStatusCode ) {
                //Put data into data transfer object
                TechniqueDto techniqueDto = response.Content.ReadAsAsync<TechniqueDto>().Result;
                return View( techniqueDto );
            } else {
                return RedirectToAction( "Error" );
            }
        }

        // POST: Technique/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit( int id, Technique technique )
        {
            string url = "TechniqueData/updateTechnique/" + id;
            HttpContent content = new StringContent( jss.Serialize( technique ) );
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

        // GET: Technique/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm( int id )
        {
            string url = "TechniqueData/findTechnique/" + id;
            HttpResponseMessage response = client.GetAsync( url ).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if( response.IsSuccessStatusCode ) {
                //Put data into player data transfer object
                TechniqueDto techniqueDto = response.Content.ReadAsAsync<TechniqueDto>().Result;
                return View( techniqueDto );
            } else {
                return RedirectToAction( "Error" );
            }
        }


        // POST: Technique/Delete/5
        [HttpPost]
        public ActionResult Delete( int id, FormCollection collection )
        {
            string url = "TechniqueData/deleteTechnique/" + id;

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
    }
}
