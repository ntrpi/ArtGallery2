using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ArtGallery2.Models;
using ArtGallery2.Models.Inventory;

namespace ArtGallery2.Controllers
{
    public class ImageDataController : ApiController
    {
        private ArtGalleryDbContext db = new ArtGalleryDbContext();

        // GET: api/ImagesData/getImages
        // Authorize annotation will block requests unless user is authorized
        // authorization process checks for valid cookies in request
        [Authorize]
        public IEnumerable<ImageDto> getImages()
        {
            List<Image> images = db.images.ToList();
            List<ImageDto> imageDtos = new List<ImageDto> { };

            //Here you can choose which information is exposed to the API
            foreach( var image in images ) {
                ImageDto imageDto = new ImageDto {
                    imageId = image.imageId,
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
        public IHttpActionResult addImage( [FromBody] Image image )
        {
            //Will Validate according to data annotations specified on model
            if( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            db.images.Add( image );
            db.SaveChanges();

            return CreatedAtRoute( "DefaultApi", new {
                id = image.imageId
            }, image );
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

        // PUT: api/Images/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutImage(int id, Image image)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != image.imageId)
            {
                return BadRequest();
            }

            db.Entry(image).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
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