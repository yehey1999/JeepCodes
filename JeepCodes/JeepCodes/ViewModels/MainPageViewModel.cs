using JeepCodes.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JeepCodes.ViewModels
{
    class MainPageViewModel : BaseModel
    {
        private IDictionary<string, string[]> jeepCodesArr = new Dictionary<string, string[]>()
        {
            { "01A", new string[] { "Alpha", "Bravo", "Charlie", "Echo", "Golf" } },
            { "02B", new string[] { "Alpha", "Charlie", "Delta", "Foxtrot", "Golf" } },
            { "03C", new string[] { "Charlie", "Delta", "Foxtrot", "Hotel", "India" } },
            { "04A", new string[] { "Charlie", "Delta", "Echo", "Foxtrot", "Golf" } },
            { "04D", new string[] { "Charlie", "Echo", "Foxtrot", "Golf", "India" } },
            { "06B", new string[] { "Delta", "Hotel", "Juliet", "Kilo", "Lima" } },
            { "06D", new string[] { "Delta", "Foxtrot", "Golf", "India", "Kilo" } },
            { "10C", new string[] { "Foxtrot", "Golf", "Hotel", "India", "Juliet" } },
            { "10H", new string[] { "Foxtrot", "Hotel", "Juliet", "Lima", "November" } },
            { "11A", new string[] { "Foxtrot", "Golf", "Kilo", "Mike", "November" } },
            { "11B", new string[] { "Foxtrot", "Golf", "Lima", "Oscar", "Papa" } },
            { "20A", new string[] { "India", "Juliet", "November", "Papa", "Romeo" } },
            { "20C", new string[] { "India", "Kilo", "Lima", "Mike", "Romeo" } },
            { "42C", new string[] { "Juliet", "Kilo", "Lima", "Mike", "Oscar" } },
            { "42D", new string[] { "Juliet", "November", "Oscar", "Quebec", "Romeo" } }
        };

        private string jeepCode;
        private string answer;

        public string JeepCode
        {
            get
            {
                return jeepCode;
            }
            set
            {
                jeepCode = value;
                OnPropertyChanged("JeepCode");
            }
        }

        public string Answer
        {
            get
            {
                return answer;
            }
            set
            {
                answer = value;
                OnPropertyChanged("Answer");
            }
        }

        public Command OnClickShowResultCommand { get; private set; }

        public MainPageViewModel()
        {
            OnClickShowResultCommand = new Command(OnClickShowResult);
        }

        private void OnClickShowResult()
        {
            string answer = "";
            string _jeepCode = JeepCode.ToUpper();
            string[] _jeepCodes = _jeepCode.Split(',');
            int _jeepCodesLength = _jeepCodes.Length;
            string[] _arrayOfColors = new string[] { "blue", "red", "orange", "green", "yellow" };
            IDictionary<string, string> _matchingJeepColors = new Dictionary<string, string>();

            if (_jeepCodes.Length == 0 )
            {
                answer = "JeepCode/s is empty";
            }
            else
            {
                for (int row = 0; row < _jeepCodesLength; row++)
                {
                    string _code = _jeepCodes[row].Trim();
                    answer += _code + "=>";
                    string[] _routesOfJeep = jeepCodesArr[_code];

                    for (int _routesIndex = 0; _routesIndex < _routesOfJeep.Length; _routesIndex++)
                    {
                        string _value = _routesOfJeep[_routesIndex];
                        bool _existsInOthers = false;
                        string _matchingCode = "";

                        if (_jeepCodesLength > 1)
                        {
                            for (int col = 0; col < _jeepCodesLength; col++)
                            {
                                string _codeToCheckAgainst = _jeepCodes[col].Trim();
                                string[] _routesOfJeepToCheckAgainst = jeepCodesArr[_codeToCheckAgainst];

                                if ( row != col && Array.Exists(_routesOfJeepToCheckAgainst, x => x == _value))
                                {
                                    if (!_matchingJeepColors.TryGetValue(_value, out var result))
                                    {
                                        string _color = _arrayOfColors[row % 4];
                                        _matchingJeepColors[_value] = _color;
                                        // answer += _matchingJeepColors[_value];
                                    }
                                    _matchingCode = _value;
                                    _existsInOthers = true;
                                    break;
                                }
                            }
                        }
                        if (_existsInOthers)
                            answer += " <span style='color: " + _matchingJeepColors[_matchingCode] + ";'>" + _value + "</span>";
                        else
                            answer += " " + _value;
                        if (_routesIndex != _routesOfJeep.Length - 1)
                            answer += "->";
                        else if (_routesIndex == _routesOfJeep.Length - 1 && row != _jeepCodesLength - 1)
                            answer += ", ";
                    }
                    // if (row != _jeepCodesLength - 1)
                    //    answer += "<->";
                }
            }
            Answer = answer;
        }
    }
}
