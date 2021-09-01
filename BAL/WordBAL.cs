using DAL;
using Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BAL
{
    public class WordBAL
    {        
        public List<Word> CalculateWordsFrequency(string sentence)
        {
            List<Word> words = new List<Word>();

            try
            {   
                char[] separators = new char[] { ' ', '.', ',', ';', ':', '?', '\n', '\r' };
                string[] separateWords = sentence.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in separateWords)
                {
                    var existingWord = words.Where(item => StringHelper.RemoveDiacritics(item.Name) == StringHelper.RemoveDiacritics(word)).FirstOrDefault();

                    if (existingWord != null)
                    {
                        existingWord.Count++;
                    }
                    else
                    {
                        words.Add(new Word() { Name = word, Count = 1 });
                    }
                }

                return words;                
            }
            catch
            {
                throw;
            }
            finally
            {
                words = null;
            }
        }

        public IEnumerable<Word> Load()
        {
            WordDAL wordDAL = new WordDAL();

            try
            {
                return wordDAL.Load();
            }
            catch
            {
                throw;
            }
            finally
            {
                wordDAL = null;
            }
        }
    }
}
