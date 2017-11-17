using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Constant;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.Site;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Http;


namespace SilverzoneERP.Api.api.Site
{
    public class AccountController : ApiController
    {
        private IAccountRepository accountRepository;
        private IUserLogsRepository userLogsRepository;
        public IGenericOTPRepository genericOTPRepository { get; private set; }
        public IForgetPasswordRepository forgetPasswordRepository { get; private set; }
        private IRoleRepository roleRepository;

        [HttpGet]
        public IHttpActionResult send_sms_onPhone(string mobileNo)
        {
            string _result = string.Empty;

            // OTP is random six digit Number only.. not string !
            int _code = accountRepository.get_smsCode();

            // if user is not registered
            if (accountRepository.GetAll().Where(x => x.MobileNumber == mobileNo).Count() == 0)
            {
                int record_count = genericOTPRepository.GetAll().Where(x => x.mobileNo == mobileNo).Count();

                // check exsited Send OTP Numbers
                if (record_count == 0)       // add
                {
                    genericOTPRepository.Create(new GenericOTP
                    {
                        mobileNo = mobileNo,
                        max_attempt = 1,
                        sms_code = _code,
                        //valid_time = get_currentDate().AddMinutes(10),
                        valid_time = get_currentDate().AddMinutes(10),
                        max_attempt_unlock_date = get_currentDate()
                    });

                    // send code on mobile
                    accountRepository.sms_verification(mobileNo, _code, verficationType.register);

                    _result = "ok";
                }
                else        // update existed record
                {
                    // find record by mobile number 
                    var _model = genericOTPRepository.GetByMobile(mobileNo);

                    if (_model.max_attempt < 5)            // maximum attempt upto 5 times
                    {
                        //DateTime date1 = Convert.ToDateTime(get_currentDate().ToString("MM/dd/yyyy"));          // convert datetime to Date

                        // https://msdn.microsoft.com/en-us/library/system.datetime.compare(v=vs.110).aspx
                        int result = get_currentDate().Date.CompareTo(_model.max_attempt_unlock_date.Date);

                        if (result == -1)
                            _result = "Block";
                        else
                        {
                            // update data whatever we want
                            _model.max_attempt = result > 0 ? 1 : ++_model.max_attempt;

                            _model.sms_code = _code;
                            _model.valid_time = get_currentDate().AddMinutes(10);
                            _model.max_attempt_unlock_date = get_currentDate();

                            // send code on mobile
                            accountRepository.sms_verification(mobileNo, _code, verficationType.register);

                            genericOTPRepository.Update(_model);

                            _result = "ok";
                        }

                    }
                    else                  // block user
                    {
                        _model.max_attempt_unlock_date = get_currentDate().AddDays(1);
                        _model.max_attempt = 0;         // reset max_attempt

                        genericOTPRepository.Update(_model);

                        _result = "Block";
                    }

                }
            }
            else
            {
                _result = "exist";
            }

            // if Ok is use to only go to success of angular ajax
            // otherwise go to interceptor types of error > 404/501/400
            return Ok(new { result = _result });
        }

        [HttpPost]
        public IHttpActionResult Register_user(RegisterViewModel model)
        {
            string _result = string.Empty;

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            var ipAddress = host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            if (ModelState.IsValid && roleRepository.role_isActive(model.RoleId))
            {
                accountRepository.Create(new Entities.Models.User
                {
                    MobileNumber = model.mobileNumber,
                    Password = accountRepository.GetPasswordHash(model.Password),
                    Browser = HttpContext.Current.Request.Browser.Browser,
                    IPAddress = ipAddress.ToString(),
                    OperatingSystem = Environment.OSVersion.ToString(),
                    Location = RegionInfo.CurrentRegion.DisplayName,
                    CreationDate = accountRepository.get_DateTime(),
                    UpdationDate = accountRepository.get_DateTime(),
                    RoleId = model.RoleId,
                    Status = true
                });
                    _result = "ok";   // autiomatically save data for above
                }
                else
                {
                    _result = "invalid_Role";   // autiomatically save data for above
                }

            return Ok(new { result = _result });
        }

