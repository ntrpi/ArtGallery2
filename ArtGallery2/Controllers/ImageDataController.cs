using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Description;
using ArtGallery2.Models;
using ArtGallery2.Models.Inventory;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace ArtGallery2.Controllers
{
    public class ImageDataController : ApiController
    {
        private ArtGalleryDbContext db = new ArtGalleryDbContext();

        private JavaScriptSerializer jss = new JavaScriptSerializer();

        // GET: api/ImagesData/getImages
        // Authorize annotation will block requests unless user is authorized
        // authorization process checks for valid cookies in request
        // [Authorize]
        public IEnumerable<ImageDto> getImages( int id )
        {
            List<Image> images = db.images.Where( i => i.pieceId == id ).ToList();
            List<ImageDto> imageDtos = new List<ImageDto> { };

            //Here you can choose which information is exposed to the API
            foreach( var image in images ) {
                ImageDto imageDto = new ImageDto {
                    imageId = image.imageId,
                    imageName = image.imageName,
                    imageExt = image.imageExt,
                    imagePath = image.imagePath,
                    pieceId = image.pieceId
                };
                imageDtos.Add( imageDto );
            }

            return imageDtos;
        }

        // GET: api/ImageData/findImage/5
        [ResponseType( typeof( ImageDto ) )]
        [HttpGet]
        public IHttpActionResult findImage( int id )
        {
            //Find the data
            Image image = db.images.Find( id );
            //if not found, return 404 status code.
            if( image == null ) {
                return NotFound();
            }

            //put into a 'friendly object format'
            ImageDto imageDto = new ImageDto {
                imageId = image.imageId,
                imageName = image.imageName,
                imageExt = image.imageExt,
                imagePath = image.imagePath,
                pieceId = image.pieceId
            };

            //pass along data as 200 status code OK response
            return Ok( imageDto );
        }

        // POST: api/Images/addImage
        // FORM DATA: Image JSON Object
        [ResponseType( typeof( Image ) )]
        [HttpPost]
        public IHttpActionResult addImage( int id )
        {
            // Check that the HttpContext contains more than one part.
            if( !Request.Content.IsMimeMultipartContent() ) {
                return new TextResult( "Missing image file.", Request );
            }
            Debug.WriteLine( "Received multipart form data." );

            //Check if a file is posted
            int numfiles = HttpContext.Current.Request.Files.Count;
            if( numfiles != 1 || HttpContext.Current.Request.Files[ 0 ] == null ) {
                return new TextResult( "No file posted.", Request );
            }
            Debug.WriteLine( "Files Received: " + numfiles );

            var imageFile = HttpContext.Current.Request.Files[ 0 ];
            // Check if the file is empty.
            if( imageFile.ContentLength == 0 ) {
                return new TextResult( "File is emtpy.", Request );
            }

            // Create an image object.
            Image image = new Image();
            image.pieceId = id;

            // Check file extension.
            var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
            image.imageExt = Path.GetExtension( imageFile.FileName ).Substring( 1 );
            if( !valtypes.Contains( image.imageExt ) ) {
                return new TextResult( "File does not have an image extension.", Request );
            }

            try {
                // Create a file name.
                image.imageName = "pieceId_" + image.pieceId + "_"
                    + Stopwatch.GetTimestamp();

                // Create a complete file path.
                image.imagePath = Path.Combine( HttpContext.Current.Server.MapPath( "~/Content/Images/" ), image.imageName + '.' + image.imageExt );

                // Save the image.
                imageFile.SaveAs( image.imagePath );

            } catch( Exception e ) {
                Debug.WriteLine( "Image was not saved successfully." );
                Debug.WriteLine( "Exception:" + e );

                return new TextResult( "Unable to save file.", Request );
            }

            //Will Validate according to data annotations specified on model
            if( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            db.images.Add( image );
            db.SaveChanges();

            // Not using this right now, but leaving it because I plan to.
            return Ok( image.imageId );
        }

        // POST: api/Images/deleteImage/5
        [HttpPost]
        public IHttpActionResult deleteImage( int id )
        {
            Image image = db.images.Find( id );
            if( image == null ) {
                return NotFound();
            }

            db.images.Remove( image );
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ImageExists(int id)
        {
            return db.images.Count(e => e.imageId == id) > 0;
        }
    }
}