using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using testMvc.Models;
using GuessedNumbers;

namespace testMvc.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {
        _gameModel = new GameModel();
        _gameModel.SecretNumber = new Random().Next(1, 101);
        Console.WriteLine($"SecretNumber { _gameModel.SecretNumber}");
    }

    private GameModel _gameModel;

    public IActionResult Index(GameModel model)
    {
      return View(model);
    }

    [HttpPost]
    public IActionResult Guess(int CurrentGuess)
    {
      if (!_gameModel.StartGame)
        _gameModel.StartGame = true;
        _gameModel.CurrentGuess = CurrentGuess;

      Console.WriteLine($"SecretNumber2 { _gameModel.SecretNumber}");

      return View("Index", _gameModel);
    }
}