        [HttpPost]
        public IHttpActionResult Login(LoginViewModel model)
        {
            var _user = new Entities.Models.User();

            if (ModelState.IsValid)
            {
                _user = accountRepository.check_User(model.userName, model.verificationMode);

                if (_user != null)
                {
                    if (accountRepository.VerifyPassword(model.Password, _user.Password))
                    {
                        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                        var ipAddress = host
                            .AddressList
                            .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

                        var deviceInfo = string.Empty;
                        var browser = HttpContext.Current.Request.Browser;
                        if (browser.IsMobileDevice || browser.Browser == "Unknown")
                            deviceInfo = "Mobile Device";
                        else
                            deviceInfo = browser.Browser;

                        userLogsRepository.Create(new UserLogs()
                        {
                            Login_DateTime = get_currentDate(),
                            UserId = _user.Id,
                            Browser = deviceInfo,
                            IPAddress = ipAddress.ToString(),
                            Location = RegionInfo.CurrentRegion.DisplayName,
                        });

                        // if condison is success , get user Token &  then return from this controller
                        return Ok(new { result = "ok", user = UserProfileViewModel.Parse(_user), token = getToken(model) });
                    }
                    else
                    {
                        return Ok(new { result = "invalid" });        // no need to pass data from here 
                    }
                }
                else
                {
                    return Ok(new { result = "notfound" });
                }
            }
            return Ok(new { result = "error" });
        }

        private AccessTokenViewModel getToken(LoginViewModel model)
        {
            var url = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority;
            WebRequest myWebRequest = WebRequest.Create(url + "/token");
            myWebRequest.ContentType = "application/x-www-form-urlencoded";
            myWebRequest.Method = "POST";
            var request = string.Format("grant_type=password&userName={0}&Password={1}&verificationMode={2}",
                                                                         HttpUtility.UrlEncode(model.userName),
                                                                         HttpUtility.UrlEncode(model.Password),
                                                                         HttpUtility.UrlEncode(model.verificationMode.ToString())
                                                                         );

            byte[] bytes = Encoding.ASCII.GetBytes(request);
            myWebRequest.ContentLength = bytes.Length;
            using (Stream outputStream = myWebRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }

            using (WebResponse webResponse = myWebRequest.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AccessTokenViewModel));

                //Get deserialized object from JSON stream
                AccessTokenViewModel token = (AccessTokenViewModel)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }

        [HttpPost]
        public IHttpActionResult verify_OTP(OTPViewModel model)
        {
            var _result = string.Empty;

            if (ModelState.IsValid)
            {
                var entity = genericOTPRepository.GetByMobile(model.mobileNumber);

                if (entity != null)
                {
                    if (entity.sms_code == model.sms_OTP)
                    {
                        if (entity.valid_time >= get_currentDate())     // both dateTime has same structure
                            _result = "ok";
                        else
                            _result = "expire";
                    }
                    else
                    {
                        _result = "invalid";
                    }
                }
            }
            return Ok(new { result = _result });
        }

        [HttpPost]  // if email id is detected then forget email template will be returned
        public IHttpActionResult forget_password(loginModel model)
        {
            if (ModelState.IsValid)
            {
                var _user = accountRepository.check_User(model.userName, model.verificationMode);

                if (_user != null)
                {
                    // calling method to save data & save its output
                    var _status = sendSms_forgetPassword(new passwordRecoveryViewModel()
                    {
                        verificationMode = model.verificationMode,    // request for mobile/email verification
                        userId = _user.Id,
                        userName = model.userName,      // contain emailId or Mobile no.
                        OTP_code = accountRepository.get_smsCode(),
                        verficationType = verficationType.forget
                    });

                    return Ok(new { result = _status });
                }
                return Ok(new { result = "notfound" });
            }
            return Ok(new { result = "error" });
        }

