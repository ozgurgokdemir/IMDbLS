using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace IMDbLSLibrary
{
    public class MovieList
    {
        public List<MovieModel> Get()
        {
            List<MovieModel> output = new List<MovieModel>();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://www.imdb.com/list/ls092551956/");
            foreach (HtmlNode node in document.DocumentNode.SelectNodes("//div[@class='lister-item-content']"))
            {
                if (string.IsNullOrWhiteSpace(node.InnerText.ToString()) == false)
                {
                    MovieModel models = new MovieModel
                    {
                        MovieName = node.SelectSingleNode(".//a[starts-with(@href, '/title/')]").InnerText.ToString(),
                        MovieRate = node.SelectSingleNode(".//span[@class='ipl-rating-star__rating']").InnerText.ToString()
                    };
                    output.Add(models);
                }
            }
            return output;
        }
    }
}
