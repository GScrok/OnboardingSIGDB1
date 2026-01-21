namespace OnboardingSIGDB1.Domain.Utils
{
    public static class CpfValidation
    {
        public static bool Validate(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return false;

            string cleanCpf = StringUtils.RemoveMask(cpf);

            if (cleanCpf.Length != 11) return false;
            
            if (cleanCpf.All(c => c == cleanCpf[0])) return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cleanCpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cleanCpf.EndsWith(digito);
        }
    }
}