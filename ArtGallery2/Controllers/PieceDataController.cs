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
    public class PieceDataController : ApiController
    {
        private ArtGalleryDbContext db = new ArtGalleryDbContext();

        // GET: api/PieceData/getPieces
        // Authorize annotation will block requests unless user is authorized
        // authorization process checks for valid cookies in request
        //[Authorize]
        public IEnumerable<PieceDto> getPieces()
        {
            List<Piece> pieces = db.pieces.ToList();
            List<PieceDto> pieceDtos = new List<PieceDto> { };

            //Here you can choose which information is exposed to the API
            foreach( var piece in pieces ) {
                PieceDto pieceDto = new PieceDto {
                    pieceId = piece.pieceId,
                    pieceName = piece.pieceName,
                    pieceDescription = piece.pieceDescription,
                    height = piece.height,
                    length = piece.length,
                    width = piece.width,
                    piecePrice = piece.piecePrice,
                    formId = piece.formId
                };
                pieceDtos.Add( pieceDto );
            }

            return pieceDtos;
        }

        // GET: api/PieceData/findPiece/5
        [ResponseType( typeof( PieceDto ) )]
        [HttpGet]
        public IHttpActionResult findPiece( int id )
        {
            //Find the data
            Piece piece = db.pieces.Find( id );
            //if not found, return 404 status code.
            if( piece == null ) {
                return NotFound();
            }

            //put into a 'friendly object format'
            PieceDto pieceDto = new PieceDto {
                pieceId = piece.pieceId,
                pieceName = piece.pieceName,
                pieceDescription = piece.pieceDescription,
                height = piece.height,
                length = piece.length,
                width = piece.width,
                piecePrice = piece.piecePrice,
                formId = piece.formId
            };

            //pass along data as 200 status code OK response
            return Ok( pieceDto );
        }

        // POST: api/Pieces/addPiece
        // FORM DATA: Piece JSON Object
        [ResponseType( typeof( Piece ) )]
        [HttpPost]
        public IHttpActionResult addPiece( [FromBody] Piece piece )
        {
            //Will Validate according to data annotations specified on model
            if( !ModelState.IsValid ) {
                return BadRequest( ModelState );
            }

            db.pieces.Add( piece );
            db.SaveChanges();

            return CreatedAtRoute( "DefaultApi", new {
                id = piece.pieceId
            }, piece );
        }

        // POST: api/Pieces/deletePiece/5
        [HttpPost]
        public IHttpActionResult deletePiece( int id )
        {
            Piece piece = db.pieces.Find( id );
            if( piece == null ) {
                return NotFound();
            }

            db.pieces.Remove( piece );
            db.SaveChanges();

            return Ok();
        }

        // PUT: api/Pieces/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult updatePiece(int id, [FromBody] Piece piece )
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != piece.pieceId) {
                return BadRequest();
            }

            db.Entry(piece).State = EntityState.Modified;

            try {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) {
                if( !PieceExists(id) ) {
                    return NotFound();
                } else
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

        private bool PieceExists(int id)
        {
            return db.pieces.Count(e => e.pieceId == id) > 0;
        }
    }
}