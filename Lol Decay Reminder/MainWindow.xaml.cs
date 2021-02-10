using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Lol_Decay_Reminder.Templates;
using Lol_Decay_Reminder.Models;
using Lol_Decay_Reminder.Helper_Classes;
using MongoDB.Bson;
using Match = Lol_Decay_Reminder.Models.Match;
using System.Globalization;

namespace Lol_Decay_Reminder
{
    //TODO: 
    // Future plans: add number of games played etc (master+ needs 10 games a week) 


    public partial class MainWindow : Window
    {
        private readonly DBClass _dBClass;
        private readonly string _ApiKey = "#"; //add key here
        public List<SavedUsersModel> listOfNames = new List<SavedUsersModel>();
        private Dictionary<string, int> DicOfDecayRanks()
        {
            //List of all ranks with decay
           return new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"Diamond", 28 },
                {"Master", 7 },
                {"GrandMaster", 7 },
                {"Challenger", 7 }
            };
        }

        public MainWindow()
        {
            InitializeComponent();
            _dBClass = new DBClass();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitialSetup();
        }

        public void InitialSetup()
        {
            //Initial setup, where it fills list of existing users and informations
            txtSearch.BorderThickness = new Thickness { Top = 1, Bottom = 0, Left = 1, Right = 1 };
            var _SavedUsers = _dBClass.GetAll();
            listOfNames.AddRange(_SavedUsers.Select(a => new SavedUsersModel(a.name, a.region)));
            List<string> ListOfRegions = new List<string> { "ALL", "EUW", "EUNE", "NA", "BR", "LAN", "LAS", "OCE", "RU", "TR", "JP", "KR" };

            ListOfRegions.ForEach(x => CbRegion.Items.Add(x));
            listOfNames.ForEach(x => UpdateList(new SavedUsersModel(x.Name, x.Region)));
            CbRegion.SelectedIndex = 0;
          
            if (SpNames.Children.Count >= 1)
            {
                var btn = (Button)SpNames.Children[0];
                Btn_Click(btn, null);
            }
            //Gets list of all accounts which is close to the decay timer (10 days), by getting their current rank and userinfo
            List<DbUserModel> warnings = new List<DbUserModel>();
            warnings = _SavedUsers.AsParallel().Where(x => x.lastMatch.AddDays
                  (
                  GetDecayTimer(
                        GetRankFromAPI(new SavedUsersModel(x.name, x.region),
                        GetAccountFromAPI(new SavedUsersModel(x.name, x.region)).id)
                      )
                  ).Subtract(DateTime.Now).Days <= 10).ToList();
            /* old warning using db
            var warnings = _SavedUsers.Where(x => x.lastMatch.AddDays(28).Subtract(DateTime.Now).Days <= 10).ToList();
             */
            if (warnings.Count > 0)
            {
                //Warns user if decay timer is close at hand
                string warning = "Warning! Decay Timer Eminent on following account(s):\n";
                warning += string.Join("\n", warnings.Select(x => x.name + $": {x.lastMatch.AddDays(DicOfDecayRanks()[x.tier]).Subtract(DateTime.Now).Days} Days Left!")).Replace("%20", " ");
                MessageBox.Show(warning);
            }
        }
    public static DateTime UnixTimeToDateTime(long unixtime)
        {
            //Converts unix time to datetime format
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dtDateTime;
        }
        private void TxtSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Clear();
        }
        public string ConvertRegion(string content)
        {
            // Converts regions to match Riot Devoloper APi Region requests
            string result = "";
            switch (content)
            {
                case "EUW":
                    result = "Euw1";
                    break;
                case "EUNE":
                    result = "Eun1";
                    break;
                case "NA":
                    result = "Na1";
                    break;
                case "BR":
                    result = "Br1";
                    break;
                case "LAN":
                    result = "La1";
                    break;
                case "LAS":
                    result = "La2";
                    break;
                case "OCE":
                    result = "Oc1";
                    break;
                case "RU":
                    result = "Ru1";
                    break;
                case "TR":
                    result = "Tr1";
                    break;
                case "JP":
                    result = "Jp1";
                    break;
                case "KR":
                    result = "Kr";
                    break;
            }
            return result;
        }
        private void BtnAddNew_Confirm(object sender, RoutedEventArgs e)
        {
            AddNew addNew = new AddNew(this);
            addNew.Show();
        }
        public async void BtnAddNewConfirmed(SavedUsersModel NewUser)
        {
            // Section for adding new users
            SavedUsersModel user = new SavedUsersModel(NewUser.Name, ConvertRegion(NewUser.Region));
            if (user.Name.Contains(" "))
                user.Name = user.Name.Replace(" ", "%20");
            if (!listOfNames.Contains(user))
            {
                //Checks if user is existing
                var getUser = GetUserFromAPi(user);
                if (getUser != null)
                {
                    if (!DicOfDecayRanks().ContainsKey(GetRankFromAPI(user, getUser.id).tier))
                    {
                        var dialogResult = MessageBox.Show("Looks like the rank you're trying to add does not fall under the Decay System, are you sure you want to proceed?", "Confirmation", MessageBoxButton.YesNoCancel);
                        if (dialogResult == MessageBoxResult.No || dialogResult == MessageBoxResult.Cancel)
                            return;
                    }
                    listOfNames.Add(user);
                    UpdateList(user);
                    await Task.Run(() => _dBClass.Create(getUser));
                    _dBClass.Create(getUser);
                }
            }
        }
        public void UpdateList(SavedUsersModel user)
        {
            var name = user.Name;
            if (name.Contains("%20"))
                name = name.Replace("%20", " ");
            var btn = buttonSetup(name, 14, Brushes.White, Brushes.Black, HorizontalAlignment.Left);
            btn.Name = $"listOfNames{SpNames.Children.Count}";
            btn.Click += Btn_Click;
            SpNames.Children.Add(btn);
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
                var name = (sender as Button).Name;
                name = Regex.Replace(name, "[^0-9.]", "");
            if (!string.IsNullOrEmpty(name)) 
                FillForm(_dBClass.GetOne(listOfNames[Convert.ToInt32(name)].Name));
            
        }
        public void FillForm(DbUserModel dbUser)
        {
            string name = dbUser.name;
            if (name.Contains("%20"))
                name = dbUser.name.Replace("%20", " ");
            lbName.Content = name;
            var id =  GetAccountFromAPI(new SavedUsersModel(dbUser.name, dbUser.region)).id;
            var rank = GetRankFromAPI(new SavedUsersModel(dbUser.name, dbUser.region), id);
            lbRank.Content = $"{rank.tier} {rank.rank}";
            lbSummonerLvl.Content = dbUser.summonerLvl;
            lbLastMatch.Content = dbUser.lastMatch;
            var decayTimer = GetDecayTimer(rank);
            lbDaysLeft.Content = (dbUser.lastMatch.AddDays(decayTimer).Subtract(DateTime.Now)).Days;
            
        }
        public int GetDecayTimer(IRankModel rank)
        {
            var decayTimer = 1000;
            if (DicOfDecayRanks().ContainsKey(rank.tier))
                decayTimer = DicOfDecayRanks()[rank.tier];
            return decayTimer;
        }

        #region Riot API Call
        public DbUserModel GetUserFromAPi(SavedUsersModel savedUser)
        {
            try
            {
                var User = GetAccountFromAPI(savedUser);
                var LastMatch = GetLastMatchFromAPI(savedUser, User.accountId);
                var lastMatchTime = UnixTimeToDateTime(LastMatch.timestamp);
                var rank = GetRankFromAPI(savedUser, User.id);
                return new DbUserModel(ObjectId.Empty, savedUser.Name, savedUser.Region, User.summonerLevel, LastMatch.role, lastMatchTime, User.id, User.accountId, User.puuid, LastMatch.gameId, rank.rank, rank.tier);
            }
            catch
            {
                MessageBox.Show("Error! User does not exist / No Internet Connection Detected!");
                return null;
            }
        }
        public IUserModel GetAccountFromAPI(SavedUsersModel savedUsers)
        {
            return JsonConvert.DeserializeObject<UserModel>(new WebClient().DownloadString($"https://{savedUsers.Region}.api.riotgames.com/lol/summoner/v4/summoners/by-name/{savedUsers.Name}?api_key={_ApiKey}"));
        }
        public IMatch GetLastMatchFromAPI(SavedUsersModel savedUser, string accountId)
        {
            return JsonConvert.DeserializeObject<MatchesModel>(new WebClient().DownloadString($"https://{savedUser.Region}.api.riotgames.com/lol/match/v4/matchlists/by-account/{accountId}?queue=420&api_key={_ApiKey}")).matches.FirstOrDefault();
        }
        public IRankModel GetRankFromAPI(SavedUsersModel savedUsers, string SummonerId)
        {
            return JsonConvert.DeserializeObject<List<RankModel>>(new WebClient().DownloadString($"https://{savedUsers.Region}.api.riotgames.com/lol/league/v4/entries/by-summoner/{SummonerId}?api_key={_ApiKey}"))[0];
        }
        #endregion

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var user =_dBClass.GetOne(lbName.Content.ToString().Replace(" ", "%20"));
            _dBClass.Delete(user);
            SpNames.Children.Clear();
            listOfNames.RemoveAll(x => x.Name == user.name);
            listOfNames.ForEach(x => UpdateList(x));
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SpNames.Children.Clear();
            if (txtSearch.Text == string.Empty)
            {
                listOfNames.ForEach(x => UpdateList(x));
            }
            else
            {
                var FilteredList = new List<SavedUsersModel>(listOfNames);
                FilteredList.RemoveAll(x => !x.Name.Contains(txtSearch.Text, (StringComparison)CompareOptions.IgnoreCase));
                FilteredList.ForEach(x => UpdateList(x));
            }
        }

        private void CbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbRegion.SelectedItem.ToString() == "ALL")
            {
                SpNames.Children.Clear();
                listOfNames.ForEach(x => UpdateList(x));
            }
            else
            {
                var FilteredList = new List<SavedUsersModel>(listOfNames);
                FilteredList.RemoveAll(x => x.Region != ConvertRegion(CbRegion.SelectedItem.ToString()));
                SpNames.Children.Clear();
                FilteredList.ForEach(x => UpdateList(x));
            }
        }
    }
}



