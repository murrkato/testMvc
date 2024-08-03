using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using testMvc.Models;
using GuessedNumbers;

namespace testMvc.Controllers;

public class HomeController : Controller
{
    public int NumberFrom = 1;
    public int NumberTo = 100;
    public bool StartGame = false;
    public bool PlayerWin = false;
    public bool IsErrorMsg = false;
    public Random NewRandom = new();
    public int? StoredRandom;
    string? StoredRandomStr;
    public int[] reducedRange = new int[2]; 
    private readonly ILogger<HomeController> _logger;
    public enum RangeKeys {From, To};

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(RandomNumModel model)
    {
      bool isStored = string.IsNullOrEmpty(HttpContext.Session.GetString("StoredRandom"));

      if (isStored)
      {
        StoredRandom = NewRandom.Next(NumberFrom, NumberTo);
        StoredRandomStr = StoredRandom?.ToString();

        if (StoredRandomStr != null) {
          HttpContext.Session.SetString("StoredRandom", StoredRandomStr);
        }
      }
      else
      {
        string stored = HttpContext.Session.GetString("StoredRandom") ?? "";
        StoredRandom = Int32.Parse(stored);
      }

      if (model.RandomNum != null)
        TempData["StartGame"] = true;

      if (model.RandomNum != null || StoredRandom != null)
      {
        if (model.RandomNum < NumberFrom || model.RandomNum > NumberTo)
        {
          ViewBag.Message = NumAnswers.OutOfRangeNum;
        } 
        else if (model.RandomNum < StoredRandom)
        {
         OnChangeRange((int)model.RandomNum, RangeKeys.From);

          ViewBag.Message = NumAnswers.HigherNum;
        }
        else if (model.RandomNum > StoredRandom)
        {
          OnChangeRange((int)model.RandomNum, RangeKeys.To);
          ViewBag.Message = NumAnswers.LowerNum;
        }
        else if (model.RandomNum == StoredRandom)
        {
          TempData["PlayerWin"] = true;
          ViewBag.Message = $"{NumAnswers.CorrectNum}{model.RandomNum}";
          model.IsErrorMsg = false;
        }
        else
        {
          ViewBag.Message = NumAnswers.IncorrectNum;
        }
      }
      else
      {
        ViewBag.Message = NumAnswers.IncorrectNum;
      }

      ViewBag.NumberFrom = NumberFrom;
      ViewBag.NumberTo = NumberTo;

      return View();
    }

    public IActionResult Reset()
    {
      ClearData();
      return RedirectToAction("");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public void ClearData()
    {
      HttpContext.Session.Clear();
      reducedRange[0] = NumberFrom;
      reducedRange[1] = NumberTo; 
    }
    public void OnChangeRange(int guessedNum, RangeKeys rangeKey)
    {
      bool isEmpty = string.IsNullOrEmpty(HttpContext.Session.GetString("StoredRange"));    

      reducedRange[0] = NumberFrom;
      reducedRange[1] = NumberTo;
    
      if (rangeKey == RangeKeys.From)
        reducedRange[0] = guessedNum;

      if (rangeKey == RangeKeys.To)
        reducedRange[1] = guessedNum;

      if (isEmpty == false)
      {
        string storedRange = HttpContext.Session.GetString("StoredRange") ?? "";
         
         if (storedRange != null)
         {
          string[] storedRangeArr = storedRange.Split(",");

          if (rangeKey == RangeKeys.From)
            reducedRange[1] = Int32.Parse(storedRangeArr[1]);

          if (rangeKey == RangeKeys.To)
            reducedRange[0] = Int32.Parse(storedRangeArr[0]);
         }
      }

      var StoredRangeStr = string.Join(",", reducedRange);

      if (StoredRangeStr != null) {
        HttpContext.Session.SetString("StoredRange", StoredRangeStr);
      }

      NumberFrom = reducedRange[0];
      NumberTo = reducedRange[1];
    }
}
