using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Text;
using WebApiCore.Class;
using WebApiCore.Class.auth;
using WebApiCore.Context;
using WebApiCore.Security;

namespace WebApiCore.Controllers.auth
{
    [AllowAnonymous]
    public class authController : ControllerBase
    {
        private readonly bdERP db;
        private readonly IConfiguration _configuration;
        public authController(bdERP context, IConfiguration configuration)
        {
            db = context;
            _configuration = configuration;
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        [HttpPost]
        [Route("validateEmailUsuario")]
        public async Task<ActionResult> ValidateEmailUsuario([FromBody] LoginRequest login)
        {
            if (login == null)
                return BadRequest();

            bool isRedirect = false;
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["isRedirect"]))
            {
                isRedirect = bool.Parse(HttpContext.Request.Query["isRedirect"]);
            }

            var objUser = db.tblUsuario.Where(x => (x.email.Equals(login.Email) || x.usuario.Equals(login.Email)) && !x.isEliminado).FirstOrDefault();
            if (objUser != null)
            {
                return Ok("PolarierUser");
            }
            else if (!isRedirect)
            {
                try
                {
                    HttpClient client = new HttpClient();
                    var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("https://sas-mypolarier.polarier.com:448/ValidateEmailUsuario?isRedirect=true", content);

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return Ok("SasUser");
                    }
                }
                catch
                {

                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("validateEmail")] //RRHH
        public ActionResult ValidateEmail([FromBody] LoginRequest login)
        {
            if (login == null)
                return BadRequest();

            var objUser = db.tblUsuario.Where(x => (x.email.Equals(login.Email) || x.usuario.Equals(login.Email) || x.idPersonaNavigation.email == login.Email)
            && !x.isEliminado).Select(x => new
            {
                x.password,
                personaDesactivada = !x.idPersonaNavigation.eliminado || x.idPersonaNavigation == null
            }).FirstOrDefault();

            if (objUser != null)
            {
                if (objUser.personaDesactivada)
                {
                    return Ok(objUser.password == "");
                }
                else
                {
                    return BadRequest("personaDesactivada");
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("validatePassword")] //MyPolarier
        public ActionResult ValidatePassword([FromBody] LoginRequest login)
        {
            var objUser = db.tblUsuario.Where(x => (x.email.Equals(login.Email) || x.usuario.Equals(login.Email)) && !x.isEliminado).FirstOrDefault();
            if (objUser != null)
            {
                string pass = Utils.getMD5(login.Password);
                if (db.tblUsuario.Where(x => (x.usuario == objUser.usuario && x.password == pass) && !x.isEliminado).Count() > 0)
                {
                    return Ok(1);
                }
            }
            return Ok(-1);
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] LoginRequest login)
        {
            if (login == null)
                return BadRequest();

            string pass = Utils.getMD5(login.Password);
            var objUser = db.tblUsuario.Where(x => (x.email.Equals(login.Email) || x.usuario.Equals(login.Email)) && x.password == pass && !x.isEliminado).FirstOrDefault();

            if (objUser != null && objUser.password.Length > 0)
            {
                return Ok(getToken(objUser.idUsuario, true));
            }
            // Unauthorized access 
            return Unauthorized();
        }

        [HttpPost]
        [Route("login_mobile")]
        public ActionResult Login_Mobile([FromBody] LoginRequest login)
        {
            if (login == null)
                return BadRequest();

            string pass = Utils.getMD5(login.Password);
            tblUsuario objUser = db.tblUsuario
                .Where(x => (x.email == login.Email || x.usuario == login.Email || x.idPersonaNavigation.email == login.Email) &&
                             x.password == pass &&
                             ((!x.idPersonaNavigation.eliminado) || x.idPersonaNavigation == null) &&
                            !x.isEliminado)
                .FirstOrDefault();


            if (objUser != null && objUser.password.Length > 0)
            {
                if (login.notificationToken != null)
                {
                    objUser.notificationToken = login.notificationToken;
                    db.SaveChanges();
                }

                return Ok(getToken_Mobile(objUser.idUsuario, true));
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("refreshToken")]
        public ActionResult RefreshToken([FromBody] LoginResponse login)
        {
            if (login == null)
                return BadRequest();

            Guid guid;
            bool isValid = Guid.TryParse(login.refreshToken, out guid);

            if (!isValid)
                return BadRequest();

            tblToken_Refresh objRefreshToken = db.tblToken_Refresh.Where(x => x.idToken == guid).FirstOrDefault();

            if (objRefreshToken != null && objRefreshToken.fechaExpiracion >= DateTime.Now)
            {
                return Ok(getToken(objRefreshToken.idUsuario, false));
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("refreshToken_Mobile")]
        public ActionResult RefreshToken_Mobile([FromBody] LoginResponse login)
        {
            if (login == null)
                return BadRequest();

            Guid guid;
            bool isValid = Guid.TryParse(login.refreshToken, out guid);

            if (!isValid)
                return BadRequest();

            tblToken_Refresh_Mobile objRefreshToken = db.tblToken_Refresh_Mobile.Where(x => x.idToken == guid).FirstOrDefault();
            db.SaveChanges();

            if (objRefreshToken != null && objRefreshToken.fechaExpiracion >= DateTime.Now)
            {
                return Ok(getToken_Mobile(objRefreshToken.idUsuario, false));
            }
            return Unauthorized();
        }

        private LoginResponse getToken(int idUsuario, bool getRefreshToken)
        {
            int expireTime = int.Parse(_configuration.GetSection("JWT:EXPIRE_MINUTES").Value);
            int refreshExpireTime = int.Parse(_configuration.GetSection("JWT:REFRESH_EXPIRE_MINUTES").Value);
            string token = new TokenGenerator(_configuration).GenerateTokenJwt(idUsuario);

            tblUsuario objUser = db.tblUsuario.Where(x => x.idUsuario == idUsuario).FirstOrDefault();

            LoginResponse result = new LoginResponse()
            {
                token = token,
                token_expireDate = DateTime.Now.AddMinutes(expireTime)
            };

            if (getRefreshToken)
            {
                tblToken_Refresh objNewToken = new tblToken_Refresh()
                {
                    idToken = Guid.NewGuid(),
                    idUsuario = idUsuario,
                    fechaExpiracion = DateTime.Now.AddMinutes(refreshExpireTime),
                };

                if (objUser != null && objUser.enableFullScreen == true)
                {
                    objNewToken.fechaExpiracion = DateTime.Now.AddYears(3);
                }

                db.tblToken_Refresh.Add(objNewToken);

                db.SaveChanges();
                InvalidateToken(idUsuario);

                result.refreshToken = objNewToken.idToken.ToString();
                result.refreshToken_expireDate = objNewToken.fechaExpiracion;
            }

            return result;
        }

        private LoginResponse getToken_Mobile(int idUsuario, bool getRefreshToken)
        {
            int expireTime = int.Parse(_configuration.GetSection("JWT_MOBILE:EXPIRE_MINUTES").Value);
            int refreshExpireTime = int.Parse(_configuration.GetSection("JWT_MOBILE:REFRESH_EXPIRE_MINUTES").Value);
            string token = new TokenGenerator(_configuration).GenerateTokenJwt_mobile(idUsuario);

            tblUsuario objUser = db.tblUsuario.Where(x => x.idUsuario == idUsuario).FirstOrDefault();

            LoginResponse result = new LoginResponse()
            {
                idUsuario = objUser.idUsuario,
                nombreUsuario = objUser.nombre,
                token = token,
                token_expireDate = DateTime.Now.AddMinutes(expireTime)
            };

            if (getRefreshToken)
            {
                tblToken_Refresh_Mobile objNewToken = new tblToken_Refresh_Mobile()
                {
                    idToken = Guid.NewGuid(),
                    idUsuario = idUsuario,
                    fechaExpiracion = DateTime.Now.AddMinutes(refreshExpireTime),
                };

                db.tblToken_Refresh_Mobile.Add(objNewToken);

                db.SaveChanges();
                InvalidateToken(idUsuario);

                result.refreshToken = objNewToken.idToken.ToString();
                result.refreshToken_expireDate = objNewToken.fechaExpiracion;
            }

            return result;
        }

        public void InvalidateToken(int idUsuario)
        {
            db.tblToken_Refresh.RemoveRange(
                db.tblToken_Refresh.Where(x => x.idUsuario == idUsuario).OrderByDescending(x => x.fechaExpiracion).Skip(1)
            );

            db.tblToken_Refresh_Mobile.RemoveRange(
               db.tblToken_Refresh_Mobile.Where(x => x.idUsuario == idUsuario).OrderByDescending(x => x.fechaExpiracion).Skip(1)
           );

            db.SaveChanges();
        }

        [HttpPost]
        [Route("recoveryPassword")]
        public ActionResult RecoveryPassword([FromBody] RecoveryPassword recoveryPassword)
        {
            if (recoveryPassword == null)
                return BadRequest();

            tblUsuario user = db.tblUsuario.Where(x => x.email == recoveryPassword.Email && !x.isEliminado).FirstOrDefault();

            if (user != null)
            {
                db.tblRecoveryPassword.RemoveRange(db.tblRecoveryPassword.Where(x => x.idUsuario == user.idUsuario));

                tblRecoveryPassword objRecovery = new tblRecoveryPassword()
                {
                    idUsuario = user.idUsuario,
                    fecha = DateTime.Now,
                    guid = Guid.NewGuid()
                };

                db.tblRecoveryPassword.Add(objRecovery);

                db.SaveChanges();

                //Enviar correo
                Notificar_recoveryPassword(user, objRecovery);
                return Ok();
            }
            return Unauthorized();
        }

        [EnableQuery]
        [HttpGet]
        private void Notificar_recoveryPassword(tblUsuario user, tblRecoveryPassword recoveryPassword)
        {
            string url;
            if (Utils.isProduccion())
            {
                url = "http://mypolarier.polarier.com/RecoveryPassword/" + recoveryPassword.guid;
            }
            else
            {
                url = "http://testmypolarier.polarier.com/RecoveryPassword/" + recoveryPassword.guid;
            }

            //url = "http://localhost:3000/RecoveryPassword/" + recoveryPassword.guid + "/recoveryPassword";

            // Generamos el paquete SMTP con la configuracion del servidor y las credenciales de acceso
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "outlook.office365.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("mypolarier@polarier.com", "Vog45080");

            // Generamos el mail a enviar
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("mypolarier@polarier.com", "No Reply MyPolarier");

            mail.To.Add(user.email);

            mail.Subject = "My Polarier: Recuperación de su contraseña";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            mail.Body = string.Empty;

            using (StreamReader reader = new StreamReader(@"./Pages/RecoveryPassword.html"))
            {
                mail.Body = reader.ReadToEnd();
            }


            string currentDirectory = Directory.GetCurrentDirectory();
            string path = "Media/Imagenes";
            string fullPath = Path.Combine(currentDirectory, path, "logoLoveYourLinen.png");

            Attachment objAttach = new Attachment(fullPath);
            mail.Attachments.Add(objAttach);


            mail.Body = mail.Body.Replace("@@url", url);
            mail.Body = mail.Body.Replace("@@img", objAttach.ContentId);

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            //Enviamos el mail
            try
            {
                smtpClient.Send(mail);
            }
            catch
            {
            }
        }

        [HttpPost]
        [Route("checkRecoveryPassword_Token")]
        public ActionResult checkRecoveryPassword_Token([FromBody] RecoveryPassword recoveryPassword)
        {
            if (recoveryPassword == null)
                return BadRequest();

            tblRecoveryPassword objRecovery = db.tblRecoveryPassword.Where(x => x.guid == recoveryPassword.Token).FirstOrDefault();

            if (objRecovery != null)
            {
                tblUsuario user = db.tblUsuario.Where(x => x.idUsuario == objRecovery.idUsuario && !x.isEliminado).FirstOrDefault();
                bool guidCorrecto = (DateTime.Now - objRecovery.fecha).TotalMinutes < (user.password == "" ? (60 * 24 * 3) : 60);
                if (guidCorrecto)
                {
                    return Ok();
                }
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("changePassword")]
        public ActionResult changePassword([FromBody] RecoveryPassword recoveryPassword)
        {
            if (recoveryPassword == null)
                return BadRequest();

            tblRecoveryPassword objRecovery = db.tblRecoveryPassword.Where(x => x.guid == recoveryPassword.Token).FirstOrDefault();

            if (objRecovery != null)
            {
                tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == objRecovery.idUsuario && !x.isEliminado).FirstOrDefault();
                if (objUsuario != null)
                {
                    //Cambiamos contraseña
                    objUsuario.password = Utils.getMD5(recoveryPassword.NewPassword);
                    //Limpiamos la tabla tblRecoveryPassword
                    db.tblRecoveryPassword.RemoveRange(db.tblRecoveryPassword.Where(x => x.idUsuario == objUsuario.idUsuario));

                    db.SaveChanges();

                    recoveryPassword.Email = objUsuario.email;
                    return Ok(recoveryPassword);
                }
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("emailAltaUsuario")]
        public ActionResult emailAltaUsuario([FromBody] tblUsuario user)
        {
            if (user == null)
                return BadRequest();

            if (user != null)
            {
                db.tblRecoveryPassword.RemoveRange(db.tblRecoveryPassword.Where(x => x.idUsuario == user.idUsuario));

                tblRecoveryPassword objRecovery = new tblRecoveryPassword()
                {
                    idUsuario = user.idUsuario,
                    fecha = DateTime.Now,
                    guid = Guid.NewGuid()
                };

                tblUsuario objUsuario = db.tblUsuario.Where(x => x.idUsuario == user.idUsuario && !x.isEliminado).FirstOrDefault();

                if (user.email == null)
                {
                    user.email = objUsuario.email;
                }

                db.tblRecoveryPassword.Add(objRecovery);
                db.SaveChanges();

                //Enviar correo
                Notificar_emailAltaUsuario(user, objRecovery);
                return Ok();
            }
            return Unauthorized();
        }

        [EnableQuery]
        [HttpGet]
        private void Notificar_emailAltaUsuario(tblUsuario user, tblRecoveryPassword setPassword)
        {
            string url;
            if (Utils.isProduccion())
            {
                url = "http://mypolarier.polarier.com/RecoveryPassword/" + setPassword.guid;
            }
            else
            {
                url = "http://testmypolarier.polarier.com/RecoveryPassword/" + setPassword.guid;

            }

            //url = "http://localhost:3000/RecoveryPassword/" + setPassword.guid;

            // Generamos el paquete SMTP con la configuracion del servidor y las credenciales de acceso
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "outlook.office365.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("mypolarier@polarier.com", "Vog45080");

            // Generamos el mail a enviar
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("mypolarier@polarier.com", "No Reply MyPolarier");

            mail.To.Add(user.email);

            mail.Subject = "Bienvenido a My Polarier";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            mail.Body = string.Empty;

            using (StreamReader reader = new StreamReader(@"./Pages/SetPassword.html"))
            {
                mail.Body = reader.ReadToEnd();
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = "Media/Imagenes";
            string fullPath = Path.Combine(currentDirectory, path, "logoLoveYourLinen.png");

            Attachment objAttach = new Attachment(fullPath);
            mail.Attachments.Add(objAttach);

            mail.Body = mail.Body.Replace("@@url", url);
            mail.Body = mail.Body.Replace("@@img", objAttach.ContentId);
            mail.Body = mail.Body.Replace("@@tiempoExpira", "3 días");

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            //Enviamos el mail
            try
            {
                smtpClient.Send(mail);
            }
            catch
            {
            }
        }

        [HttpPost]
        [Route("setPassword")] //RRHH
        public async Task<ActionResult> setPassword([FromBody] SetPassword setPassword)
        {
            if (setPassword == null)
                return BadRequest();

            tblRecoveryPassword objRecovery = db.tblRecoveryPassword.Where(x => x.guid == setPassword.PasswordToken).FirstOrDefault();
            if (objRecovery == null)
                return BadRequest();

            tblUsuario objUsuario = db.tblUsuario
             .Where(x => (x.email == setPassword.Email || x.usuario == setPassword.Email || x.idPersonaNavigation.email == setPassword.Email) && !x.isEliminado)
             .FirstOrDefault();
            if (objUsuario == null)
                return BadRequest();

            objUsuario.notificationToken = setPassword.notificationToken;
            db.SaveChanges();

            bool guidCorrecto = (DateTime.Now - objRecovery.fecha).TotalMinutes < (objUsuario.password == "" ? (60 * 24 * 3) : 60);
            if (!guidCorrecto)
                return BadRequest();


            objUsuario.password = Utils.getMD5(setPassword.NewPassword);

            db.tblRecoveryPassword.RemoveRange(db.tblRecoveryPassword.Where(x => x.idUsuario == objUsuario.idUsuario));

            await db.SaveChangesAsync();

            return Ok(getToken_Mobile(objUsuario.idUsuario, true));
        }

        #region LOGEAR COMO OTRO USUARIO MY POLARIER
        [HttpPost]
        [Route("loginChangeUser")]
        public ActionResult loginChangeUser([FromBody] LoginChangeUser objChangeUser)
        {
            List<int> idsUsuarios_accesoLoginChangeUser = new List<int>(new int[] { 178, 179, 180, 931, 1611, 2397 }); // idsUsuario con acceso a la funcionalidad
            if (idsUsuarios_accesoLoginChangeUser.Contains(objChangeUser.idUsuarioOriginal))
            {
                return Ok(getToken_changeUser(objChangeUser.idUsuarioSel));
            }
            return Unauthorized();
        }

        private LoginResponse getToken_changeUser(int idUsuarioSel)
        {
            int expireTime = int.Parse(_configuration.GetSection("JWT:EXPIRE_MINUTES").Value);
            LoginResponse result = new LoginResponse()
            {
                token = new TokenGenerator(_configuration).GenerateTokenJwt(idUsuarioSel),
                token_expireDate = DateTime.Now.AddMinutes(expireTime)
            };

            return result;
        }
        #endregion

        [HttpPost]
        [Route("GetEnableDatosSalarialesOficina")]
        public ActionResult GetEnableDatosSalarialesOficina([FromBody] LoginRequest login)
        {
            if (login == null)
                return BadRequest();

            string password = Utils.getMD5(login.Password);
            var user = db.tblUsuario.FirstOrDefault(x => (x.email == login.Email || x.usuario == login.Email) && x.password == password && !x.isEliminado);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user?.enableDatosSalarialesOficina ?? false);
        }
    }
}