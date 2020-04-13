using System;

namespace Flannounce.Model.DTO.Utils
{
    public static class DtoUtils
    {
        public static Tuple<int, int> ConvertParameters(this ScrapParametersDto parametersDto)
        {
            var startPage = parametersDto?.StartPage ?? 1;
            var stopAfter = parametersDto?.StopAfter ?? 0;
            
            return new Tuple<int, int>(startPage,stopAfter);
        }
    }
}