using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Storage;

namespace SmashGoBang
{
    public class ScoreHandler
    {
        public List<Score> hiScoreList;
        private int maxScore = 5;
        private string filename = "XNAsavegame";

        public ScoreHandler()
        {
            hiScoreList = LoadHighScores();
        }

        public bool CheckTopScores(int newScore)
        {
            if (hiScoreList.Count < maxScore) { return true; };

            if (newScore > hiScoreList.Last().score)
            {
                return true;
            }

            return false;
        }

        public void AddHiScore(int newScore)
        {
            hiScoreList.Add(new Score {score = newScore, name = "newPlaceHolder" });
        }

        public List<Score> SortScoreList(List<Score> _hiScoreList)
        {
            _hiScoreList = _hiScoreList.OrderByDescending(s => s.score).ToList();

            _hiScoreList = _hiScoreList.Take(5).ToList();

            SaveHighScores(_hiScoreList);

            return _hiScoreList;
        }

        private void SaveHighScores(List<Score> highScores)
        {
            //Get the path of the saved high scores
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string fullpath = Path.Combine(path, filename);

            //Open the file
            FileStream stream = File.Open(fullpath, FileMode.Create, FileAccess.ReadWrite);     //Mindre Bug, den gammla sparfilen skrivs inte över utan den har alla gammla rekord.
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
                serializer.Serialize(stream, hiScoreList);
            }
            finally
            {
                //Close the file
                stream.Close();
            }

        }

        private List<Score> LoadHighScores()
        {
            List<Score> data = new List<Score>();

            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string fullpath = Path.Combine(path, filename);

            // Open the file
            if (File.Exists(fullpath))
            {
                FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);   //var .OpenOrCreate
                    try
                    {                    
                            // Read the data from the file
                            XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
                            data = (List<Score>)serializer.Deserialize(stream);                    
                    }
                    finally
                    {
                        // Close the file
                        stream.Close();
                    }
            }

            data = SortScoreList(data);

            return data;
        }

    }
}
