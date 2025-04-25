using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CitilinkParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var options = new ChromeOptions();
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            // options.AddArgument("--headless"); // Если вы не хотите открывать окно браузера
            using var driver = new ChromeDriver(options);

            var urlsToParse = new List<string>
            {
                "https://www.citilink.ru/catalog/materinskie-platy/"
                // Можно добавить другие категории или непосредственно ссылки на товары
            };

            var productLinks = new List<string>();

            foreach (var url in urlsToParse)
            {
                productLinks.AddRange(GetProductLinks(driver, url));
            }

            var productInfo = new List<Dictionary<string, object>>();
            foreach (var link in productLinks.Distinct())
            {
                var productDetails = ParseProductInfo(driver, link);
                productInfo.Add(productDetails);
            }

            SaveToCsv(productInfo, "citilink_products.csv");
            Console.WriteLine("Парсинг завершен!");
        }

        static List<string> GetProductLinks(IWebDriver driver, string categoryUrl)
        {
            driver.Navigate().GoToUrl(categoryUrl);
            Thread.Sleep(new Random().Next(7000, 11000)); // Задержка для загрузки страницы

            var links = new List<string>();
            var doc = new HtmlDocument();
            doc.LoadHtml(driver.PageSource);

            //var productNodes = doc.DocumentNode.SelectNodes("//a[contains(@class, 'product')]");
            var productNodes = doc.DocumentNode.SelectNodes("//*[@id=\"__next\"]/div[1]/main/section/div[2]/div/div[3]/section/div[2]/div[2]/div[1]/div/div[2]/div[4]/div[1]/a");

            if (productNodes != null)
            {
                links.AddRange(productNodes.Select(node => node.GetAttributeValue("href", string.Empty)));
            }

            return links;
        }

        static Dictionary<string, object> ParseProductInfo(IWebDriver driver, string productLink)
        {
            driver.Navigate().GoToUrl($"https://www.citilink.ru/{productLink}");
            Thread.Sleep(new Random().Next(2000, 5000)); // Задержка для загрузки страницы

            var doc = new HtmlDocument();
            doc.LoadHtml(driver.PageSource);

            var productDetails = new Dictionary<string, object>
            {
                ["Наименование"] = doc.DocumentNode.SelectSingleNode("//h1")?.InnerText.Trim(),
                ["Цена"] = doc.DocumentNode.SelectSingleNode("//*[@id=\"__next\"]/div[1]/main/div/div[2]/div/div[4]/div/div[3]/div/div[2]/div/div[2]/span/span/span[1]")?.InnerText.Trim(),
                ["Описание"] = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'ProductDescription')]")?.InnerText.Trim(),
                ["Ссылка"] = productLink
            };

            return productDetails;
        }

        static void SaveToCsv(List<Dictionary<string, object>> data, string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                // Запись заголовков
                writer.WriteLine(string.Join(",", data[0].Keys));

                // Запись данных
                foreach (var product in data)
                {
                    writer.WriteLine(string.Join(",", product.Values.Select(v => v.ToString().Replace(",", ";"))));
                }
            }
        }
    }
}