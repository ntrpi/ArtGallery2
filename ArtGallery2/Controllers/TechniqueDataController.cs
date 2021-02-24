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
    public class TechniqueDataController: ApiController
    {
        private ArtGalleryDbContext db = new ArtGalleryDbContext();

        private PieceTechniqueDto getPieceTechniqueDto( PieceTechnique pieceTechnique )
        {
            PieceTechniqueDto pieceTechniqueDto = new PieceTechniqueDto {
                pieceTechniqueId = pieceTechnique.pieceTechniqueId,
                pieceId = pieceTechnique.pieceId,
                techniqueId = pieceTechnique.techniqueId
            };

            return pieceTechniqueDto;
        }

        public IEnumerable<PieceTechniqueDto> getPieceTechiqueDtosForPiece( int id )
        {
            List<PieceTechnique> techniques = db.pieceTechniques.Where( i => i.pieceId == id ).ToList();
            List<PieceTechniqueDto> pieceTechniques = new List<PieceTechniqueDto>();
            foreach( PieceTechnique pieceTechnique in techniques ) {
                pieceTechniques.Add( getPieceTechniqueDto( pieceTechnique ) );
            }
            return pieceTechniques;
        }

        // GET: api/TechniqueData/getTechniquesForPiece/{id}
        // Authorize annotation will block requests unless user is authorized
        // authorization process checks for valid cookies in request
        // [Authorize]
        // id == pieceId
        public IEnumerable<TechniqueDto> getTechniquesForPiece( int id )
        {
            List<PieceTechnique> techniques = db.pieceTechniques.Where( i => i.pieceId == id ).ToList();
            List<TechniqueDto> techniqueDtos = new List<TechniqueDto> { };

            //Here you can choose which inimageation is exposed to the API
            foreach( var technique in techniques ) {
                techniqueDtos.Add( getTechniqueDto( technique.techniqueId ) );
            }

            return techniqueDtos;
        }

        // id == pieceId
        public IEnumerable<TechniqueDto> getTechniquesNotForPiece( int id )
        {
            List<PieceTechnique> techniques = db.pieceTechniques.Where( i => i.pieceId != id ).ToList();
            List<TechniqueDto> techniqueDtos = new List<TechniqueDto> { };

            //Here you can choose which inimageation is exposed to the API
            foreach( var technique in techniques ) {
                techniqueDtos.Add( getTechniqueDto( technique.techniqueId ) );
            }

            return techniqueDtos;
        }

        // GET: api/TechniquesData/getTechniques
        // Authorize annotation will block requests unless user is authorized
        // authorization process checks for valid cookies in request
        //[Authorize]
        public IEnumerable<TechniqueDto> getTechniques()
        {
            List<Technique> techniques = db.techniques.ToList();
            List<TechniqueDto> techniqueDtos = new List<TechniqueDto> { };

            //Here you can choose which information is exposed to the API
            foreach( var technique in techniques ) {
                TechniqueDto techniqueDto = new TechniqueDto {
                    techniqueId = technique.techniqueId,
                    techniqueName = technique.techniqueName
                };
                techniqueDtos.Add( techniqueDto );
            }

            return techniqueDtos;
        }

        private TechniqueDto getTechniqueDto( int id )
        {
            Technique technique = db.techniques.Find( id );
            if( technique == null ) {
                return null;
            }
            //put into a 'friendly object format'
            TechniqueDto techniqueDto = new TechniqueDto {
                techniqueId = technique.techniqueId,
                techniqueName = technique.techniqueName
            };

            return techniqueDto;
        }

        // GET: api/TechniqueData/findTechnique/5
        [ResponseType( typeof( TechniqueDto ) )]
        [HttpGet]
        public IHttpActionResult findTechnique( int id )
        {
            //Find the data
            TechniqueDto technique = getTechniqueDto( id );
            //if not found, return 404 status code.
            if( technique == null ) {
                return NotFound();
            }

            //pass along data as 200 status code OK response
            return Ok( technique );
        }

        // POST: api/Techniques/addTechnique
        // FORM DATA: Technique JSON Object
        [ResponseType( typeof( Technique ) )]
        [HttpPost]
        public IHttpActionResult addTechnique( [FromBody] Technique technique )
        {
            //Will Validate according to data annotations specified on model
            if( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            db.techniques.Add( technique );
            db.SaveChanges();

            return CreatedAtRoute( "DefaultApi", new {
                id = technique.techniqueId
            }, technique );
        }

        [ResponseType( typeof( void ) )]
        [HttpPost]
        public IHttpActionResult updateTechnique( int id, [FromBody] Technique technique )
        {
            if( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            if( id != technique.techniqueId ) {
                return BadRequest();
            }

            db.Entry( technique ).State = EntityState.Modified;

            try {
                db.SaveChanges();

            } catch( DbUpdateConcurrencyException ) {
                if( !TechniqueExists( id ) ) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode( HttpStatusCode.NoContent );
        }


        // POST: api/Techniques/deleteTechnique/5
        [HttpPost]
        public IHttpActionResult deleteTechnique( int id )
        {
            Technique technique = db.techniques.Find( id );
            if( technique == null ) {
                return NotFound();
            }

            db.techniques.Remove( technique );
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing ) {
                db.Dispose();
            }
            base.Dispose( disposing );
        }

        private bool TechniqueExists( int id )
        {
            return db.techniques.Count( e => e.techniqueId == id ) > 0;
        }
    }
}