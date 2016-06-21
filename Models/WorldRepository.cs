using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    using Microsoft.Data.Entity;
    using Microsoft.Extensions.Logging;

    public class WorldRepository : IWorldRepository
    {
        private readonly WorldContext _context;

        private readonly ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try
            {
                return this._context.Trips.OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips from database", ex);
                return null;
            }
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return this._context.Trips.Include(t => t.Stops).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }

        public void AddTrip(Trip newTrip)
        {
            this._context.Add(newTrip);
        }

        public bool SaveAll()
        {
            // If savechanges returns > 0 then records were saved
            return this._context.SaveChanges() > 0;
        }

        public Trip GetTripByName(string tripName, string username)
        {
            return this._context.Trips.Include(t => t.Stops)
                .Where(t => t.Name == tripName && t.UserName == username)
                .FirstOrDefault();
        }

        public void AddStop(string tripName, string username, Stop newStop)
        {

            var theTrip = GetTripByName(tripName, username);
            newStop.Order = theTrip.Stops.Max(s => s.Order) + 1;
            theTrip.Stops.Add(newStop);
            this._context.Stops.Add(newStop);
        }

        public IEnumerable<Trip> GetUserTripsWithStops(string name)
        {
            try
            {
                return this._context.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name)
                    .Where(t => t.UserName == name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }
    }
}
