using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SimpleStore.ViewModels.Supporting_tools
{
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory; // Помощник для построения URL-адресов 
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext] // получение контекста представления
        [HtmlAttributeNotBound]  // для избежания привязки к атрибутам тега 
        public ViewContext ViewContext { get; set; } // контекста представления
        public PageViewModel PageModel { get; set; } // вспомогательные рассчёты для отображения объектов какой-либо модели(ей) и проверки наличия страниц
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")] // установка атрибута для предачи значений 
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>(); // словарь для передачи в хелпер сторонних значений

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext); // объединение помошника и контекста предсталения
            output.TagName = "div"; // замена тега <PageLink> тегом <div>

            // набор ссылок будет представлять список ul
            TagBuilder tag = new TagBuilder("ul"); // неупорядоченный список
            tag.AddCssClass("pagination"); // установка класса

            // формируем три ссылки - на текущую, предыдущую и следующую
            TagBuilder currentItem = CreateTag(PageModel.PageNumber, urlHelper);

            // создаем ссылку на предыдущую страницу, если она есть
            if (PageModel.HasPreviousPage)
            {
                TagBuilder prevItem = CreateTag(PageModel.PageNumber - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }

            tag.InnerHtml.AppendHtml(currentItem); // добавление HTML кода ссылок страниц

            // создаем ссылку на следующую страницу, если она есть
            if (PageModel.HasNextPage)
            {
                TagBuilder nextItem = CreateTag(PageModel.PageNumber + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);
            }
            output.Content.AppendHtml(tag); // добавление отображение тэга неупорядоченного списка, состоящего из элементов ссылок на страницы
        }

        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            TagBuilder item = new TagBuilder("li"); // элемент списка внутри неупорядоченного списка ul
            TagBuilder link = new TagBuilder("a"); // ссылка
            
            if (pageNumber == PageModel.PageNumber)
            {
                item.AddCssClass("active"); // установка класса
            }
            else
            {
                PageUrlValues["page"] = pageNumber; // номер страницы
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues); // ссылки на др. страницы
            }
            
            item.AddCssClass("page-item"); // установка класса
            link.AddCssClass("page-link"); // установка класса
            link.InnerHtml.Append(pageNumber.ToString()); // отображение номера страницы
            item.InnerHtml.AppendHtml(link); //  добавление HTML кода ссылки в элемент неупорядоченного списка ul
            return item; // получение элемента списка представляющего ссылку на страницу
        }
    }
}