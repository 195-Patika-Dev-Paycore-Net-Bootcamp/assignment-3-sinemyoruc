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
        public List<Vehicle> Get() //Tum araclari getir
        {
            var response = session.Vehicles.ToList();
            return response;
        }


        [HttpPost]
        public void Post([FromBody] Vehicle vehicle) //Arac ekleme
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
        public ActionResult<Vehicle> Put([FromBody] Vehicle request) //Arac guncelleme
        {
            Vehicle vehicle = session.Vehicles.Where(x => x.Id == request.Id).FirstOrDefault();
            if (vehicle == null)
            {
                return NotFound();
            }

            try
            {
                session.BeginTransaction();

                // id guncellenmedi
                vehicle.VehicleName = request.VehicleName != default ? request.VehicleName : vehicle.VehicleName;
                vehicle.VehiclePlate = request.VehiclePlate != default ? request.VehiclePlate : vehicle.VehiclePlate;

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
        public ActionResult<Vehicle> Delete(int id) //Arac silme
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
                Container container = session.Containers.Where(x => x.VehicleId == vehicle.Id).FirstOrDefault(); //ayni vehicleIdye sahip container varsa onu da siliyor
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
