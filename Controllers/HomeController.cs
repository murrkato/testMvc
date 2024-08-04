using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using testMvc.Models;
using GuessedNumbers;

namespace testMvc.Controllers;

public class HomeController : Controller
{
    private static GameModel? _gameModel;
    private const int from = 1;
    private const int to = 100;
    public HomeController()
    {
        if (_gameModel == null)
        {
          _gameModel = new GameModel
          {
            SecretNumber = new Random().Next(from, to + 1)
          };
        }
    }

    public IActionResult Index(GameModel model, string ButtonClicked)
    {
      model.NumberFrom = from;
      model.NumberTo = to;
      
      if (ButtonClicked == GameModel.ButtonNames.Play.ToString()) {
        if (model.CurrentGuess != null)
          Guess((int)model.CurrentGuess, model);
      }

      if (ButtonClicked == GameModel.ButtonNames.Reset.ToString()) {
        if (model.CurrentGuess != null)
          Clear(model);
      }

      return View(model);
    }

    [HttpPost]
    public void Guess(int CurrentGuess, GameModel model)
    {
      if (_gameModel != null)
      {
        var numFrom = from;
        var numTo = to;

        model.IsErrorMsg = true;

        if (!model.StartGame)
          model.StartGame = true;
          model.CurrentGuess = CurrentGuess;

        if (CurrentGuess < model.NumberFrom || CurrentGuess > model.NumberTo)
        {
          model.Message = NumAnswers.OutOfRange;
        }
        else if (CurrentGuess > _gameModel.SecretNumber)
        {
          numTo = CurrentGuess;
          numFrom = _gameModel.NumberFrom == 0 ? from : _gameModel.NumberFrom;
          model.Message = NumAnswers.Lower;
        }
        else if (CurrentGuess < _gameModel.SecretNumber)
        {
          numFrom = CurrentGuess;
          numTo = _gameModel.NumberTo == 0 ? to : _gameModel.NumberTo;
          model.Message = NumAnswers.Higher;
        }
        else if (CurrentGuess == _gameModel.SecretNumber)
        {
          model.Message = $"{NumAnswers.Correct}{CurrentGuess}";
          model.IsErrorMsg = false;
          model.EndGame = true;
        }
        else
        {
          model.Message = NumAnswers.Incorrect;
        }

        model.NumberFrom = numFrom;
        model.NumberTo = numTo;
        _gameModel.NumberFrom = numFrom;
        _gameModel.NumberTo = numTo;
      }
    }

    public void Clear(GameModel model)
    {
      if (_gameModel != null)
      {
        _gameModel.SecretNumber = new Random().Next(1, 101); 
        _gameModel.CurrentGuess = 0;
        _gameModel.Message = null; 
        _gameModel.NumberFrom = from;
        _gameModel.NumberTo= to;
        _gameModel.StartGame = false;
        _gameModel.EndGame = false;
      }

        model.CurrentGuess = 0;
        model.Message = null; 
        model.NumberFrom = from;
        model.NumberTo= to;
        model.StartGame = false;
        model.EndGame = false;
    }
}
