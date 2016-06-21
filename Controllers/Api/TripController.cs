using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Controllers.Api
{
    using System;
    using System.Net;

    using AutoMapper;

    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Logging;

    using TheWorld.Models;
    using TheWorld.ViewModels;

    [Authorize]
    [Route("api/trips")]
    public class TripController : Controller
    {
        private readonly IWorldRepository _repository;

        private readonly ILogger<TripController> _logger;

        public TripController(IWorldRepository repository, ILogger<TripController> logger)
        {
            this._repository = repository;
            this._logger = logger;
        }

        [HttpGet("")]
        public JsonResult Get()
        {
            var trips = this._repository.GetUserTripsWithStops(User.Identity.Name);
            var results = Mapper.Map<IEnumerable<TripViewModel>>(trips);

            return Json(results);
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]TripViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTrip = Mapper.Map<Trip>(vm);
                    newTrip.UserName = User.Identity.Name;

                    // Save to the Database
                    this._logger.LogInformation("Attempting to save a new trip");
                    this._repository.AddTrip(newTrip);

                    if (this._repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<TripViewModel>(newTrip));
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("Failed to save new trip", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }
    }
}
