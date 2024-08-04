namespace testMvc.Models;

public class GameModel
{
  public bool StartGame {get; set;}
  public bool EndGame {get; set;}

  public int SecretNumber {set; get;}
  public int NumberFrom {get; set;}
  public int NumberTo {get; set;}
  public int? CurrentGuess { get; set; }


  public bool IsErrorMsg {get; set;}
  public string? Message {get; set;}

  public enum ButtonNames {Play, Reset}
  public ButtonNames ButtonClicked {get; set;}
}