        // shared common fx
        public string sendSms_forgetPassword(passwordRecoveryViewModel model)
        {
            // if no record is found > Any will return true if record is exist but we will add record whhen it false
            if (!forgetPasswordRepository.GetAll().Any(x => x.UserId == model.userId && x.verificationMode == model.verificationMode))
            {
                forgetPasswordRepository.Create(new ForgetPassword()
                {
                    UserId = model.userId,
                    verificationMode = model.verificationMode,
                    max_attempt = 1,
                    sms_code = model.OTP_code,
                    valid_time = get_currentDate().AddMinutes(10),
                    max_attempt_unlock_date = get_currentDate()
                });

                // send sms verification code 
                sendCode(model);            // create a single mehod to send instead of again and again writing code

                return "ok";        // exit from here > below code won't wrk
            }

            // updating record
            var _model = forgetPasswordRepository.getRecords(model.userId, model.verificationMode);

            if (_model.max_attempt < 5)            // maximum attempt upto 5 times
            {
                int result = get_currentDate().Date.CompareTo(_model.max_attempt_unlock_date.Date);

                if (result == -1)
                    return "Block";        // exit from here > below code won't wrk

                // update data whatever we want
                _model.max_attempt = result > 0 ? 1 : ++_model.max_attempt;
                _model.sms_code = model.OTP_code;
                _model.valid_time = get_currentDate().AddMinutes(10);
                _model.max_attempt_unlock_date = get_currentDate();

                // send sms verification code 
                sendCode(model);

                // update records
                forgetPasswordRepository.Update(_model);

                return "ok";        // exit from here > below code won't wrk
            }

            _model.max_attempt_unlock_date = get_currentDate().AddDays(1);
            _model.max_attempt = 0;         // reset max_attempt

            // update records
            forgetPasswordRepository.Update(_model);

            return "Block";        // exit from here
        }

        private void sendCode(passwordRecoveryViewModel model)
        {
            // sending verification code to mobile/email
            if (model.verificationMode.Equals(verificationMode.email))
            {
                System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

                forgetPasswordRepository.sendEmail_forgetPassword(
                    //HostingEnvironment.MapPath("~/templates/EmailTemplates/forget_password.html"),
                    image_urlResolver.project_root + "templates/EmailTemplates/forget_password.html",
                    model.OTP_code,
                    model.userName
                    );
            }
            else if (model.verificationMode.Equals(verificationMode.mobile))
            {
                // send code on mobile
                accountRepository.sms_verification(model.userName, model.OTP_code, model.verficationType);
            }
        }

        [HttpPost]
        public IHttpActionResult verify_forgetLogin_OTP(passwordRecoveryViewModel model)
        {
            var _result = string.Empty;

            if (ModelState.IsValid)
            {
                model.userId = accountRepository.check_User(model.userName, model.verificationMode).Id;

                return Ok(new { result = verify_newPasswordOTP(model) });
            }
            return Ok(new { result = "error" });
        }

        public string verify_newPasswordOTP(passwordRecoveryViewModel model)
        {  // shared fx 
            var entity = forgetPasswordRepository.getRecords(model.userId, model.verificationMode);

            if (entity != null)
            {
                //var OTP_valid_date = Convert.ToDateTime(string.Format("{0: dd/MM/yyyy HH:mm:ss}", entity.valid_time));
                //var current_date = Convert.ToDateTime(string.Format("{0: dd/MM/yyyy HH:mm:ss}", get_currentDate()));

                if (entity.sms_code == model.OTP_code)
                {
                    if (entity.valid_time >= get_currentDate())
                        return "ok";
                    else
                        return "expire";
                }
                return "invalid";
            }
            return "notfound";
        }

        [HttpPost]
        public IHttpActionResult saved_newforget_password(LoginViewModel model)
        {
            var _user = accountRepository.check_User(model.userName, model.verificationMode);

            if (_user != null)
            {
                _user.Password = accountRepository.GetPasswordHash(model.Password);

                accountRepository.Update(_user);
                return Ok(new { result = "success" });
            }
            return Ok(new { result = "notfound" });
        }

        private DateTime get_currentDate()
        {
            return accountRepository.get_DateTime();
        }


        // *****************  Constructors  ********************************

        public AccountController(                           // dependency injection resolve from DI
         IAccountRepository _accountRepository,
         IGenericOTPRepository _genericOTPRepository,
         IForgetPasswordRepository _forgetPasswordRepository,
         IUserLogsRepository _userLogsRepository,
         IRoleRepository _roleRepository)
        {
            accountRepository = _accountRepository;
            genericOTPRepository = _genericOTPRepository;
            forgetPasswordRepository = _forgetPasswordRepository;
            roleRepository = _roleRepository;
            userLogsRepository = _userLogsRepository;
        }

        // 2nd constructor to use with other controller ('UserProfileController') with required dependency injection params
        public AccountController(IAccountRepository _accountRepository, IForgetPasswordRepository _forgetPasswordRepository)
        {
            accountRepository = _accountRepository;
            forgetPasswordRepository = _forgetPasswordRepository;
        }


    }
}
