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
using ArtGallery2.Models.ViewModels;

namespace ArtGallery2.Controllers.Inventory
{
    public class PieceController: Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static PieceController()
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

        // GET: Piece
        public ActionResult Index()
        {
            return View( List() );
        }

        // GET: Piece/list
        public ActionResult List()
        {
            string url = "PieceData/getPieces";
            HttpResponseMessage response = client.GetAsync( url ).Result;
            if( response.IsSuccessStatusCode ) {
                IEnumerable<PieceDto> forms = response.Content.ReadAsAsync<IEnumerable<PieceDto>>().Result;
                return View( forms );
            } else {
                return RedirectToAction( "Error" );
            }
        }

        private FormDto getFormDto( int id )
        {
            string url = "FormData/findForm/" + id;
            HttpResponseMessage formResponse = client.GetAsync( url ).Result;
            if( formResponse.IsSuccessStatusCode ) {
                FormDto formDto = formResponse.Content.ReadAsAsync<FormDto>().Result;
                return formDto;
            }

            // If not successful, return null.
            return null;
        }

        private IEnumerable<FormDto> getFormDtos()
        {
            string url = "FormData/getForms";
            HttpResponseMessage response = client.GetAsync( url ).Result;
            if( response.IsSuccessStatusCode ) {
                IEnumerable<FormDto> forms = response.Content.ReadAsAsync<IEnumerable<FormDto>>().Result;
                return forms;
            }
            return null;
        }

            private PieceDto getPieceDto( int id )
        {
            string url = "PieceData/findPiece/" + id;
            HttpResponseMessage response = client.GetAsync( url ).Result;

            if( response.IsSuccessStatusCode ) {
                //Put data into player data transfer object
                PieceDto pieceDto = response.Content.ReadAsAsync<PieceDto>().Result;
                return pieceDto;
            }
            return null;
        }

        private ShowPiece getShowPiece( int id )
        {
            ShowPiece piece = new ShowPiece();

            PieceDto pieceDto = getPieceDto( id );

            // If not successful, return null.
            if( pieceDto == null ) {
                return null;
            }

            piece.piece = pieceDto;
            piece.form = getFormDto( pieceDto.formId );
            return piece;
        }

        private UpdatePiece getUpdatePiece( int id )
        {
            UpdatePiece piece = new UpdatePiece();
            PieceDto pieceDto = getPieceDto( id );
            // If not successful, return null.
            if( pieceDto == null ) {
                return null;
            }

            piece.piece = pieceDto;
            piece.forms = getFormDtos();
            return piece;
        }

        // GET: Piece/details/5
        public ActionResult Details( int id )
        {
            ShowPiece piece = getShowPiece( id );
            if( piece == null ) {
                return RedirectToAction( "Error" );
            }
            return View( piece );
        }

        // GET: Piece/Create
        public ActionResult Create()
        {
            UpdatePiece piece = new UpdatePiece();
            piece.forms = getFormDtos();
            return View( piece );

            // TODO: deal with null forms.
        }

        // POST: Piece/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create( Piece piece )
        {
            string url = "PieceData/addPiece";
            Debug.WriteLine( jss.Serialize( piece ) );
            HttpContent content = new StringContent( jss.Serialize( piece ) );
            content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );
            HttpResponseMessage response = client.PostAsync( url, content ).Result;

            if( response.IsSuccessStatusCode ) {

                string jsonContent = response.Content.ReadAsStringAsync().Result;
                PieceDto pieceDto = jss.Deserialize<PieceDto>( jsonContent );
                return RedirectToAction( "Details", new {
                    id = pieceDto.pieceId
                } );

                // The ReadAsAsync kept throwing a System.AggregateException
                //try {
                //    pieceId = response.Content.ReadAsAsync<int>().Result;
                //    return RedirectToAction( "Details", new {
                //        id = pieceId
                //    } );
                //} catch( Exception e ) {
                //    Debug.WriteLine( e );
                //    return RedirectToAction( "List" );
                //}


            } else {
                return RedirectToAction( "Error" );
            }
        }

        [HttpGet]
        public ActionResult Edit( int id )
        {
            UpdatePiece piece = new UpdatePiece();
            piece.piece = getPieceDto( id );
            if( piece == null ) {
                return RedirectToAction( "Error" );
            }

            piece.forms = getFormDtos();
            return View( piece );
        }

        // POST: Piece/edit/5
        [HttpPost]
        public ActionResult Edit( int id, Piece piece )
        {
            string url = "PieceData/updatePiece/" + id;
            HttpContent content = new StringContent( jss.Serialize( piece ) );
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


        // GET: Piece/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm( int id )
        {
            ShowPiece piece = getShowPiece( id );
            if( piece == null ) {
                return RedirectToAction( "Error" );
            }
            return View( piece );
        }

        // POST: Piece/Delete/5
        [HttpPost]
        public ActionResult Delete( int id )
        {
            string url = "PieceData/deletePiece/" + id;

            HttpContent content = new StringContent( "" );
            HttpResponseMessage response = client.PostAsync( url, content ).Result;

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
