using Biblioteca.Aplicacao;
using Biblioteca.Aplicacao.Dtos;
using Biblioteca.Infra.Configuracoes.Orm.Permissionamento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Biblioteca.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class LoginsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly ILogger<LoginsController> _logger;

        public LoginsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations, ILogger<LoginsController> logger, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
            _logger = logger;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public object Post(UsuarioDto usuario)
        {
            bool credenciaisValidas = false;
            if (usuario != null && !String.IsNullOrWhiteSpace(usuario.UserID))
            {
                // Verifica a existência do usuário nas tabelas do
                // ASP.NET Core Identity
                var userIdentity = _userManager
                    .FindByNameAsync(usuario.UserID);
                if (userIdentity != null)
                {
                    // Efetua o login com base no Id do usuário e sua senha
                    var resultadoLogin = _signInManager
                        .CheckPasswordSignInAsync(userIdentity.Result, usuario.Password, false)
                        .Result;
                    if (resultadoLogin.Succeeded)
                    {
                        // Verifica se o usuário em questão possui
                        // a role Acesso-APIAlturas
                        credenciaisValidas = _userManager.IsInRoleAsync(
                            userIdentity.Result, Roles.ROLE_API_ALTURAS).Result;
                    }
                }
            }

            if (credenciaisValidas)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.UserID, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserID)
                    }
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao +
                    TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _tokenConfigurations.Issuer,
                    Audience = _tokenConfigurations.Audience,
                    SigningCredentials = _signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                return new
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                };
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("registro")]
        public async Task<JsonResult> Register(UsuarioDto usuario, bool ehAdmin = false)
        {
            var user = new ApplicationUser { UserName = usuario.UserID, Email = usuario.UserID };
            user.Id = "1";
            var result = await _userManager.CreateAsync(user, usuario.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                if (ehAdmin)
                {
                    var resultado = await _userManager.AddToRoleAsync(user, "Admin");
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User created a new account with password.");
                return new JsonResult(Ok());
            }
            AddErrors(result);
            return new JsonResult("Deu ruim");
        }

        //private IActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction(nameof(HomeController.Index), "Home");
        //    }
        //}

        [HttpPost]
        [AllowAnonymous]
        [Route("admin")]
        public async Task<OkResult> CriarPapel()
        {
            var admin = new ApplicationRole { Name = "Admin", Id = "2" };
            var resultado = await _roleManager.CreateAsync(admin);
            if (resultado.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                return Ok();
            }
            throw new Exception("Não foi possível criar essa role");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("virarAdmin")]
        public async Task<OkResult> CriarAdmin(UsuarioDto usuarioDto)
        {
            var usuario = await _userManager.FindByNameAsync(usuarioDto.UserID);

            await _userManager.AddToRoleAsync(usuario, "Admin");

            return Ok();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}