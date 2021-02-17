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
    public class MediaDataController : ApiController
    {
        private ArtGalleryDbContext db = new ArtGalleryDbContext();

        // GET: api/MediasData/getMedias
        // Authorize annotation will block requests unless user is authorized
        // authorization process checks for valid cookies in request
        [Authorize]
        public IEnumerable<MediaDto> getMedias()
        {
            List<Media> medias = db.medias.ToList();
            List<MediaDto> mediaDtos = new List<MediaDto> { };

            //Here you can choose which information is exposed to the API
            foreach( var media in medias ) {
                MediaDto mediaDto = new MediaDto {
                    mediaId = media.mediaId,
                    mediaName = media.mediaName,
                };
                mediaDtos.Add( mediaDto );
            }

            return mediaDtos;
        }

        // GET: api/MediaData/findMedia/5
        [ResponseType( typeof( MediaDto ) )]
        [HttpGet]
        public IHttpActionResult findMedia( int id )
        {
            //Find the data
            Media media = db.medias.Find( id );
            //if not found, return 404 status code.
            if( media == null ) {
                return NotFound();
            }

            //put into a 'friendly object format'
            MediaDto mediaDto = new MediaDto {
                mediaId = media.mediaId,
                mediaName = media.mediaName,
            };

            //pass along data as 200 status code OK response
            return Ok( mediaDto );
        }

        // POST: api/Medias/addMedia
        // FORM DATA: Media JSON Object
        [ResponseType( typeof( Media ) )]
        [HttpPost]
        public IHttpActionResult addMedia( [FromBody] Media media )
        {
            //Will Validate according to data annotations specified on model
            if( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            db.medias.Add( media );
            db.SaveChanges();

            return CreatedAtRoute( "DefaultApi", new {
                id = media.mediaId
            }, media );
        }

        // POST: api/Medias/deleteMedia/5
        [HttpPost]
        public IHttpActionResult deleteMedia( int id )
        {
            Media media = db.medias.Find( id );
            if( media == null ) {
                return NotFound();
            }

            db.medias.Remove( media );
            db.SaveChanges();

            return Ok();
        }

        // PUT: api/Media/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMedia(int id, Media media)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != media.mediaId)
            {
                return BadRequest();
            }

            db.Entry(media).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaExists(id))
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

        private bool MediaExists(int id)
        {
            return db.medias.Count(e => e.mediaId == id) > 0;
        }
    }
}