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
        private IRepository<Text> _textRepository;
        private readonly TextService _textService;

        public TextController(IRepository<Text> repository, TextService textService)
        {
            _textRepository = repository;
            _textService = textService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<TextDescriptionDto> GetAllTextsDescription()
        {
            return Mapper.Map<IQueryable<Text>, IEnumerable<TextDescriptionDto>>(_textRepository.GetAll());
        }

        [HttpGet("{textId}")]
        public TextDto GetText(string textId)
        {
            return _textService.GenerateTextDtoById(textId); ;
        }

        [HttpPost]
        public string PostText([FromBody]TextDto textDto)
        {
            Text text = _textService.GenerateTextByDto(textDto);
            _textRepository.Insert(text);
            return $"Success post {textDto.Title}";
        }


        [HttpPut("{id}")]
        public string PutText(string id, [FromBody]TextDto textDto)
        {
            Text text = _textService.GenerateTextByDto(textDto);

            _textRepository.Update(text);
            return $"Success put {textDto.Title}";
        }

        [HttpDelete("{id}")]
        public void DeleteText(string id)
        {
            _textRepository.Delete(id);
        }
    }
}
