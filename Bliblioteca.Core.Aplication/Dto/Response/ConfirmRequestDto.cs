namespace Bliblioteca.Core.Aplication.Dto.Response
{
    public class ConfirmRequestDto
    {    
        public string? Message { get; set; }

        public List<string>? Error { get; set; }

        public bool HasError { get; set; }
    }
}
