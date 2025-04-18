﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AppSemTemplate.Extensions
{
    public class CustomAuthorization
    {
        public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
        {
            if (context.User.Identity == null)
            {
                throw new InvalidOperationException();
            }

            bool usuarioAutenticado = context.User.Identity.IsAuthenticated;
            bool possuiNomeDaClaim = context.User.Claims.Any(c => c.Type == claimName);
            bool contemValorDaClaim = context.User.Claims.Any(c => c.Value.Split(',').Contains(claimValue));

            return usuarioAutenticado && possuiNomeDaClaim && contemValorDaClaim;
        }
    }

    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequisitoClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity == null)
            {
                throw new InvalidOperationException();
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            area = "Identity",
                            page = "/Account/Login",
                            ReturnUrl = context.HttpContext.Request.Path.ToString()
                        }
                    )
                );
                return;
            }

            if (!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = [new Claim(claimName, claimValue)];
        }
    }
}
