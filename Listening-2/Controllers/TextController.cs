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
using System.Threading.Tasks;

namespace listening.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class TextController : Controller
    {
        private IRepository<Text, string> _textRepository;
        private readonly TextService _textService;

        public TextController(IRepository<Text, string> repository, TextService textService)
        {
            _textRepository = repository;
            _textService = textService;
        }

        // TODO: should be rewritten (add here pagination and filtering and combine this with cache)
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<TextDescriptionDto> GetAllTextsDescription()
        {
            return Mapper.Map<IQueryable<Text>, IEnumerable<TextDescriptionDto>>(_textRepository.GetAll());
        }

        [HttpGet("{textId}")]
        public TextDto GetText(string textId)
        {
            return _textService.GetTextDtoById(textId);
        }

        [HttpPost]
        public string PostText([FromBody]TextDto textDto)
        {
            //Text text = _textService.GenerateTextByDto(textDto);
            //_textRepository.Insert(text);
            _textService.Insert(textDto);
            return $"Success post {textDto.Title}";
        }


        [HttpPut("{id}")]
        public string PutText(string id, [FromBody]TextDto textDto)
        {
            //Text text = _textService.GenerateTextByDto(textDto);

            //_textRepository.Update(text);
            _textService.Update(id, textDto);
            return $"Success put {textDto.Title}";
        }

        [HttpDelete("{id}")]
        public void DeleteText(string id)
        {
            //_textRepository.Delete(id);
            _textService.Delete(id);
        }
    }
}
