using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IMDbLSLibrary.Tests
{
    public class MovieListTests
    {
        [Fact]
        public void Get_ShouldGetMovieList()
        {
            //Act
            List<MovieModel> actual = MovieList.Get("www.imdb.com/list/ls092551956/");

            //Assert
            Assert.True(actual != null);
            //Assert.Contains<MovieModel>(actual);
        }
        [Fact]
        public void LoadHtmlDocument_ValidUrlShouldWork()
        {
            HtmlAgilityPack.HtmlDocument document = MovieList.LoadHtmlDocument("https://www.imdb.com/list/ls092551956/");
            Assert.NotNull(document);
        }
        [Fact]
        public void LoadHtmlDocument_InvalidUrlShouldFail()
        {
            HtmlAgilityPack.HtmlDocument document = MovieList.LoadHtmlDocument("www.imdb.com/list/ls092551956/");
        }
    }
}
