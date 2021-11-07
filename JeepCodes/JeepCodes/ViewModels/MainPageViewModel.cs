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

        private string jeepCode = "";
        private string answer;
        private FormattedString formatted = new FormattedString();

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

        public FormattedString FormattedAnswer
        {
            get
            {
                return formatted;
            }
            set
            {
                formatted = value;
                OnPropertyChanged("FormattedAnswer");
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
            string[] _jeepCodes = _jeepCode.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int _jeepCodesLength = _jeepCodes.Length;
            string[] _arrayOfColors = new string[] { "red", "blue", "orange", "green", "yellow", "indigo", "maroon", "aqua", "blueviolet", "brown"};
            Color[] _arrayOfObjectColors = new Color[] { Color.Red, Color.Blue, Color.Orange, Color.Green, Color.Indigo, Color.Maroon, Color.Aqua, Color.BlueViolet, Color.Brown };
            
            FormattedString formattedAnswer = new FormattedString();

            // store the color of matching jeep route in each jeep code
            IDictionary<string, string> _matchingJeepColors = new Dictionary<string, string>();
            IDictionary<string, Color> _matchingJeepObjectColors = new Dictionary<string, Color>();
            if (_jeepCodes.Length == 0 )
            {
                answer = "JeepCode/s is empty";
                formattedAnswer.Spans.Add(new Span { Text = "JeepCode/s is empty" } );
            }
            else
            {
                // loop through to all codes in the jeep codes input from the user
                for (int row = 0; row < _jeepCodesLength; row++)
                {
                    string _code = _jeepCodes[row].Trim();
                    answer += _code + "=>";
                    formattedAnswer.Spans.Add(new Span { Text=_code+"=>" });
                    // get the routes array associated in this jeep code
                    string[] _routesOfJeep = jeepCodesArr[_code];

                    // loopt through to all the routes in this specific jeep code (refere line 87)
                    for (int _routesIndex = 0; _routesIndex < _routesOfJeep.Length; _routesIndex++)
                    {
                        string _value = _routesOfJeep[_routesIndex];
                        bool _existsInOthers = false;
                        string _matchingCode = "";

                        // if jeep codes input by user is greater than, then do these
                        if (_jeepCodesLength > 1)
                        {
                            for (int col = 0; col < _jeepCodesLength; col++)
                            {
                                // current jeep code to check to other jeep codes
                                string _codeToCheckAgainst = _jeepCodes[col].Trim();
                                // current route assiocated to current jeep code
                                string[] _routesOfJeepToCheckAgainst = jeepCodesArr[_codeToCheckAgainst];

                                // check if the _value (line 93) exists in the current route (line 105) and if it exists then they are the same..
                                if (row != col && Array.Exists(_routesOfJeepToCheckAgainst, x => x == _value))
                                {
                                    // check if the current route which is the same to other route has existing color counterpart,
                                    // if there isn't then add specific color for this route
                                    if (!_matchingJeepColors.TryGetValue(_value, out var result))
                                    {
                                        string _color = _arrayOfColors[row % 10];
                                        _matchingJeepColors[_value] = _color;
                                        Color _colorObject = _arrayOfObjectColors[row % 10];
                                        _matchingJeepObjectColors[_value] = _colorObject;
                                    }
                                    _matchingCode = _value;
                                    _existsInOthers = true;
                                    break;
                                }
                            }
                        }
                        if (_existsInOthers)
                        {
                            answer += " <span style='color: " + _matchingJeepColors[_matchingCode] + ";'>" + _value + "</span>";
                            formattedAnswer.Spans.Add(new Span { Text = _value, ForegroundColor = _matchingJeepObjectColors[_matchingCode] });
                        }
                        else
                        {
                            answer += " " + _value;
                            formattedAnswer.Spans.Add(new Span { Text = " " + _value });
                        }
                        if (_routesIndex != _routesOfJeep.Length - 1)
                        {
                            answer += "->";
                            formattedAnswer.Spans.Add(new Span { Text = "<->" });
                        }
                        else if (_routesIndex == _routesOfJeep.Length - 1 && row != _jeepCodesLength - 1)
                        {
                            answer += ", ";
                            formattedAnswer.Spans.Add(new Span { Text = ", " });
                        }
                    }
                }
            }
            Answer = answer;
            FormattedAnswer = formattedAnswer;
        }
    }
}
