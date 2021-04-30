using System;
using System.Collections.Generic;
using IMDbLSLibrary;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            MovieList movieList = new MovieList();
            List<MovieModel> list = movieList.Get();
            int position = 0;
            foreach (var item in list)
            {
                position++;
                Console.WriteLine($"{ position }. { item.MovieName } *{ item.MovieRate }");
            }
        }
    }
}
