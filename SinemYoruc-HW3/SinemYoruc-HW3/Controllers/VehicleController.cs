using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SinemYoruc_HW3
{
    [ApiController]
    [Route("api/[controller]s")]
    public class VehicleController : ControllerBase
    {
        private readonly IMapperSession session;
        public VehicleController(IMapperSession session)
        {
            this.session = session;
        }


        [HttpGet]
        public List<Vehicle> Get()
        {
            var response = session.Vehicles.ToList();
            return response;
        }


        [HttpGet("{id}")]
        public ActionResult<List<Container>> GetById(int id)
        {
            try
            {
                Vehicle vehicle = session.Vehicles.Where(x => x.Id == id).FirstOrDefault();
                var container = session.Containers.Where(x => x.VehicleId == vehicle.Id).ToList();
                return container;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Vehicle is not found");
                return NotFound("Vehicle is not found");
            }

        }

        [HttpPost]
        public void Post([FromBody] Vehicle vehicle)
        {
            try
            {
                session.BeginTransaction();
                session.Save(vehicle);
                session.Commit();
            }
            catch (Exception ex)
            {
                session.Rollback();
                Log.Error(ex, "Vehicle Insert Error");
            }
            finally
            {
                session.CloseTransaction();
            }
        }

        [HttpPut]
        public ActionResult<Vehicle> Put([FromBody] Vehicle request)
        {
            Vehicle vehicle = session.Vehicles.Where(x => x.Id == request.Id).FirstOrDefault();
            if (vehicle == null)
            {
                return NotFound();
            }

            try
            {
                session.BeginTransaction();

                vehicle.VehicleName = request.VehicleName;
                vehicle.VehiclePlate = request.VehiclePlate;

                session.Update(vehicle);

                session.Commit();
            }
            catch (Exception ex)
            {
                session.Rollback();
                Log.Error(ex, "Vehicle Updated Error");
            }
            finally
            {
                session.CloseTransaction();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Vehicle> Delete(int id)
        {
            Vehicle vehicle = session.Vehicles.Where(x => x.Id == id).FirstOrDefault();

            if (vehicle == null)
            {
                return NotFound();
            }

            try
            {
                session.BeginTransaction();
                session.Delete(vehicle);
                session.Commit();
                Container container = session.Containers.Where(x => x.VehicleId == vehicle.Id).FirstOrDefault();
                if (container != null)
                {
                    session.Delete(container);
                    session.Commit();
                }
            }
            catch (Exception ex)
            {
                session.Rollback();
                Log.Error(ex, "Vehicle Deleted Error");
            }
            finally
            {
                session.CloseTransaction();
            }

            return Ok();
        }

    }
}
