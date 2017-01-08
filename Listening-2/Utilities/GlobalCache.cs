using listening.Models.TextViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using listening.Models;

namespace listening.Utilities
{
    //public interface IIdenticable<T>
    //{
    //    T Id { get; }
    //}

    //public class TextCache : TextDto, IIdenticable<string>
    //{
    //    public string[][] WordsInParagraphs { get; set; }
    //    public string[][] CountsInParagraphs { get; set; }

    //    // wrapped to make possibility of generic cache 
    //    // (probably renaming (TextId->Id) would be better idea, but we should be ensured, 
    //    // that all front-end endpoints works correct after change)
    //    public string Id
    //    {
    //        get { return TextId; }
    //        set { TextId = value; }
    //    }
    //}

    public class GlobalCache<T, Y>
        where T : IIdenticable<Y>
        where Y : IEquatable<Y>
    {
        private int MaxCount = 512;
        private ConcurrentDictionary<T, DateTime> _textDtos = new ConcurrentDictionary<T, DateTime>();

        public T GetTextCached(Y id)
        {
            var textDtoCached = _textDtos.Keys.FirstOrDefault(x => x.Id.Equals(id));
            if (textDtoCached != null)
                _textDtos[textDtoCached] = DateTime.Now;
            return textDtoCached;
        }

        public void InsertTextDto(T textCache)
        {
            RemoveIfExceed();
            _textDtos[textCache] = DateTime.Now;
        }

        public void Delete(Y id)
        {
            DateTime time;
            var exist = _textDtos.Keys.First(x => x.Id.Equals(id));
            _textDtos.TryRemove(exist, out time);
        }

        private void RemoveIfExceed()
        {
            DateTime time;
            if (_textDtos.Count <= MaxCount)
                return;
            var forDeleting = _textDtos.First(x => x.Value == _textDtos.Values.Min());
            _textDtos.TryRemove(forDeleting.Key, out time);
        }
    }

    //public class GlobalCache
    //{
    //    private int MaxCount = 512;
    //    private ConcurrentDictionary<TextCache, DateTime> _textDtos = new ConcurrentDictionary<TextCache, DateTime>();

    //    public TextCache GetTextCached(string id)
    //    {
    //        var textDtoCached = _textDtos.Keys.FirstOrDefault(x => x.TextId == id);
    //        if (textDtoCached != null)
    //            _textDtos[textDtoCached] = DateTime.Now;
    //        return textDtoCached;
    //    }

    //    public void InsertTextDto(TextCache textCache)
    //    {
    //        Console.WriteLine("Insert and remove");
    //        RemoveIfExceed();
    //        _textDtos[textCache] = DateTime.Now;
    //    }

    //    //public void Update(TextCache textCache)
    //    //{
    //    //    Delete(textCache.TextId);
    //    //    _textDtos[textCache] = DateTime.Now;
    //    //}

    //    public void Delete(string id)
    //    {
    //        DateTime time;
    //        var exist = _textDtos.Keys.First(x => x.TextId == id);
    //        _textDtos.TryRemove(exist, out time);
    //    }

    //    private void RemoveIfExceed()
    //    {
    //        DateTime time;
    //        if (_textDtos.Count <= MaxCount)
    //            return;
    //        var forDeleting = _textDtos.First(x => x.Value == _textDtos.Values.Min());
    //        _textDtos.TryRemove(forDeleting.Key, out time);
    //    }
    //}
}
