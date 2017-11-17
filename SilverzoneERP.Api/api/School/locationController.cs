using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class locationController : ApiController
    {
        ICountryRepository _countryRepository;
        IZoneRepository _zoneRepository;
        IStateRepository _stateRepository;
        IDistrictRepository _districtRepository;
        ICityRepository _cityRepository;
        IUserRepository _userRepository;
        public locationController(ICountryRepository _countryRepository, IZoneRepository _zoneRepository, IStateRepository _stateRepository, IDistrictRepository _districtRepository, ICityRepository _cityRepository, IUserRepository _userRepository)
        {
            this._countryRepository = _countryRepository;
            this._zoneRepository = _zoneRepository;
            this._stateRepository = _stateRepository;
            this._districtRepository = _districtRepository;
            this._cityRepository = _cityRepository;
            this._userRepository = _userRepository;
        }

        #region GET
        [HttpGet]
        public IHttpActionResult GetCountry()
        {
            try
            {
                var data = from c in _countryRepository.GetAll()
                           select new {
                               c.CountryName,
                               CountryId = c.Id,
                               c.RowVersion,
                               Guid = Guid.NewGuid(),
                               Zones = c.Zones.Select(z => new {
                                   ZoneId = z.Id,
                                   z.ZoneName,
                                   z.RowVersion,
                                   Guid = Guid.NewGuid(),
                                   States = z.States.Select(s => new {
                                       StateId = s.Id,
                                       s.StateName,
                                       s.StateCode,
                                       s.RowVersion,
                                       Guid = Guid.NewGuid(),
                                       Districts = s.Districts.Select(d => new {
                                           DistrictId = d.Id,
                                           d.DistrictName,
                                           d.RowVersion,
                                           Guid = Guid.NewGuid(),
                                           Cities = d.Cities.Select(ci => new {
                                               CityId = ci.Id,
                                               ci.CityName,
                                               ci.RowVersion,
                                               Guid = Guid.NewGuid()
                                           })
                                       })
                                   })
                               })
                           };

                //var CountryList =from z in _zoneRepository.FindBy(z=>z.Status==true)
                //                 join c in 
                return Ok(new { result = data });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Create/Update  
             
        [HttpPost]
        public IHttpActionResult Country(Country model)
        {
            if (ModelState.IsValid)
            {
                
                using (var transaction = _countryRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.Id == 0)
                        {                            
                            if (_countryRepository.Exists(model.CountryName))                         
                                return Ok(new { result = "Success", message = "Country already exists!" });
                           
                            _countryRepository.Create(new Country
                            {
                                CountryName = model.CountryName,
                                Status = true
                            });
                            msg = "Successfully country created!";                            
                        }
                        else
                        {
                            var _country = _countryRepository.Get(model.Id);

                            if (_country != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_country.RowVersion, 0))
                            {                                
                                if (_countryRepository.Exists(model.Id,model.CountryName))                             
                                    return Ok(new { result = "Success", message = "Country already exists!" });
                                                             
                                if (_country != null)
                                {
                                    _country.CountryName = model.CountryName;
                                    _countryRepository.Update(_country);
                                }
                                msg = "Successfully country updated!";                                
                            }
                            else
                                return Ok(new { result = "Success", message = "The record you attemped to edit was modified by another user. After you got original value then modified !", data = _country });
                        }
                        transaction.Commit();
                        return Ok(new { result = "Success", message = msg });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "Error", message = ex.Message });
                    }
                }
            }
            return Ok(new { result = "Error", message = "Error" });           
        }

        [HttpPost]
        public IHttpActionResult Zone(Zone model)
        {
            if (ModelState.IsValid)
            {

                using (var transaction = _countryRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.Id == 0)
                        {                            
                            if (_zoneRepository.Exists(model.CountryId,model.ZoneName))                         
                                return Ok(new { result = "Success", message = "Zone already exists!" });
                            
                            _zoneRepository.Create(new Zone
                            {
                                ZoneName = model.ZoneName,
                                CountryId = model.CountryId,
                                Status = true
                            });
                            msg = "Successfully zone created!";                            
                        }
                        else
                        {
                            var _zone = _zoneRepository.Get(model.Id);
                            if (_zone != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_zone.RowVersion, 0))
                            {
                                if (_zoneRepository.Exists(model.CountryId, model.Id, model.ZoneName))
                                    return Ok(new { result = "Success", message = "Zone already exists!" });


                                if (_zone != null)
                                {
                                    _zone.ZoneName = model.ZoneName;
                                    _zone.CountryId = model.CountryId;
                                    _zoneRepository.Update(_zone);
                                }
                                msg = "Successfully zone updated!";                                
                            }  
                            else
                                return Ok(new { result = "Success", message = "The record you attemped to edit was modified by another user. After you got original value then modified !", data = _zone });
                        }
                        transaction.Commit();
                        return Ok(new { result = "Success", message = msg });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "Error", message = ex.Message });
                    }
                }
            }
            return Ok(new { result = "Error", message = "Error" });           
        }

        [HttpPost]
        public IHttpActionResult State(State model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _stateRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";

                        if (model.Id == 0)
                        {
                            if (_stateRepository.Exists(model.StateName, model.StateCode, model.CountryId))
                            {
                                return Ok(new { result = "error", message = "State already exists!" });
                            }
                            State _state = new State();
                            _state.Status = true;
                            SetStateValue(model, ref _state);

                            _stateRepository.Create(_state);
                            msg = "Successfully state created!";
                        }
                        else
                        {
                            var _state = _stateRepository.GetById(model.Id);

                            if (_state != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_state.RowVersion, 0))
                            {
                                if (_stateRepository.Exists(model.Id, model.StateName, model.StateCode, model.CountryId))
                                    return Ok(new { result = "error", message = "State already exists!" });

                                if (_stateRepository.Exists(model.Id, model.StateCode, model.CountryId))
                                    return Ok(new { result = "error", message = "State code already exists!" });

                                if (_state != null)
                                {
                                    SetStateValue(model, ref _state);                                    
                                    _stateRepository.Update(_state);
                                }
                                msg = "Successfully state updated!";
                            }
                            else
                                return Ok(new { result = "Success", message = "The record you attemped to edit was modified by another user. After you got original value then modified !", data = _state });
                        }

                        transaction.Commit();
                        return Ok(new { result = "Success", message = msg });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "Error", message = ex.Message });
                    }
                }
            }
            return Ok(new { result = "error", message = "error" });
        }

        [HttpPost]
        public IHttpActionResult District(District model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";
                using (var transaction = _stateRepository.BeginTransaction())
                {
                    try
                    {
                        if (model.Id == 0)
                        {
                            if (_districtRepository.Exists(model.DistrictName, model.CountryId, model.ZoneId, model.StateId))
                            {
                                return Ok(new { result = "Success", message = "District already exists!" });
                            }
                            District _district = new District();
                            _district.Status = true;
                            SetDistrictValue(model, ref _district);
                            _districtRepository.Create(_district);
                            msg = "Successfully district created!";
                        }
                        else
                        {
                            var _district = _districtRepository.Get(model.Id);

                            if (_district != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_district.RowVersion, 0))
                            {
                                _district = _districtRepository.FindBy(x => x.Id != model.Id && x.DistrictName.Trim().ToLower() == model.DistrictName.Trim().ToLower() && x.CountryId == model.CountryId && x.ZoneId == model.ZoneId && x.StateId == model.StateId).FirstOrDefault();
                                if (_districtRepository.Exists(model.Id, model.DistrictName, model.CountryId, model.ZoneId, model.StateId))
                                    return Ok(new { result = "Success", message = "District already exists!" });


                                if (_district != null)
                                {
                                    SetDistrictValue(model, ref _district);
                                    _districtRepository.Update(_district);
                                }

                                msg = "Successfully district updated!";
                            }
                            else                            
                                return Ok(new { result = "Success", message = "The record you attemped to edit was modified by another user. After you got original value then modified !", data = _district });                            
                        }
                        transaction.Commit();
                        return Ok(new { result = "Success", message = msg });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "Error", message = ex.Message });
                    }
                }
            }
            return Ok(new { result = "Error", message = "Error" });
        }

        [HttpPost]
        public IHttpActionResult City(City model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _cityRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (model.Id == 0)
                        {
                            if (_cityRepository.Exists(model.CityName, model.CountryId, model.ZoneId, model.StateId, model.DistrictId))
                                return Ok(new { result = "Success", message = "City already exists!" });

                            City _city = new City();
                            _city.Status = true;
                            SetCityValue(model, ref _city);
                            _cityRepository.Create(_city);

                            msg = "Successfully city created!";
                        }
                        else
                        {
                            var _city = _cityRepository.Get(model.Id);

                            if (_city != null && BitConverter.ToInt64(model.RowVersion, 0) == BitConverter.ToInt64(_city.RowVersion, 0))
                            {
                                if (_cityRepository.Exists(model.Id, model.CityName, model.CountryId, model.ZoneId, model.StateId, model.DistrictId))
                                    return Ok(new { result = "Success", message = "City already exists!" });


                                if (_city != null)
                                {
                                    SetCityValue(model, ref _city);
                                    _cityRepository.Update(_city);
                                }
                                msg = "Successfully city updated!";
                            }
                            else                            
                                return Ok(new { result = "Success", message = "The record you attemped to edit was modified by another user. After you got original value then modified !", data = _city });                            
                        }
                        transaction.Commit();
                        return Ok(new { result = "Success", message = msg });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "Error", message = ex.Message });
                    }
                }
            }
            return Ok(new { result = "Error", message = "Error" });
        }


        #endregion

        #region Set Value
        private void SetStateValue(State model,ref State _state)
        {
            _state.StateName = model.StateName;
            _state.StateCode = model.StateCode.ToUpper();
            _state.ZoneId = model.ZoneId;
            _state.CountryId = model.CountryId;
        }
        private void SetDistrictValue(District model,ref District _district)
        {
            _district.DistrictName = model.DistrictName;
            _district.CountryId = model.CountryId;
            _district.ZoneId = model.ZoneId;
            _district.ZoneId = model.ZoneId;
            _district.StateId = model.StateId;            
        }
        private void SetCityValue(City model ,ref City _city)
        {
            _city.CityName = model.CityName;
            _city.CountryId = model.CountryId;
            _city.ZoneId = model.ZoneId;
            _city.ZoneId = model.ZoneId;
            _city.StateId = model.StateId;
            _city.DistrictId = model.DistrictId;
        }
        #endregion
    }
}
