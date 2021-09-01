using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using BAL;
using DAL;
using System.Web.Script.Serialization;
using System.ServiceModel.Channels;

namespace WebApplication
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class MyService
    {
        private const int _maxRequestLenght = 100;

        [OperationContract]
        [WebInvoke(Method = "POST")]
        public string CountSimilarWordsAsString(string sentence)
        {            
            // cheking input lenght
            if (sentence.Length > _maxRequestLenght)
                return new JavaScriptSerializer().Serialize("Initial data is too long.");            

            // cheking if request is not the same
            if (Storage.LastInput != string.Empty)
            {
                if (Storage.LastInput == sentence)
                    return "";
            }
            Storage.LastInput = sentence;

            string result = string.Empty;
            
            try
            {
                List<Word> allWords = CalculateWordsStatistics(sentence);

                foreach (var word in allWords)
                {
                    result += $"{word.Name}({word.Count}) ";
                }
                result.TrimEnd();
                
                return new JavaScriptSerializer().Serialize(result);                
            }
            catch (FaultException ex)
            {
                string msg = "FaultException: " + ex.Message;
                MessageFault fault = ex.CreateMessageFault();
                if (fault.HasDetail == true)
                {
                    System.Xml.XmlReader reader = fault.GetReaderAtDetailContents();
                    if (reader.Name == "ExceptionDetail")
                    {
                        ExceptionDetail detail = fault.GetDetail<ExceptionDetail>();
                        msg += "\n\nStack Trace: " + detail.StackTrace;
                    }
                }
                return new JavaScriptSerializer().Serialize(msg);
            }
            finally
            {

            }
        }
        
        [OperationContract]
        [WebInvoke(Method = "POST")]
        public string CountSimilarWordsAsList(string sentence)
        {
            if (sentence.Length > _maxRequestLenght)
                throw new System.ArgumentException("Initial data is too long.");

            try
            {
                List<Word> allWords = CalculateWordsStatistics(sentence);

                return new JavaScriptSerializer().Serialize(allWords);
            }
            catch (FaultException ex)
            {
                string msg = "FaultException: " + ex.Message;
                MessageFault fault = ex.CreateMessageFault();
                if (fault.HasDetail == true)
                {
                    System.Xml.XmlReader reader = fault.GetReaderAtDetailContents();
                    if (reader.Name == "ExceptionDetail")
                    {
                        ExceptionDetail detail = fault.GetDetail<ExceptionDetail>();
                        msg += "\n\nStack Trace: " + detail.StackTrace;
                    }
                }
                return new JavaScriptSerializer().Serialize(msg);
            }
        }

        private List<Word> CalculateWordsStatistics(string sourceSentence)
        {
            var wordsBAL = new WordBAL();

            List<Word> allWords = wordsBAL.CalculateWordsFrequency(sourceSentence);
            allWords = allWords.OrderByDescending(p => p.Count).ToList();

            return allWords;
        }
    }
}
