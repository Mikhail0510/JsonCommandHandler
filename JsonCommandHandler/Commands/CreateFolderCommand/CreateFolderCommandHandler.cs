using JsonCommandHandler.AllowedParams;
using JsonCommandHandler.CreateFolderCommand;
using JsonCommandHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.Commands.CreateFolderCommand
{
    public class CreateFolderCommandHandler : ICommandHandler<FolderCommand>
    {
        public async Task HandleAsync(FolderCommand command)
        {
            string newFolderName = command.DirectoryName.Replace("{newGuid}", DirectoryNameGenerator.ReturnNewGuid());
            string destination = HandleCommandLocation(command.LocationOptions);
            destination = Path.Combine(destination, newFolderName);
            Directory.CreateDirectory(destination);
            destination = Path.Combine(destination, command.FileName);
            using (var file = File.OpenWrite(destination))
            {
                byte[] bytes = HandleCommandContent(command.Content);
                await file.WriteAsync(bytes);
            }
        }

        private string HandleCommandLocation(LocationOptions location)
        {
            string destination = String.Empty;

            if (location == LocationOptions.ROOT)
            {
                destination = Environment.CurrentDirectory;
            }else if(location == LocationOptions.DESKTOP)
            {
                destination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }else if(location == LocationOptions.DOWNLOADS){
                destination = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
            }else if(location == LocationOptions.DOCUMENTS)
            {
                destination = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            }

            return destination;
        }

        private byte[] HandleCommandContent(Content content)
        {
            byte[] bytes = new byte[0];
            if (content == Content.MY_COMPUTER_NAME)
            {
                ComputerInfoReader reader = new ComputerInfoReader();
                bytes = Encoding.UTF8.GetBytes(reader.GetComputerName());
            }
            if (content == Content.NUMBER_OF_PROCESSORS)
            {
                ComputerInfoReader reader = new ComputerInfoReader();
                bytes = Encoding.UTF8.GetBytes(reader.GetNumberOfCores());
            }
            return bytes;
        }
    }
}
