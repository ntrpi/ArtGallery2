using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using ArtGallery2.Models.ViewModels;
using ArtGallery2.Models.Inventory;

namespace ArtGallery2.Controllers
{
    public class ImageController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static ImageController()
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

        // GET: Image
        // List pieces to get an id.
        public ActionResult Index()
        {
            return View();
        }

        private ImageDto getImageDto( int imageId )
        {
            string url = "ImageData/findImage/" + imageId;
            HttpResponseMessage response = client.GetAsync( url ).Result;
            if( response.IsSuccessStatusCode ) {
                return response.Content.ReadAsAsync<ImageDto>().Result;
            }
            return null;
        }

        private Image getImage( int imageId )
        {
            string url = "ImageData/findImage/" + imageId;
            HttpResponseMessage response = client.GetAsync( url ).Result;
            if( response.IsSuccessStatusCode ) {
                return response.Content.ReadAsAsync<Image>().Result;
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

        private IEnumerable<ImageDto> getPieceImages( int id )
        {
            string url = "ImageData/getImagesForPiece/" + id;
            HttpResponseMessage response = client.GetAsync( url ).Result;
            if( response.IsSuccessStatusCode ) {
                IEnumerable<ImageDto> images = response.Content.ReadAsAsync<IEnumerable<ImageDto>>().Result;
                return images;
            }
            return null;
        }

        private ShowImages getShowImages( int id )
        {
            ShowImages showImages = new ShowImages();
            showImages.piece = getPieceDto( id );
            if( showImages.piece == null ) {
                return null;
            }

            showImages.images = getPieceImages( id );
            if( showImages.images == null ) {
                return null;
            }
            return showImages;
        }

        private ShowImage getShowImage( int id )
        {
            ShowImage showImage = new ShowImage();
            showImage.image = getImageDto( id );
            if( showImage.image == null ) {
                return null;
            }

            showImage.showImages = new ShowImages();
            showImage.showImages.piece = getPieceDto( showImage.image.pieceId );
            if( showImage.showImages.piece == null ) {
                return null;
            }

            showImage.showImages.images = getPieceImages( showImage.showImages.piece.pieceId );
            if( showImage.showImages.images == null ) {
                return null;
            }

            return showImage;
        }

        // Get: Image/Images/5
        [HttpGet]
        public ActionResult Images( int id )
        {
            ShowImages showImages = getShowImages( id );
            if( showImages == null ) {
                return RedirectToAction( "Error" );
            }
            return View( showImages );
        }



        // GET: Image/Details/5
        public ActionResult Details(int id)
        {
            ShowImage showImage = getShowImage( id );
            if( showImage == null ) {
                return RedirectToAction( "Error" );
            }
            return View( showImage );
        }

        // GET: Image/Create

        public ActionResult Create( int id )
        {
            return View( getPieceDto( id ) );
        }

        // POST: Image/Create
        [HttpPost]
        public ActionResult Create( int id, HttpPostedFileBase imageData )
        {
            Debug.WriteLine( "Received image for piece id " + id );

            //Send over image data for player
            string url = "ImageData/addImage/" + id;

            MultipartFormDataContent requestcontent = new MultipartFormDataContent();
            HttpContent imagecontent = new StreamContent( imageData.InputStream );
            requestcontent.Add( imagecontent, "pieceImage", imageData.FileName );
            HttpResponseMessage response = client.PostAsync( url, requestcontent ).Result;

            if( response.IsSuccessStatusCode ) {

                return RedirectToAction( "Images", new {
                    id = id
                } );

            } else {
                // TODO: Get the error message from the response.
                return RedirectToAction( "Index" );
            }
        }

        //// GET: Image/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Image/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        private bool updateImage( Image image )
        {
            string url = "ImageData/updateImage/" + image.imageId;
            HttpContent content = new StringContent( jss.Serialize( image ) );
            content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );
            HttpResponseMessage response = client.PostAsync( url, content ).Result;
            return response.IsSuccessStatusCode;
        }

        // POST: Image/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SetMainImage(int id, FormCollection collection)
        {
            Image image = getImage( id );
            image.isMainImage = true;
            if( !updateImage( image ) ) {
                return RedirectToAction( "Error" );
            }

            int mainImageId = Int32.Parse( collection[ "mainImageId" ] );
            if( mainImageId != 0 ) {
                image = getImage( mainImageId );
                image.isMainImage = false;
                if( !updateImage( image ) ) {
                    return RedirectToAction( "Error" );
                }
            }

            return RedirectToAction( "Details", new {
                id = id
            } );
        }


        // GET: Image/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // GET: Image/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm( int id )
        {
            ShowImage showImage = getShowImage( id );
            if( showImage == null ) {
                return RedirectToAction( "Error" );
            }
            return View( showImage );
        }

        // POST: Image/Delete/5
        [HttpPost]
        public ActionResult Delete( int id, FormCollection collection )
        {
            string url = "ImageData/deleteImage/" + id;
            HttpContent content = new StringContent( "" );
            HttpResponseMessage response = client.PostAsync( url, content ).Result;
            if( response.IsSuccessStatusCode ) {
                return RedirectToAction( "Images", new {
                    id = collection[ "pieceId" ]
                } );
            } else {
                return RedirectToAction( "Error" );
            }
        }
    }
}
