using System;
using System.Collections.Generic;
using IMDbLSLibrary;

namespace ConsoleUI
{
    public static class Application
    {
        // https://www.imdb.com/list/ls092551956/
        public static void Run()
        {
            Console.WriteLine($"IMDbLS [Version 0.9a]");
            Console.Write(Environment.NewLine);
            do
            {
                try
                {
                    var userInput = GetUserInput();
                    PrintMovieList(SortMovieList(GetMovieList(userInput.url), userInput.sortType));
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"{ ex.Message }");
                    Console.Write(Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ ex.Message }");
                    Console.Write(Environment.NewLine);
                }
            } while (true);
        }
        static (string url, SortType sortType) GetUserInput()
        {
            string userInput;
            string url = "";
            SortType sortType = SortType.None;

            bool is_url_valid = false;
            bool is_option_valid = false;

            do
            {
                DisplayHelp();
                userInput = Console.ReadLine().ToLower();

                if (userInput.StartsWith("sort" + " "))
                {
                    int option_index = userInput.IndexOf("-");
                    string option = option_index > 0 ? userInput.Substring(option_index, 2) : null;
                    if (option == null)
                    {
                        is_option_valid = true;
                    }
                    else if (option == "-r")
                    {
                        sortType = SortType.Rate;
                        is_option_valid = true;
                    }
                    else if (option == "-d")
                    {
                        sortType = SortType.Date;
                        is_option_valid = true;
                    }
                    else if (option == "-a")
                    {
                        sortType = SortType.Alphabetical;
                        is_option_valid = true;
                    }
                    else
                    {
                        Console.WriteLine($"Bad option { option }.");
                        Console.Write(Environment.NewLine);
                        is_option_valid = false;
                    }
                    if (is_option_valid == true)
                    {
                        int uri_index = userInput.IndexOf(" " + "https://www.imdb.com/");
                        string uri = uri_index > 0 ? userInput.Substring(uri_index) : null;
                        if (uri_index > 0 && Uri.TryCreate(uri, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttps)
                        {
                            url = uriResult.ToString();
                            is_url_valid = true;
                        }
                        else
                        {
                            Console.WriteLine($"URL Structure is invalid. Please check the URL and try again.");
                            Console.Write(Environment.NewLine);
                            is_url_valid = false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"'{ userInput }' is not recognized as a command.");
                    Console.Write(Environment.NewLine);
                }  
            } while ((is_url_valid == true && is_option_valid == true) == false);

            return (url, sortType);
        }
        static List<MovieModel> GetMovieList(string url)
        {
            Console.Write(Environment.NewLine);
            Console.WriteLine($"Getting list from .{ url.Substring(20) }");
            return MovieList.Get(url);
        }
        static List<MovieModel> SortMovieList(List<MovieModel> list, SortType sortType)
        {
            switch (sortType)
            {
                case SortType.Rate:
                    Console.WriteLine($"Sorting list by IMDb Rating");
                    break;
                case SortType.Date:
                    Console.WriteLine($"Sorting list by Release Date");
                    break;
                case SortType.Alphabetical:
                    Console.WriteLine($"Sorting list by Alphabetical");
                    break;
                default:
                    break;
            }
            Console.Write(Environment.NewLine);
            return MovieList.Sort(list, sortType);
        }
        static void PrintMovieList(List<MovieModel> list)
        {
            int rank = 0;
            int max_rank_length;
            int max_name_length = 0;
            int max_date_length = 0;
            foreach (var item in list)
            {
                rank++;
                if (item.MovieName.Length > max_name_length)
                {
                    max_name_length = item.MovieName.Length;
                }
                if (item.MovieDate.Length > max_date_length)
                {
                    max_date_length = item.MovieDate.Length;
                }
            }
            max_rank_length = (int)Math.Floor(Math.Log10(rank) + 1);
            rank = 0;
            string format = "{0,-" + (max_rank_length + 2) + "}{1,-" + (max_name_length + 1) + "}{2,-" + 5 + "}{3, 15}";
            Console.WriteLine(String.Format(format, null, "Name", "Rate", "Release Date"));
            foreach (var item in list)
            {
                rank++;
                item.MovieDate = item.MovieDate.Substring(1, item.MovieDate.Length - 2).Trim();
                if (max_date_length - 2 > 4)
                {
                    switch (item.MovieDate.Length)
                    {
                        case 4:
                            item.MovieDate = item.MovieDate + "     ";
                            break;
                        case 5:
                            item.MovieDate = item.MovieDate + "    ";
                            break;
                        default:
                            item.MovieDate = item.MovieDate;
                            break;
                    }
                }
                Console.WriteLine(String.Format(format, rank + ".", item.MovieName, item.MovieRate, item.MovieDate));
            }
            Console.Write(Environment.NewLine);
        }
        static void DisplayHelp()
        {
            Console.WriteLine($"Usage: sort [-r] [-a] [-d] url");
            Console.Write(Environment.NewLine);
            Console.WriteLine($"Options:");
            Console.WriteLine(String.Format("{0,-4}{1,-15}{2,-10}", "", $"-r", $"Sort by IMDb Rating"));
            Console.WriteLine(String.Format("{0,-4}{1,-15}{2,-10}", "", $"-d", $"Sort by Release Date"));
            Console.WriteLine(String.Format("{0,-4}{1,-15}{2,-10}", "", $"-a", $"Sort by Alphabetical"));
            Console.Write(Environment.NewLine);
        }
    }
}
