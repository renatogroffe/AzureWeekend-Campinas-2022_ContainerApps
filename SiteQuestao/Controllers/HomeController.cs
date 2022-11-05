﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SiteQuestao.Models;

namespace SiteQuestao.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Acessando a página de votação...");
        if (!String.IsNullOrWhiteSpace(Request.Query["voto"]))
            TempData["Voto"] = Request.Query["voto"].ToString();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}