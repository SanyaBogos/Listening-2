using AutoMapper;
using listening.Exceptions;
using listening.Models;
using listening.Models.Text;
using listening.Models.TextViewModels;
using listening.Repositories;
using listening.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace listening.Services
{
    public class TextService
    {
        private readonly IRepository<Text, string> _textRepository;
        private string[] _specialSymbols;
        private char[] _moneySymbols;
        //private GlobalCache _globalCache;
        private GlobalCache<TextCache, string> _textCache;
        //private GlobalCache<Result, int> _resultsCache;
        private const int MaxLength = 4000;

        public TextService()
        {
            _specialSymbols = new string[] { ",", ".", "?", ":", ";", "-", "!" };
            _moneySymbols = new char[] { '$', '£', '€' };
        }

        public TextService(IRepository<Text, string> repository,
            GlobalCache<TextCache, string> globalCache,
            GlobalCache<Result, int> resultsCache) : this()
        {
            _textRepository = repository;
            _textCache = globalCache;
        }

        public string[][] GetWordCounts(string textId)
        {
            var cachedTextDto = _textCache.GetTextCached(textId);
            if (cachedTextDto != null)
                return cachedTextDto.CountsInParagraphs;

            ReadAndInsertToCacheText(textId);

            return GetWordCounts(textId);
        }

        public string[][] GetWordsInParagraphs(string textId)
        {
            var cachedTextDto = _textCache.GetTextCached(textId);
            if (cachedTextDto != null)
                return cachedTextDto.WordsInParagraphs;

            ReadAndInsertToCacheText(textId);

            return GetWordsInParagraphs(textId);
        }

        public string[][] GetWordCounts(string[][] wordsInParagraphs)
        {
            var specialSymbols = _specialSymbols.Select(x => Convert.ToChar(x));
            var wordsCounts = new List<string[]>();

            foreach (var hiddenWordsInParagraphs in wordsInParagraphs)
            {
                var hiddenWordsLengthInText = new List<string>();
                foreach (var word in hiddenWordsInParagraphs)
                    if (word.All(x => specialSymbols.Contains(x) || _moneySymbols.Contains(x)))
                        hiddenWordsLengthInText.Add(word);
                    else
                        hiddenWordsLengthInText.Add(word.Length.ToString());
                wordsCounts.Add(hiddenWordsLengthInText.ToArray());
            }

            return wordsCounts.ToArray();
        }

        public TextDto GetTextDtoById(string textId)
        {
            var cachedTextDto = _textCache.GetTextCached(textId);
            if (cachedTextDto != null)
                return cachedTextDto;

            return ReadAndInsertToCacheText(textId);
        }

        public void Insert(TextDto textDto)
        {
            CheckText(textDto);
            Text text = GenerateTextByDto(textDto);
            _textRepository.Insert(text);
            InsertToCache(textDto, text.WordsInParagraphs);
        }

        public void Update(string id, TextDto textDto)
        {
            CheckText(textDto);
            Text text = GenerateTextByDto(textDto);
            _textRepository.Update(text);
            _textCache.Delete(id);
            InsertToCache(textDto, text.WordsInParagraphs);
        }

        public void Delete(string id)
        {
            _textRepository.Delete(id);
            _textCache.Delete(id);
        }

        private TextDto ReadAndInsertToCacheText(string textId)
        {
            var text = _textRepository.GetById(textId);
            var textDto = Mapper.Map<TextDto>(text);
            textDto.Text = GetTextFromArray(text).ToString();
            InsertToCache(textDto, text.WordsInParagraphs);
            return textDto;
        }

        private void InsertToCache(TextDto textDto, string[][] wordsInParagraphs)
        {
            var textCache = Mapper.Map<TextCache>(textDto);
            textCache.WordsInParagraphs = wordsInParagraphs;
            textCache.CountsInParagraphs = GetWordCounts(wordsInParagraphs);
            _textCache.InsertTextDto(textCache);
        }

        private StringBuilder GetTextFromArray(Text text)
        {
            var sb = new StringBuilder();
            foreach (var paragraph in text.WordsInParagraphs)
            {
                foreach (var wordOrSymbol in paragraph)
                    if (_specialSymbols.Except(new string[] { "-" }).Contains(wordOrSymbol))
                        sb.Append($"{wordOrSymbol}");
                    else
                        sb.Append($" {wordOrSymbol}");

                sb.AppendLine();
            }

            return sb;
        }

        private Text GenerateTextByDto(TextDto textDto)
        {
            var formattedText = textDto.Text.Replace("’", "'").Replace("  ", " ");
            foreach (var symbol in _specialSymbols)
                formattedText = formattedText.Replace($" {symbol}", $"{symbol}");

            var paragraphs = formattedText
                .Split(new string[] { "\r\n", "\n", "\n " }, StringSplitOptions.RemoveEmptyEntries);
            var wordsInParagraphs = new List<string[]>();
            var symbolsCount = 0;

            foreach (var paragraph in paragraphs)
            {
                var words = new List<string>();
                foreach (var word in paragraph.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                    DevideWordAndSpecialSymbols(words, word);
                symbolsCount += words.Sum(x => x.Count());
                wordsInParagraphs.Add(words.ToArray());
            }

            var text = Mapper.Map<Text>(textDto);
            text.WordsInParagraphs = wordsInParagraphs.ToArray();
            text.SymbolsCount = symbolsCount;

            return text;
        }

        private void DevideWordAndSpecialSymbols(List<string> words, string word)
        {
            var specialSymbols = _specialSymbols.Select(x => Convert.ToChar(x));
            var endIndexer = word.Length;
            var startIndexer = 0;

            if (_moneySymbols.Contains(word.First()))
            {
                words.Add(word.First().ToString());
                startIndexer++;
            }

            while (specialSymbols.Contains(word[endIndexer - 1]))
                endIndexer--;

            if (endIndexer != startIndexer)
                words.Add(word.Substring(startIndexer, endIndexer - startIndexer));

            if (word.Length != endIndexer)
                words.Add(word.Substring(endIndexer, word.Length - endIndexer));
        }

        private void CheckText(TextDto textDto)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(textDto.Title))
                sb.AppendLine($"Title shouldn`t be empty");
            if (textDto.Text == null)
                sb.AppendLine($"Text shouldn`t be empty");
            else if (textDto.Text.Length > MaxLength)
                sb.AppendLine($"System does not support texts more than {MaxLength}");

            if (sb.Length > 0)
                throw new TextException(sb.ToString());
        }
    }
}
