using AutoMapper;
using listening.Models.Text;
using listening.Models.TextViewModels;
using listening.Repositories;
using listening.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace listening.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class WordController : Controller
    {
        private readonly TextService _textService;
        //private readonly IRepository<Text> _textRepository;

        public WordController(/*IRepository<Text> repository, */TextService textService)
        {
            //_textRepository = repository;
            _textService = textService;
        }

        [HttpGet("wordsInParagraphs/{id}")]
        public TextDto GetWordsInParagraphs(string id)
        {
            return _textService.GetTextDtoById(id);
            //return Mapper.Map<TextDto>(_textRepository.GetById(id));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public JsonResult GetWordsCountInParagraphs(string id)
        {
            //var wordsInParagraphs = _textRepository.GetById(id).WordsInParagraphs;
            //var wordsCounts = _textService.GetWordCounts(wordsInParagraphs);
            var wordsCounts = _textService.GetWordCounts(id);

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(wordsCounts);
        }

        [AllowAnonymous]
        [HttpGet("letter/{id}/{paragraphIndex}/{wordIndex}/{symbolIndex}")]
        public JsonResult GetLetter(string id, int paragraphIndex, int wordIndex, int symbolIndex)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(_textService.GetWordsInParagraphs(id)[paragraphIndex]
                            [wordIndex][symbolIndex]);
        }

        [AllowAnonymous]
        [HttpGet("wordCorrectness/{id}/{paragraphIndex}/{wordIndex}/{value}")]
        public JsonResult GetWordCorrectness(string id, int paragraphIndex, int wordIndex, string value)
        {
            value = value.Replace("`", "'");
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(value.Equals(_textService.GetWordsInParagraphs(id)[paragraphIndex][wordIndex]));
        }

        [AllowAnonymous]
        [HttpPost("wordsForCheck/{id}")]
        public JsonResult PostCheckWords(string id, [FromBody]string[] words)
        {
            var formattedWords = words.Select(x => x.Replace("`", "'")).ToArray();
            var wordsInParagraphs = _textService.GetWordsInParagraphs(id);
            var correctWordLocatorsDtoList = new List<CorrectWordLocatorsDto>();

            foreach (var word in formattedWords)
            {
                var locators = new List<WordLocatorDto>();

                for (int i = 0; i < wordsInParagraphs.Length; i++)
                    for (int j = 0; j < wordsInParagraphs[i].Length; j++)
                        if (word.Equals(wordsInParagraphs[i][j], StringComparison.CurrentCultureIgnoreCase))
                            locators.Add(new WordLocatorDto
                            {
                                ParagraphIndex = i,
                                WordIndex = j,
                                IsCapital = char.IsUpper(wordsInParagraphs[i][j].First())
                            });

                if (locators.Count > 0)
                    correctWordLocatorsDtoList.Add(
                        new CorrectWordLocatorsDto { Locators = locators.ToArray(), Word = word });
            }

            return Json(correctWordLocatorsDtoList);
        }
    }
}
