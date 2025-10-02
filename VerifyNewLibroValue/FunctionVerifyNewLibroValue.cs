using Biblioteca.Infraestructura.Identity.Entities;
using Bliblioteca.Core.Aplication.Dto.Email;
using Bliblioteca.Core.Aplication.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace VerifyNewLibroValue;

public class FunctionVerifyNewLibroValue
{
    private readonly ILogger _logger;
    private readonly ILibroServices _libroServices;
    private readonly IEmailServices _emailServices;
    private readonly UserManager<AppUser> _userManager;

    public FunctionVerifyNewLibroValue(ILoggerFactory loggerFactory, ILibroServices libroServices, IEmailServices emailServices, UserManager<AppUser> userManager)
    {
        _logger = loggerFactory.CreateLogger<FunctionVerifyNewLibroValue>();
        _libroServices = libroServices;
        _emailServices = emailServices;
        _userManager = userManager;
    }


    [Function("FunctionVerifyNewLibroValue")]
    public async Task Run ([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        try 
        {
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation("Next timer schedule at: {NextSchedule}", myTimer.ScheduleStatus.Next);
            }

            _logger.LogInformation("C# Timer trigger function executed at: {ExecutionTime}", DateTime.Now);

            if (myTimer.IsPastDue)
            {
                _logger.LogWarning("The timer is past due!");
            }

            else
            {
                var libros = await _libroServices.GetAllListAsync();

                if (libros.Count == 0)
                {
                    _logger.LogInformation("No hay libros registrados.");
                    return;
                }
                var emailContent = string.Join("", libros.Select(l =>
                     $@"
                    <tr>
                        <td style='padding: 15px; border-bottom: 1px solid #e0e0e0;'>
                            <div style='font-size: 16px; font-weight: 600; color: #2c3e50; margin-bottom: 5px;'>
                                 {l.Titulo}
                            </div>
                            <div style='font-size: 14px; color: #7f8c8d;'>
                                <strong>Autor:</strong> {l.Autor}
                            </div>
                        </td>
                    </tr>"
                ));

                var allUserEmail = await _userManager.Users
                    .Where(u => u.EmailConfirmed && !string.IsNullOrWhiteSpace(u.Email))
                    .Select(u => u.Email!)
                    .ToListAsync();

                EmailRequestDto emailRequestDto = new()
                {
                    ToRange = allUserEmail,
                    Subject = "Listado de Libros Registrados",
                    HtmlBdy = $@"
                        <!DOCTYPE html>
                        <html lang='es'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title>Listado de Libros</title>
                        </head>
                        <body style='margin: 0; padding: 0; font-family: Arial, Helvetica, sans-serif; background-color: #f4f4f4;'>
                            <table role='presentation' style='width: 100%; border-collapse: collapse; background-color: #f4f4f4;'>
                                <tr>
                                    <td style='padding: 20px 0;'>
                                        <table role='presentation' style='width: 100%; max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                    
                                            <!-- Header -->
                                            <tr>
                                                <td style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 8px 8px 0 0;'>
                                                    <h1 style='margin: 0; color: #ffffff; font-size: 28px; font-weight: 700;'>
                                                          Biblioteca Digital
                                                    </h1>
                                                    <p style='margin: 10px 0 0 0; color: #f0f0f0; font-size: 14px;'>
                                                        Listado actualizado de libros registrados
                                                    </p>
                                                </td>
                                            </tr>
                    
                                            <!-- Saludo -->
                                            <tr>
                                                <td style='padding: 30px 30px 20px 30px;'>
                                                    <p style='margin: 0 0 15px 0; font-size: 16px; color: #2c3e50; line-height: 1.6;'>
                                                        Estimado usuario, no olvides tus próximas lecturas.
                                                    </p>
                                                    <p style='margin: 0; font-size: 16px; color: #2c3e50; line-height: 1.6;'>
                                                        Estos son los libros que has guardado en tu biblioteca personal y aún requieren tu atención:
                                                    </p>
                                                </td>
                                            </tr>
                    
                                            <!-- Lista de Libros -->
                                            <tr>
                                                <td style='padding: 0 30px;'>
                                                    <table role='presentation' style='width: 100%; border-collapse: collapse; background-color: #f8f9fa; border-radius: 6px; overflow: hidden;'>
                                                        {emailContent}
                                                    </table>
                                                </td>
                                            </tr>
                    
                                            <!-- Información adicional -->
                                            <tr>
                                                <td style='padding: 30px; text-align: center;'>
                                                    <div style='background-color: #e8f4f8; padding: 20px; border-radius: 6px; border-left: 4px solid #667eea;'>
                                                        <p style='margin: 0; font-size: 14px; color: #34495e; line-height: 1.6;'>
                                                            <strong>Tareas de lectura pendientes:</strong> {libros.Count()}
                                                        </p>
                                                    </div>
                                                </td>
                                            </tr>
                    
                                            <!-- Footer -->
                                            <tr>
                                                <td style='background-color: #2c3e50; padding: 25px; text-align: center; border-radius: 0 0 8px 8px;'>
                                                    <p style='margin: 0 0 10px 0; font-size: 14px; color: #ecf0f1;'>
                                                        © {DateTime.Now.Year} Tu Biblioteca Digital. Todos los derechos reservados.
                                                    </p>
                                                    <p style='margin: 0; font-size: 12px; color: #95a5a6;'>
                                                        Este es un correo automático, por favor no responder.
                                                    </p>
                                                </td>
                                            </tr>
                    
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </body>
                        </html>"
                };
                await _emailServices.SendAsync(emailRequestDto);
            }
        }
        catch (Exception ex) 
        {
            _logger.LogError("Error in FunctionVerifyNewLibroValue: {Message}", ex.Message);
            return;
        }        
    }
}