using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HHruParser
{
    public class ApiWorker
    {
        public static List<VacancyCategories> GetCategories()
        {
            List<VacancyCategories> vacancyCategories = new List<VacancyCategories>();
            WebClient webClient = new WebClient();

            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705)");

            string htmlData = webClient.DownloadString("https://hh.ru/?hhtmFrom=main");

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlData);

            HtmlNodeCollection categoriesList = document.DocumentNode.
                SelectNodes("//div[@class='dashboard-tiles-item dashboard-tiles-item_active']");

            for (int i = 0; i < categoriesList.Count; i++)
            {
                string categoryVacancy = categoriesList[i]
                    .SelectSingleNode("section/div/div/h2")
                    .InnerText.TrimStart();
                string link = categoriesList[i]
                    .SelectSingleNode("section/div/div/a")
                    .GetAttributeValue("href", "Default");

                vacancyCategories.Add(new VacancyCategories()
                {
                    Title = categoryVacancy,
                    Link = link
                });
            }

            for (int i = 0; i < categoriesList.Count; i++)
            {
                string vacancyTitle = string.Empty;
                string filmRating = string.Empty;
                string vacancyLink = string.Empty;
                HtmlNodeCollection categoryFilms = categoriesList[i].
                    SelectNodes("section/tiles-carousel-responsive/tiles-carousel-responsive-item");

                for (int j = 0; j < categoryFilms.Count; j++)
                {
                    HtmlNode node = categoryFilms[j].SelectSingleNode("tile-dynamic/a/span");

                    if (node != null)
                    {
                        vacancyTitle = categoryFilms[j].SelectSingleNode("tile-dynamic/a/span").InnerText;
                        filmRating = categoryFilms[j].SelectSingleNode("tile-dynamic/a/score-pairs").InnerText.TrimStart();
                        vacancyLink = categoryFilms[j].SelectSingleNode("tile-dynamic/a").GetAttributeValue("href", "Default");
                    }
                    else
                    {
                        vacancyTitle = categoryFilms[j].SelectSingleNode("a/tile-dynamic/div/span").InnerText;
                        filmRating = categoryFilms[j].SelectSingleNode("a/tile-dynamic/div/score-pairs").InnerText.TrimStart();
                        vacancyLink = categoryFilms[j].SelectSingleNode("a").GetAttributeValue("href", "Default");
                    }

                    Vacancy film = new Vacancy() { Title = vacancyTitle, Link = vacancyLink };
                    //filmsCategories[i].Films.Add(film);
                }
            }



            //for (int i = 0; i < filmsCategories.Count; i++)
            //{
            //    HtmlDocument documentFilm = new HtmlDocument();
            //    HtmlAgilityPack.HtmlWeb web = new HtmlWeb();

            //    if (filmsCategories[i].Link.Contains("https"))
            //    {
            //        documentFilm = web.Load($"{filmsCategories[i].Link}");
            //    }
            //    else
            //    {
            //        documentFilm = web.Load($"https://www.rottentomatoes.com{filmsCategories[i].Link}");
            //    }



            //    HtmlNodeCollection filmsList = documentFilm.DocumentNode.SelectNodes("//div[@class='discovery-tiles__wrap']");

            //    for (int j = 0; j < filmsList.Count; j++)
            //    {
            //        //filmsCategories[i].Films.Add(filmsList[j].InnerText.TrimStart());
            //    }
            //}

            return vacancyCategories;
        }

        async function fetchCategories(group)
        {
            return new Promise((resolve) => {
                setTimeout(() => {
                    const categoriesContainer = document.querySelector(
                    ".dashboard-tiles-item-drop-container_visible"
                    );
                    // console.log(categoriesContainer);

                    const titles = Array.from(
                    categoriesContainer.querySelectorAll(".multiple-column-list-item a")
                    ).map((anchor) => anchor.innerText);

                    const descriptions = Array.from(
                    categoriesContainer.querySelectorAll(
                    ".dashboard-tiles-drop-compensation"
                    )
                    ).map((div) => div.innerText);

                    resolve(
                    titles.map((title, idx) => {
                        return { title, description: descriptions[idx] };
                    })
                    );
                }, 100);
            });
        }

        public async function collectCategories()
        {
            document.querySelector('.dashboard-tiles-content button.bloko-link').dispatchEvent(new Event("click", { bubbles: true }));

            let result = [];
            const groups = Array.from(document.querySelectorAll(".dashboard-tiles-item"));
            for (let idx = 0; idx < groups.length; idx++)
            {
                console.log(groups[idx])
            groups[idx]
            .querySelector(".dashboard-tiles-item__body")
            .dispatchEvent(new Event("click", { bubbles: true }));

            await fetchCategories(groups[idx]).then((categories) => {
            result.push({ ...categories });
        });
}
    console.log(result);
}

collectCategories();
    }
}
