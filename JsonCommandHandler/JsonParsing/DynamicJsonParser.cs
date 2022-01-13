using JsonCommandHandler.Commands.GoogleCommand;
using JsonCommandHandler.CreateFolderCommand;
using JsonCommandHandler.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.JsonParsing
{
    internal class DynamicJsonParser
    {
        private readonly ILogger _logger;

        internal DynamicJsonParser(ILogger<Program> logger)
        {
            _logger = logger;
        }

        public string GetCommandFieldValue(string stringWithJson)
        {
            dynamic JsonFields = JsonConvert.DeserializeObject(stringWithJson);
            if (JsonFields != null)
            {
                try
                {
                    return JsonFields["message-type"];
                }
                catch
                {
                    return String.Empty;
                }
            }
            return JsonFields;
        }

        public dynamic Parse(string stringWithJson)
        {
            dynamic JsonFields = JsonConvert.DeserializeObject(stringWithJson);
            
            if (JsonFields != null)
            {
                string messageType = JsonFields["message-type"];
                if (messageType == "google-search-command")
                {
                    return GetGoogleCommand(JsonFields);
                }
                if(messageType == "create-folder-with-file-command")
                {
                    return GetFolderCommand(JsonFields);
                }
            }
            return JsonFields;
        }

        private FolderCommand GetFolderCommand(dynamic jsonFields)
        {
            FolderCommand command = new FolderCommand();
            try
            {
                command.LocationOptions = jsonFields["body"]["location"];
                command.DirectoryName = jsonFields["body"]["directory-name"];
                command.FileName = jsonFields["body"]["file-name"];
                command.Content = jsonFields["body"]["content"];
                return command;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new FolderCommand();
            }
        }

        private GoogleSearchCommand GetGoogleCommand(dynamic jsonFields)
        {
            GoogleSearchCommand command = new GoogleSearchCommand();
            try
            {
                command.SearchCriterion = jsonFields["body"]["search-criterion"];
                command.DisplayLinks = jsonFields["body"]["display-links"];
                return command;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new GoogleSearchCommand();
            }
            
        }

        
    }
}
