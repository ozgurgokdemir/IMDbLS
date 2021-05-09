using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;

namespace IMDbLSLibrary
{
    public static class MovieList
    {
        public static List<MovieModel> Get(string url)
        {
            HtmlDocument document = LoadHtmlDocument(url);
            if (url.Contains("list") && url.Contains("watchlist") == false || url.Contains("ratings"))
            {
                return GetInnerText(document, "//div[@class='lister-list']//div[@class='lister-item-content']", ".//h3[@class='lister-item-header']//a", 
                    ".//span[@class='ipl-rating-star__rating']", ".//h3[@class='lister-item-header']//span[@class='lister-item-year text-muted unbold']");
            }
            else if (url.Contains("top") || url.Contains("meter"))
            {
                return GetInnerText(document, "//*[@class='lister-list']//tr", ".//td[@class='titleColumn']//a", 
                    ".//td[@class='ratingColumn imdbRating']//strong", ".//td[@class='titleColumn']//span[@class='secondaryInfo']");
            }
            else
            {
                throw new ArgumentException($"URL Subdirectory is missing. Please check the URL and try again.");
            }
        }
        public static HtmlDocument LoadHtmlDocument(string url)
        {
            HtmlWeb web = new();
            return web.Load(url);
        }
        private static List<MovieModel> GetInnerText(HtmlDocument document, string mainXpath, string nameXpath, string rateXpath, string dateXpath)
        {
            List<MovieModel> output = new List<MovieModel>();
            foreach (HtmlNode node in document.DocumentNode.SelectNodes(mainXpath) ??
                throw new ArgumentException($"List could not found. Please check the URL and try again."))
            {
                if (String.IsNullOrWhiteSpace(node.InnerText.ToString()) == false)
                {
                    MovieModel models = new MovieModel
                    {
                        MovieName = node.SelectSingleNode(nameXpath).InnerText.ToString(),
                        MovieRate = node?.SelectSingleNode(rateXpath)?.InnerText.ToString(),
                        MovieDate = node?.SelectSingleNode(dateXpath)?.InnerText.ToString()
                    };
                    output.Add(models);
                }
            }
            return output;
        }
        public static List<MovieModel> Sort(List<MovieModel> movieList, SortType typeOfSort)
        {
            switch (typeOfSort)
            {
                case SortType.Rate:
                    return movieList?.OrderByDescending(x => x.MovieRate).ThenBy(x => x.MovieName).ThenByDescending(x => x.MovieDate).ToList();
                case SortType.Date:
                    return movieList?.OrderByDescending(x => x.MovieDate.Substring(1, 5)).ThenByDescending(x => x.MovieRate).ThenBy(x => x.MovieName).ToList();
                case SortType.Alphabetical:
                    return movieList?.OrderBy(x => x.MovieName).ThenByDescending(x => x.MovieRate).ThenByDescending(x => x.MovieDate).ToList();
                default:
                    return movieList;
            }
        }
    }
}
