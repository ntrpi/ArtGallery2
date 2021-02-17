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
    public class FormDataController : ApiController
    {
        private ArtGalleryDbContext db = new ArtGalleryDbContext();

        // GET: api/FormsData/getForms
        // Authorize annotation will block requests unless user is authorized
        // authorization process checks for valid cookies in request
        //[Authorize]
         public IEnumerable<FormDto> getForms()
        {
            List<Form> forms = db.forms.ToList();
            List<FormDto> formDtos = new List<FormDto> {};

            //Here you can choose which information is exposed to the API
            foreach( var form in forms ) {
                FormDto formDto = new FormDto {
                    formId = form.formId,
                    formName = form.formName
                };
                formDtos.Add( formDto );
            }

            return formDtos;
        }

        [HttpGet]
        public string test()
        {
            return "hi";
        }

        /// <summary>
        /// Finds a particular form in the database with a 200 status code. If the form is not found, return 404.
        /// </summary>
        /// <param name="id">The form id</param>
        /// <returns>Information about the form, including form id, bio, first and last name, and teamid</returns>
        // <example>
        // GET: api/FormData/findForm/5
        // </example>
        [HttpGet]
        [ResponseType( typeof( FormDto ) )]
        public IHttpActionResult findForm( int id )
        {
            //Find the data
            Form form = db.forms.Find( id );
            //if not found, return 404 status code.
            if( form == null ) {
                return NotFound();
            }

            //put into a 'friendly object format'
            FormDto formDto = new FormDto {
                formId = form.formId,
                formName = form.formName
            };

            //pass along data as 200 status code OK response
            return Ok( formDto );
        }


        // POST: api/Forms/addForm
        // FORM DATA: Form JSON Object
        [ResponseType( typeof( Form ) )]
        [HttpPost]
        public IHttpActionResult addForm( [FromBody] Form form )
        {
            //Will Validate according to data annotations specified on model
            if( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            db.forms.Add( form );
            db.SaveChanges();

            return CreatedAtRoute( "DefaultApi", new {
                id = form.formId
            }, form );
        }

        // POST: api/Forms/deleteForm/5
        [HttpPost]
        public IHttpActionResult deleteForm( int id )
        {
            Form form = db.forms.Find( id );
            if( form == null ) {
                return NotFound();
            }

            db.forms.Remove( form );
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Updates a form in the database given information about the form.
        /// </summary>
        /// <param name="id">The form id</param>
        /// <param name="form">A form object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/FormsData/updateForm/5
        /// FORM DATA: Form JSON Object
        /// </example>
        [ResponseType( typeof( void ) )]
        [HttpPost]
        public IHttpActionResult updateForm( int id, [FromBody] Form form )
        {
            if( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            if( id != form.formId ) {
                return BadRequest();
            }

            db.Entry( form ).State = EntityState.Modified;

            try {
                db.SaveChanges();

            } catch( DbUpdateConcurrencyException ) {
                if( !FormExists( id ) ) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode( HttpStatusCode.NoContent );
        }


        // POST: api/FormsData
        [ResponseType(typeof(Form))]
        public IHttpActionResult postForm( Form form )
        {
            if( !ModelState.IsValid ) {
                return BadRequest(ModelState);
            }

            db.forms.Add(form);
            db.SaveChanges();

            return CreatedAtRoute( "DefaultApi", new { id = form.formId }, form );
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing ) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FormExists( int id )
        {
            return db.forms.Count(e => e.formId == id) > 0;
        }
    }
}