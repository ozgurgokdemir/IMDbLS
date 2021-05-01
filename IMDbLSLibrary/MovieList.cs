using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;

namespace IMDbLSLibrary
{
    public class MovieList
    {
        public List<MovieModel> Get(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(url);
            if (url.Contains("list") && url.Contains("watchlist") == false || url.Contains("ratings"))
            {
                return GetInnerText(document, "//div[@class='lister-list']//div[@class='lister-item-content']", ".//h3[@class='lister-item-header']//a", ".//span[@class='ipl-rating-star__rating']");
            }
            else if (url.Contains("top") || url.Contains("meter"))
            {
                return GetInnerText(document, "//*[@class='lister-list']//tr", ".//td[@class='titleColumn']//a", ".//td[@class='ratingColumn imdbRating']//strong");
            }
            else
            {
                return null;
            }
        }
        private List<MovieModel> GetInnerText(HtmlDocument document, string mainXpath, string nameXpath, string rateXpath)
        {
            
            List<MovieModel> output = new List<MovieModel>();
            foreach (HtmlNode node in document.DocumentNode.SelectNodes(mainXpath))
            {
                if (string.IsNullOrWhiteSpace(node.InnerText.ToString()) == false)
                {
                    MovieModel models = new MovieModel
                    {
                        MovieName = node.SelectSingleNode(nameXpath).InnerText.ToString(),
                        MovieRate = node?.SelectSingleNode(rateXpath)?.InnerText.ToString()
                    };
                    output.Add(models);
                }
            }
            return output;
        }
        public List<MovieModel> Sort(List<MovieModel> movieList)
        {
            return movieList.OrderByDescending(x => x.MovieRate).ThenBy(x => x.MovieName).ToList();
        }
    }
}
